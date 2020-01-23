using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using Server.SharedThings.Packets;
using Server.World.Network.Interfaces;

namespace Server.World.Network
{
    public class PacketFactory : IPacketFactory
    {
        private readonly IDictionary<uint, Type> _packetsIdentities = new Dictionary<uint, Type>();
        private readonly IDictionary<Type, IPacketHandler> _packetsHandlers = new Dictionary<Type, IPacketHandler>();
        
        private readonly ILogger _log;

        public PacketFactory(ILogger log)
        {
            _log = log;
        }
        
        /// <summary>
        /// If it couldn't find it, it return null.
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns>The type of the packet identified with that identification.</returns>
        public Type GetPacketById(uint identifier)
        {
            return !_packetsIdentities.TryGetValue(identifier, out var type) ? null : type;
        }

        public Task Handle(IPacket packet, ISession session)
        {
            if (!_packetsHandlers.TryGetValue(packet.GetType(), out var handler))
            {
                return Task.CompletedTask;
            }

            handler.Handle(packet, session);
            return Task.CompletedTask;
        }

        public Task RegisterAsync(IPacketHandler handler, Type packetType)
        {
            if (_packetsHandlers.ContainsKey(packetType))
            {
                return Task.CompletedTask;
            }
            _packetsHandlers[packetType] = handler;
            _packetsIdentities.Add(
                packetType.GetCustomAttribute<PacketPropertiesAttribute>().Identifier, packetType);
            return Task.CompletedTask;
        }

        public Task UnregisterAsync(IPacketHandler handler, Type packetType)
        {
            _packetsHandlers.Remove(packetType);
            _packetsIdentities.Remove(packetType.GetCustomAttribute<PacketPropertiesAttribute>().Identifier);
            return Task.CompletedTask;
        }
    }
}
