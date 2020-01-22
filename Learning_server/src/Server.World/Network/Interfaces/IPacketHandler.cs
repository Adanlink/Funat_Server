using System.Threading.Tasks;
using Server.SharedThings.Packets;

namespace Server.World.Network.Interfaces
{
    public interface IPacketHandler
    {
        Task Handle(IPacket packet, ISession session);
    }
}
