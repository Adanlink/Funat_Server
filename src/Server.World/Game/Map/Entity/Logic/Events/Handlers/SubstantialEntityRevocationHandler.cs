using System.Threading;
using System.Threading.Tasks;
using Server.SharedThings.Packets.ServerPackets.Game;
using Server.World.Game.Map.Entity.Interfaces;
using Server.World.Game.Map.Entity.Logic.Bases;
using Server.World.Game.Map.Entity.Logic.Interfaces;

namespace Server.World.Game.Map.Entity.Logic.Events.Handlers
{
    public class SubstantialEntityRevocationHandler : GenericLogicHandlerAsync<SubstantialEntityRevocation>
    {
        protected override Task Handle(SubstantialEntityRevocation logicEvent, CancellationToken cancellation)
        {
            if (!(logicEvent.Sender is IPlayerEntity playerEntity))
            {
                return Task.CompletedTask;
            }
            
            playerEntity.Session.SendPacketAsync(new RevokeEntity
            {
                Id = logicEvent.RealSender.Id.ToString()
            });
            return Task.CompletedTask;
        }
    }
}