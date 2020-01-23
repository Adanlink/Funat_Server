using System;
using System.Threading.Tasks;
using Server.Login.Network.Interfaces;
using Server.SharedThings.Packets;

namespace Server.Login.Network
{
    public interface IPacketFactory
    {
        Task RegisterAsync(IPacketHandler handler, Type packetType);

        Task UnregisterAsync(IPacketHandler handler, Type packetType);

        Task Handle(IPacket packet, ISession session);

        Type GetPacketById(uint identifier);
    }
}
