using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Groups;
using DotNetty.Transport.Channels.Sockets;
using Server.Core.Logging;
using Server.Network.Packets.Serializers;
using Server.Core.IoC;
using Autofac;
using Server.Database.Models;
using Server.Network.Packets;
using Server.SharedThings.Packets;
using Server.SharedThings.Packets.ServerPackets;
using Server.SharedThings.Packets.ServerPackets.Enums;
using Server.World.Game.Map.Entity;
using Server.World.Game.Map.Entity.Interfaces;
using Server.World.Game.Map.Interfaces;
using Server.World.Network.Interfaces;

namespace Server.World.Network
{
    public class WorldTcpHandler : ChannelHandlerAdapter, ICustomTcpChannelHandler, ISession
    {
        private ILogger _log = Logger.GetLogger<WorldTcpHandler>();
        
        private readonly IChannel _channel;
        private static volatile IChannelGroup _group;
        
        private static readonly IPacketFactory PacketFactory =
            new Lazy<IPacketFactory>(() => UsefulContainer.Instance.Resolve<IPacketFactory>()).Value;
        private static readonly ISerializer PacketSerializer =
            new Lazy<ISerializer>(() => UsefulContainer.Instance.Resolve<ISerializer>()).Value;
        
        private static readonly ISessionManager SessionManager =
            new Lazy<ISessionManager>(() => UsefulContainer.Instance.Resolve<ISessionManager>()).Value;
        private static readonly IMapManager MapManager =
            new Lazy<IMapManager>(() => UsefulContainer.Instance.Resolve<IMapManager>()).Value;

        public IPEndPoint EndPoint { get; private set; }

        public AccountModel Account { get; private set; }

        public bool IsAuthenticated => Account != default;

        public bool IsPlaying => Player != default;

        public IPlayerEntity Player { get; private set; } = null;

        /// <summary>
        /// Should only be used for creating new ones.
        /// </summary>
        public WorldTcpHandler()
        {
        }

        public WorldTcpHandler(IChannel channel)
        {
            _channel = channel;
        }

        public ICustomTcpChannelHandler GetNewWithChannelHandler(ISocketChannel socketChannel)
        {
            return new WorldTcpHandler(socketChannel);
        }
        
        public void LoadAccount(AccountModel accountModel)
        {
            Account = accountModel;
        }

        public void LoadCharacter(CharacterModel characterModel)
        {
            Player?.Dispose();
            Player = new PlayerEntity(MapManager.GetMap(characterModel.MapId), characterModel, this);
        }

        public override void ChannelRegistered(IChannelHandlerContext context)
        {
            var group = _group;
            if (group == null)
            {
                lock (_channel)
                {
                    if (_group == null)
                    {
                        group = _group = new DefaultChannelGroup(context.Executor);
                    }
                }
            }

            EndPoint = _channel.RemoteAddress as IPEndPoint;
            
            group.Add(context.Channel);
            
            _log = new SessionLogger(typeof(WorldTcpHandler), $"([{EndPoint?.Address}]:{EndPoint?.Port}) ");
            
            if (!SessionManager.RegisterAsync(this).Result)
            {
                SendPacket(new SessionLoginResponse
                {
                    SessionLoginResponseType = SessionLoginResponseType.MaxConnectionsReached
                });
                _log.Debug("Client has reached maximum connections per Ip, disconnecting!");
                Disconnect();
                return;
            }

            _log.Debug("Client has been accepted");
        }
        
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var deserializedPacket = PacketSerializer.Deserialize(message as IByteBuffer);

            if (deserializedPacket == null)
            {
                return;
            }

            PacketFactory.Handle(deserializedPacket, this);
        }
        
        public override void ChannelUnregistered(IChannelHandlerContext context)
        {
            Disconnect();
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            _log.Error("", exception);
            Disconnect();
        }

        public void SendPacket<T>(T packet) where T : IPacket
        {
            WriteAPacket(packet);
            _channel.Flush();
        }

        public void SendPackets<T>(IEnumerable<T> packets) where T : IPacket
        {
            if (packets == null)
            {
                return;
            }

            foreach(var packet in packets)
            {
                WriteAPacket(packet);
            }
            _channel.Flush();
        }

        public void SendPackets(IEnumerable<IPacket> packets)
        {
            if (packets == null)
            {
                return;
            }

            foreach (var packet in packets)
            {
                WriteAPacket(packet);
            }
            _channel.Flush();
        }

        public Task SendPacketAsync<T>(T packet) where T : IPacket
        {
            WriteAnAsyncPacket(packet);
            _channel.Flush();
            return Task.CompletedTask;
        }

        public Task SendPacketsAsync<T>(IEnumerable<T> packets) where T : IPacket
        {
            if (packets == null)
            {
                return Task.CompletedTask;
            }

            foreach (var packet in packets)
            {
                WriteAnAsyncPacket(packet);
            }
            _channel.Flush();
            return Task.CompletedTask;
        }

        public Task SendPacketsAsync(IEnumerable<IPacket> packets)
        {
            if (packets == null)
            {
                return Task.CompletedTask;
            }

            foreach (var packet in packets)
            {
                WriteAnAsyncPacket(packet);
            }
            _channel.Flush();
            return Task.CompletedTask;
        }

        private void WriteAPacket<T>(T packet) where T : IPacket
        {
            if (packet == null)
            {
                return;
            }

            var _packet = PacketSerializer.Serialize(packet);

            if (_packet == null)
            {
                return;
            }

            _channel.WriteAsync(_packet);
        }

        private Task WriteAnAsyncPacket<T>(T packet) where T : IPacket
        {
            if (packet == null)
            {
                return Task.CompletedTask;
            }

            var _packet = PacketSerializer.Serialize(packet);

            return _packet == null ? Task.CompletedTask : _channel.WriteAsync(_packet);
        }

        public void Disconnect()
        {
            _log.Debug("Client is being disconnected...");
            SessionManager.UnregisterAsync(this);
            if (IsPlaying)
            {
                Player.Dispose();
                Player = null;
            }
            
            _channel.DisconnectAsync().Wait();
            _log.Debug("Client has disconnected");
        }
    }
}