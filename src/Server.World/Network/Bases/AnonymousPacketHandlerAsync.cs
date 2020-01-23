using System.Threading.Tasks;
using Server.SharedThings.Packets;
using Server.World.Network.Interfaces;

namespace Server.World.Network.Bases
{
    public abstract class AnonymousPacketHandlerAsync<TPacket> : IPacketHandler
        where TPacket : IPacket
    {
        public Task Handle(IPacket packet, ISession session)
        {
            if (!(packet is TPacket typedPacket) || session == null)
            {
                return Task.CompletedTask;
            }

            return Handle(typedPacket, session);
        }

        protected abstract Task Handle(TPacket packet, ISession session);
    }
}
