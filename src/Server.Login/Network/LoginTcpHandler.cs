using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.Logging;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Groups;
using DotNetty.Transport.Channels.Sockets;
using Server.Core.IoC;
using Server.Core.Logging;
using Server.Network;
using Server.Network.Packets;
using Server.Network.Packets.Serializers;
using Server.SharedThings.Packets;
using Server.SharedThings.Packets.ClientPackets;
using ISession = Server.Login.Network.Interfaces.ISession;

namespace Server.Login.Network
{
    public class LoginTcpHandler : ChannelHandlerAdapter, ICustomTcpChannelHandler, ISession
    {
        private readonly IChannel _channel;
        private static volatile IChannelGroup _group;
        private readonly IPacketFactory _packetFactory;

        private ISerializer _packetSerializer;
        private ILogger _log = Logger.GetLogger<LoginTcpHandler>();

        private IPEndPoint _endPoint;

        /// <summary>
        /// Should only be used for creating new ones.
        /// </summary>
        public LoginTcpHandler()
        {
        }

        public LoginTcpHandler(IChannel channel, IPacketFactory packetFactory)
        {
            _channel = channel;
            _packetFactory = packetFactory;
        }

        public ICustomTcpChannelHandler GetNewWithChannelHandler(ISocketChannel socketChannel)
        {
            return new LoginTcpHandler(socketChannel, UsefulContainer.Instance.Resolve<IPacketFactory>());
        }

        public override void ChannelRegistered(IChannelHandlerContext context)
        {
            var _group = LoginTcpHandler._group;
            if (_group == null)
            {
                lock (_channel)
                {
                    if (LoginTcpHandler._group == null)
                    {
                        _group = LoginTcpHandler._group = new DefaultChannelGroup(context.Executor);
                    }
                }
            }

            _endPoint = _channel.RemoteAddress as IPEndPoint;
            _group.Add(context.Channel);

            _packetSerializer = new MsgPackGameSerializer(_log, _packetFactory);
            _log = new SessionLogger(typeof(LoginTcpHandler), $"([{_endPoint?.Address}]:{_endPoint?.Port}) ");

            _log.Debug("Client has been accepted");
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var deserializedPacket = _packetSerializer.Deserialize(message as IByteBuffer);

            if (deserializedPacket == null)
            {
                return;
            }

            _packetFactory.Handle(deserializedPacket, this);
        }

        /*private Action HandleMessage(object message)
        {
            return () =>
            {
                var deserializedPacket = _packetSerializer.Deserialize(message as IByteBuffer);

                if (deserializedPacket == null)
                {
                    return;
                }

                _packetFactory.Handle(deserializedPacket, this);
            };
        }*/

        public override void ChannelUnregistered(IChannelHandlerContext context)
        {
            context.CloseAsync();
            _log.Debug("Client has disconnected");
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
            _ = WriteAnAsyncPacket(packet);
            _channel.Flush();
            return Task.CompletedTask;
        }

        public Task SendPacketsAsync<T>(IEnumerable<T> packets) where T : IPacket
        {
            if (packets == null)
            {
                return Task.CompletedTask;
            }

            foreach (T packet in packets)
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

            var _packet = _packetSerializer.Serialize(packet);

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

            var _packet = _packetSerializer.Serialize(packet);

            if (_packet == null)
            {
                return Task.CompletedTask;
            }

            return _channel.WriteAsync(_packet);
        }

        public void Disconnect()
        {
            _log.Debug("Client is being disconnected...");
            _channel.DisconnectAsync().Wait();
        }
    }
}