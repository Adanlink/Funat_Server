using System;
using System.Threading.Tasks;
using Server.SharedThings.Packets;

namespace Server.World.Network.Interfaces
{
    public interface IPacketFactory
    {
        Task RegisterAsync(IPacketHandler handler, Type packetType);

        Task UnregisterAsync(IPacketHandler handler, Type packetType);

        Task Handle(IPacket packet, ISession session);

        Type GetPacketById(uint identifier);
    }
}
