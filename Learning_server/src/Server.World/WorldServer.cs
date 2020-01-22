using System;
using System.Reflection.Metadata;
using Server.Core.Configuration;
using Server.Database;
using NLog;
using ChickenAPI.Core.Configurations;
using Logger = Server.Core.Logging.Logger;
using ILogger = ChickenAPI.Core.Logging.ILogger;
using Server.Network;
using Server.Network.Packets;
using Server.Core.IoC;
using Autofac;
using Server.Network.Packets.Serializers;
using Server.Database.Context;
using Server.Database.Services;
using Server.Database.Services.Interfaces;
using ChickenAPI.Core.Utils;
using Microsoft.EntityFrameworkCore;
using Server.World.Configuration;
using Server.World.Game.Map;
using Server.World.Game.Map.Entity.Logic;
using Server.World.Game.Map.Entity.Logic.Bases;
using Server.World.Game.Map.Entity.Logic.Interfaces;
using Server.World.Game.Map.Interfaces;
using Server.World.Network;
using Server.World.Network.Bases;
using Server.World.Network.Interfaces;

namespace Server.World
{
    internal class WorldServer
    {
        private static readonly ILogger Log = Logger.GetLogger<WorldServer>();
        private static readonly ConfigurationHelper ConfigurationHelper = new ConfigurationHelper(new YamlConfigurationSerializer());
        private const string WorldConfigurationPath = "Config/config.yaml";
        private static WorldConfiguration _worldConfiguration;
        private static readonly Type Packets = new WorldLoop().GetType();

        private static void Main()
        {
            PrintHeader();
            InitializeLogger();
            LoadConfig();
            SetDatabase();
            ConfigAndInitializeContainer();
            PreparePacketFactory();
            NetworkManager.RunTcpServerAsync(
                UsefulContainer.Instance.Resolve<WorldConfiguration>().Port,
                UsefulContainer.Instance.Resolve<ICustomTcpChannelHandler>(), 
                UsefulContainer.Instance.Resolve<ILoop>()
                ).Wait();
            LogManager.Shutdown();
        }

        private static void InitializeLogger()
        {
            UsefulContainer.Builder.Register(s => Logger.GetLogger(s.GetType())).As<ILogger>().InstancePerDependency();
            Logger.Initialize();
        }

        private static void LoadConfig()
        {
            Log.Info("Loading configuration...");

            try
            {
                _worldConfiguration = ConfigurationHelper.Load<WorldConfiguration>(WorldConfigurationPath, true);
                UsefulContainer.Builder.RegisterInstance(_worldConfiguration).As<WorldConfiguration>();
                Log.Info("Configuration loaded!");
            }
            catch (Exception e)
            {
                Log.Fatal("There was an error loading the configuration: ", e);
                Environment.Exit(1);
            }
        }

        private static void SetDatabase()
        {
            Log.Info("Loading Database...");
            if (!DatabaseService.InitializeSqlDb(_worldConfiguration.DatabaseConfiguration))
            {
                Environment.Exit(1);
            }
            UsefulContainer.Builder.Register(_ => DatabaseService.GetSqlDatabase(_worldConfiguration.DatabaseConfiguration)).As<DbContext>().InstancePerDependency();
            //UsefulContainer.Builder.Register(_ => new AccountService(_.Resolve<DbContext>(), _.Resolve<ILogger>())).As<IAccountService>().InstancePerLifetimeScope();
            UsefulContainer.Builder.RegisterType<AccountService>().As<IAccountService>().InstancePerLifetimeScope();
            UsefulContainer.Builder.RegisterType<CharacterService>().As<ICharacterService>().InstancePerLifetimeScope();
            UsefulContainer.Builder.RegisterType<MapService>().As<IMapService>().InstancePerLifetimeScope();
        }

        private static void ConfigAndInitializeContainer()
        {
            UsefulContainer.Builder.RegisterType<WorldLoop>().As<ILoop>();
            UsefulContainer.Builder.RegisterType<WorldTcpHandler>().As<ICustomTcpChannelHandler>();
            UsefulContainer.Builder.RegisterType<PacketFactory>().As<IPacketFactory>().SingleInstance();
            UsefulContainer.Builder.RegisterType<LogicFactory>().As<ILogicFactory>().SingleInstance();
            UsefulContainer.Builder.RegisterType<MsgPackGameSerializer>().As<ISerializer>().SingleInstance();
            UsefulContainer.Builder.RegisterType<SessionManager>().As<ISessionManager>().SingleInstance();
            UsefulContainer.Builder.RegisterType<MapManager>().As<IMapManager>().SingleInstance();
            UsefulContainer.Builder.RegisterAssemblyTypes(Packets.Assembly).AsClosedTypesOf(typeof(AnonymousPacketHandlerAsync<>)).PropertiesAutowired();
            UsefulContainer.Builder.RegisterAssemblyTypes(Packets.Assembly).AsClosedTypesOf(typeof(AuthenticatedPacketHandlerAsync<>)).PropertiesAutowired();
            UsefulContainer.Builder.RegisterAssemblyTypes(Packets.Assembly).AsClosedTypesOf(typeof(CharacterPacketHandlerAsync<>)).PropertiesAutowired();
            UsefulContainer.Builder.RegisterAssemblyTypes(Packets.Assembly).AsClosedTypesOf(typeof(GenericLogicHandlerAsync<>)).PropertiesAutowired();
            UsefulContainer.Initialize();
        }

        private static void PrepareFactories()
        {
            try
            {
                PreparePacketFactory();
                PrepareLogicFactory();
            }
            catch (Exception e)
            {
                Log.Error("", e);
            }
        }

        private static void PreparePacketFactory()
        {
            var packetFactory = UsefulContainer.Instance.Resolve<IPacketFactory>();

            var listOfHandlers = Packets.Assembly.GetTypesImplementingGenericClass(typeof(AnonymousPacketHandlerAsync<>));
            listOfHandlers.AddRange(Packets.Assembly.GetTypesImplementingGenericClass(typeof(AuthenticatedPacketHandlerAsync<>)));
            listOfHandlers.AddRange(Packets.Assembly.GetTypesImplementingGenericClass(typeof(CharacterPacketHandlerAsync<>)));

            foreach (var rawPacketHandler in listOfHandlers)
            {
                if (!(UsefulContainer.Instance.Resolve(rawPacketHandler) is IPacketHandler packetHandler))
                {
                    return;
                }
                
                packetFactory.RegisterAsync(packetHandler, rawPacketHandler.BaseType.GenericTypeArguments[0]).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            
            var logicFactory = UsefulContainer.Instance.Resolve<ILogicFactory>();

            foreach (var rawLogicHandler in Packets.Assembly.GetTypesImplementingGenericClass(typeof(GenericLogicHandlerAsync<>)))
            {
                if (!(UsefulContainer.Instance.Resolve(rawLogicHandler) is ILogicHandler logicHandler))
                {
                    return;
                }

                logicFactory.RegisterAsync(logicHandler, rawLogicHandler.BaseType.GenericTypeArguments[0]).ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }
        
        private static void PrepareLogicFactory()
        {
            var packetFactory = UsefulContainer.Instance.Resolve<ILogicFactory>();

            foreach (var rawLogicHandler in Packets.Assembly.GetTypesImplementingGenericClass(typeof(GenericLogicHandlerAsync<>)))
            {
                if (!(UsefulContainer.Instance.Resolve(rawLogicHandler) is ILogicHandler logicHandler))
                {
                    continue;
                }
                
                var packetType = rawLogicHandler.BaseType.GenericTypeArguments[0];
                packetFactory.RegisterAsync(logicHandler, packetType).ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }

        /*private static void PrepareFactory(Type factoryInterface, Type handlerAbstract, Type handlerInterface)
        {
            var factory = UsefulContainer.Instance.Resolve(factoryInterface);

            foreach (var rawLogicHandler in Packets.Assembly.GetTypesImplementingGenericClass(handlerAbstract))
            {
                var logicHandler = UsefulContainer.Instance.Resolve(rawLogicHandler);
                if (!logicHandler.GetType().ImplementsInterface(handlerInterface))
                {
                    continue;
                }
                
                var packetType = rawLogicHandler.BaseType.GenericTypeArguments[0];
                factory.RegisterAsync(logicHandler, packetType).ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }*/

        private static void PrintHeader()
        {
            Console.Title = "Funat Server - World";
            const string text = @"
███████╗██╗   ██╗███╗   ██╗ █████╗ ████████╗    ██╗    ██╗ ██████╗ ██████╗ ██╗     ██████╗ 
██╔════╝██║   ██║████╗  ██║██╔══██╗╚══██╔══╝    ██║    ██║██╔═══██╗██╔══██╗██║     ██╔══██╗
█████╗  ██║   ██║██╔██╗ ██║███████║   ██║       ██║ █╗ ██║██║   ██║██████╔╝██║     ██║  ██║
██╔══╝  ██║   ██║██║╚██╗██║██╔══██║   ██║       ██║███╗██║██║   ██║██╔══██╗██║     ██║  ██║
██║     ╚██████╔╝██║ ╚████║██║  ██║   ██║       ╚███╔███╔╝╚██████╔╝██║  ██║███████╗██████╔╝
╚═╝      ╚═════╝ ╚═╝  ╚═══╝╚═╝  ╚═╝   ╚═╝        ╚══╝╚══╝  ╚═════╝ ╚═╝  ╚═╝╚══════╝╚═════╝ 
";
            Console.Write(text);
            Console.WriteLine(new string('=', Console.WindowWidth));
        }
    }
}
