using System.Threading;
using System.Threading.Tasks;
using Server.SharedThings.Packets.ServerPackets.Game;
using Server.World.Game.Map.BroadcastRules;
using Server.World.Game.Map.Chunks;
using Server.World.Game.Map.Entity.Interfaces;
using Server.World.Game.Map.Entity.Logic.Bases;
using Server.World.Game.Map.Entity.Logic.Interfaces;

namespace Server.World.Game.Map.Entity.Logic.Events.Handlers
{
    public class EntityTransferBetweenChunksHandler : GenericLogicHandlerAsync<EntityTransferBetweenChunks>
    {
        protected override Task Handle(EntityTransferBetweenChunks logicEvent, CancellationToken cancellation)
        {
            ILogicEvent firstEvent = null;
            ILogicEvent secondEvent = null;
            
            switch (logicEvent.Sender)
            {
                case IPlayerEntity playerEntity:
                    firstEvent = new PlayerEntityDeclaration
                    {
                        RealSender = playerEntity
                    };

                    secondEvent = new PlayerEntityRevocation
                    {
                        RealSender = playerEntity
                    };
                    break;
                case ISubstantialEntity substantialEntity:
                    firstEvent = new SubstantialEntityDeclaration
                    {
                        RealSender = substantialEntity
                    };
                    
                    secondEvent = new SubstantialEntityRevocation
                    {
                        RealSender = substantialEntity
                    };
                    break;
                default:
                    return Task.CompletedTask;
            }
            
            logicEvent.ToHere.BroadcastEventAsync(firstEvent, logicEvent.FromHere.Id, GridHelper.MaxChunkDistanceView);
            
            logicEvent.FromHere.BroadcastEventAsync(secondEvent, logicEvent.ToHere.Id, GridHelper.MaxChunkDistanceView);
            
            return Task.CompletedTask;
        }
    }
}