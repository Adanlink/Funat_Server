using System.Threading;
using System.Threading.Tasks;
using Server.SharedThings.Packets.ServerPackets.Game;
using Server.World.Game.Map.Entity.Interfaces;
using Server.World.Game.Map.Entity.Logic.Bases;

namespace Server.World.Game.Map.Entity.Logic.Events.Handlers
{
    public class PlayerEntityDeclarationHandler : GenericLogicHandlerAsync<PlayerEntityDeclaration>
    {
        protected override Task Handle(PlayerEntityDeclaration logicEvent, CancellationToken cancellation)
        {
            if (!(logicEvent.Sender is IPlayerEntity playerEntity))
            {
                if (logicEvent.Sender is ISubstantialEntity substantialEntity)
                {
                    //TODO to fix
                    /*logicEvent.RealSender.Session.SendPacket(new DeclareEntity
                    {
                        EntityBase = substantialEntity.ToSubstantialEntityRepresentation()
                    });*/
                }

                return Task.CompletedTask;
            }

            logicEvent.RealSender.Session.SendPacketAsync(new DeclareEntity
            {
                EntityBase = playerEntity.ToPlayerRepresentation()
            });
            
            playerEntity.Session.SendPacketAsync(new DeclareEntity
            {
                EntityBase = logicEvent.RealSender.ToPlayerRepresentation()
            });
            
            return Task.CompletedTask;
        }
    }
}