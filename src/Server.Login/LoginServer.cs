using System;
using System.Threading;
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
using Server.Crypto.Testers;
using System.Threading.Tasks;
using Server.Crypto.Hashers;
using Microsoft.EntityFrameworkCore;
using Server.Login.Network;

namespace Server.Login
{
    internal class LoginServer
    {
        private static readonly ILogger Log = Logger.GetLogger<LoginServer>();
        private static readonly ConfigurationHelper ConfigurationHelper = new ConfigurationHelper(new YamlConfigurationSerializer());
        private const string LoginConfigurationPath = "Config/login.yaml";
        private static LoginConfiguration _loginConfiguration;
        private static readonly Type Packets = new LoginLoop().GetType();
        private static Task<Argon2Hasher> _test;

        private static void Main()
        {
            PrintHeader();
            InitializeLogger();
            LoadConfig();
            SetDatabase();
            ConfigAndInitializeContainer();
            PreparePacketFactory();
            NetworkManager.RunTcpServerAsync(
                UsefulContainer.Instance.Resolve<LoginConfiguration>().Port, UsefulContainer.Instance.Resolve<ICustomTcpChannelHandler>(), UsefulContainer.Instance.Resolve<ILoop>()
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
                _loginConfiguration = ConfigurationHelper.Load<LoginConfiguration>(LoginConfigurationPath, true);
                UsefulContainer.Builder.RegisterInstance(_loginConfiguration).As<LoginConfiguration>();
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
            if (!DatabaseService.InitializeSqlDb(_loginConfiguration.DatabaseConfiguration))
            {
                Environment.Exit(1);
            }
            UsefulContainer.Builder.Register(_ => DatabaseService.GetSqlDatabase(_loginConfiguration.DatabaseConfiguration)).As<DbContext>().InstancePerDependency();
            //UsefulContainer.Builder.Register(_ => new AccountService(_.Resolve<FunatDbContext>(), _.Resolve<ILogger>())).As<IAccountService>().InstancePerLifetimeScope();
            UsefulContainer.Builder.RegisterType<AccountService>().As<IAccountService>().InstancePerLifetimeScope();
        }

        private static void ConfigAndInitializeContainer()
        {
            _test = Task.Run(() => Argon2Tester.GetBestSettings(_loginConfiguration.HasherConfiguration));
            UsefulContainer.Builder.RegisterType<LoginLoop>().As<ILoop>();
            UsefulContainer.Builder.RegisterType<LoginTcpHandler>().As<ICustomTcpChannelHandler>();
            UsefulContainer.Builder.RegisterType<PacketFactory>().As<IPacketFactory>().SingleInstance();
            UsefulContainer.Builder.RegisterType<MsgPackGameSerializer>().As<ISerializer>();
            UsefulContainer.Builder.RegisterAssemblyTypes(Packets.Assembly).AsClosedTypesOf(typeof(GenericPacketHandlerAsync<>)).PropertiesAutowired();
            UsefulContainer.Builder.Register(_ => _test.Result).As<Argon2Hasher>().InstancePerDependency();
            UsefulContainer.Initialize();
        }

        private static void PreparePacketFactory()
        {
            try
            {
                var packetFactory = UsefulContainer.Instance.Resolve<IPacketFactory>();

                foreach (var packetHandler in Packets.Assembly.GetTypesImplementingGenericClass(typeof(GenericPacketHandlerAsync<>)))
                {
                    if (!(UsefulContainer.Instance.Resolve(packetHandler) is IPacketHandler packetHandler2))
                    {
                        continue;
                    }

                    var packetType = packetHandler.BaseType.GenericTypeArguments[0];
                    packetFactory.RegisterAsync(packetHandler2, packetType).ConfigureAwait(false).GetAwaiter().GetResult();
                }
            }
            catch (Exception e)
            {
                Log.Error("", e);
            }
        }

        private static void PrintHeader()
        {
            Console.Title = "Funat Server - Login";
            const string text = @"
   ▄████████ ███    █▄  ███▄▄▄▄      ▄████████     ███           ▄█        ▄██████▄     ▄██████▄   ▄█  ███▄▄▄▄   
  ███    ███ ███    ███ ███▀▀▀██▄   ███    ███ ▀█████████▄      ███       ███    ███   ███    ███ ███  ███▀▀▀██▄ 
  ███    █▀  ███    ███ ███   ███   ███    ███    ▀███▀▀██      ███       ███    ███   ███    █▀  ███▌ ███   ███ 
 ▄███▄▄▄     ███    ███ ███   ███   ███    ███     ███   ▀      ███       ███    ███  ▄███        ███▌ ███   ███ 
▀▀███▀▀▀     ███    ███ ███   ███ ▀███████████     ███          ███       ███    ███ ▀▀███ ████▄  ███▌ ███   ███ 
  ███        ███    ███ ███   ███   ███    ███     ███          ███       ███    ███   ███    ███ ███  ███   ███ 
  ███        ███    ███ ███   ███   ███    ███     ███          ███▌    ▄ ███    ███   ███    ███ ███  ███   ███ 
  ███        ████████▀   ▀█   █▀    ███    █▀     ▄████▀        █████▄▄██  ▀██████▀    ████████▀  █▀    ▀█   █▀  
                                                                ▀                                                
";
            Console.Write(text);
            Console.WriteLine(new string('=', Console.WindowWidth));
        }
    }
}
