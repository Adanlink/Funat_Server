using System.Threading.Tasks;
using Server.Login.Network.Interfaces;
using Server.SharedThings.Packets;

namespace Server.Login.Network
{
    public interface IPacketHandler
    {
        Task Handle(IPacket packet, ISession session);
    }
}
