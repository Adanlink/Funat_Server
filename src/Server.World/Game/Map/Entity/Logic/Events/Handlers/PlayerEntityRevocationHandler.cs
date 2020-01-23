using System.Threading;
using System.Threading.Tasks;
using Server.SharedThings.Packets.ServerPackets.Game;
using Server.World.Game.Map.Entity.Interfaces;
using Server.World.Game.Map.Entity.Logic.Bases;

namespace Server.World.Game.Map.Entity.Logic.Events.Handlers
{
    public class PlayerEntityRevocationHandler : GenericLogicHandlerAsync<PlayerEntityRevocation>
    {
        protected override Task Handle(PlayerEntityRevocation logicEvent, CancellationToken cancellation)
        {
            if (!(logicEvent.Sender is IPlayerEntity playerEntity))
            {
                if (logicEvent.Sender is ISubstantialEntity substantialEntity)
                {
                    logicEvent.RealSender.Session.SendPacket(new RevokeEntity
                    {
                        Id = substantialEntity.Id.ToString()
                    });
                }

                return Task.CompletedTask;
            }
            
            logicEvent.RealSender.Session.SendPacket(new RevokeEntity
            {
                Id = playerEntity.Id.ToString()
            });
            
            playerEntity.Session.SendPacket(new RevokeEntity
            {
                Id = logicEvent.RealSender.Id.ToString()
            });
            
            return Task.CompletedTask;
        }
    }
}