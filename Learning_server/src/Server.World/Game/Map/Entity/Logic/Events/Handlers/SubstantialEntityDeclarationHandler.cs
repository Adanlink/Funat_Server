using System.Threading;
using System.Threading.Tasks;
using Server.SharedThings.Packets.ServerPackets.Game;
using Server.World.Game.Map.Entity.Interfaces;
using Server.World.Game.Map.Entity.Logic.Bases;
using Server.World.Game.Map.Entity.Logic.Interfaces;

namespace Server.World.Game.Map.Entity.Logic.Events.Handlers
{
    public class SubstantialEntityDeclarationHandler : GenericLogicHandlerAsync<SubstantialEntityDeclaration>
    {
        protected override Task Handle(SubstantialEntityDeclaration logicEvent, CancellationToken cancellation)
        {
            if (!(logicEvent.Sender is IPlayerEntity playerEntity))
            {
                return Task.CompletedTask;
            }
            
            /*playerEntity.Session.SendPacketAsync(new DeclareEntity
            {
                EntityBase = logicEvent.RealSender.ToSubstantialEntityRepresentation()
            });*/
            return Task.CompletedTask;
        }
    }
}