using System.Threading.Tasks;
using Server.SharedThings.Packets.ClientPackets.Game;
using Server.World.Network.Bases;
using Server.World.Network.Interfaces;

namespace Server.World.Network.PacketHandlers.Character
{
    public class MovementDirectionRequestHandler : CharacterPacketHandlerAsync<ClientDeclareMovementDirection>
    {
        protected override Task Handle(ClientDeclareMovementDirection packet, ISession session)
        {
            session.Player.MovableComponent.MovementDirection = packet.MovementDirection;
            return Task.CompletedTask;
        }
    }
}