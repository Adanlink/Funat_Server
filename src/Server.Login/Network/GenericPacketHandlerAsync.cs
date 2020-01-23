using System.Threading.Tasks;
using Server.Login.Network.Interfaces;
using Server.SharedThings.Packets;

namespace Server.Login.Network
{
    public abstract class GenericPacketHandlerAsync<TPacket> : IPacketHandler
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
