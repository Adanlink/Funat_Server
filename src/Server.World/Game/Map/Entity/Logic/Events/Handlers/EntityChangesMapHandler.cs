using System.Threading;
using System.Threading.Tasks;
using Server.SharedThings.Packets.ServerPackets.Game;
using Server.World.Game.Map.BroadcastEventRules;
using Server.World.Game.Map.BroadcastRules;
using Server.World.Game.Map.Chunks;
using Server.World.Game.Map.Entity.Interfaces;
using Server.World.Game.Map.Entity.Logic.Bases;
using AllExceptOne = Server.World.Game.Map.BroadcastEventRules.AllExceptOne;

namespace Server.World.Game.Map.Entity.Logic.Events.Handlers
{
    public class EntityChangesMapHandler : GenericLogicHandlerAsync<EntityChangesMap>
    {
        protected override Task Handle(EntityChangesMap logicEvent, CancellationToken cancellation)
        {
            switch (logicEvent.Sender)
            {
                case IPlayerEntity playerEntity:
                    /*logicEvent.FromHere?.BroadcastAsync(new RevokeEntity
                    {
                        Id = playerEntity.Id.ToString()
                    });*/
                    var tempRule = new AllExceptOne(playerEntity);
                    
                    logicEvent.FromHere?.BroadcastEventAsync(new PlayerEntityRevocation
                    {
                        RealSender = playerEntity
                    }, tempRule, GridHelper.MaxChunkDistanceView, true).Wait();
                    
                    playerEntity.Session.SendPacketAsync(new PlayerUpdateOwnPosition
                    {
                        X = playerEntity.X,
                        Y = playerEntity.Y
                    });

                    logicEvent.ToHere?.BroadcastEventAsync(new PlayerEntityDeclaration
                    {
                        RealSender = playerEntity
                    }, tempRule, GridHelper.MaxChunkDistanceView);
                    break;
                
                case ISubstantialEntity substantialEntity:
                    logicEvent.FromHere?.BroadcastEventAsync(new SubstantialEntityRevocation
                    {
                        RealSender = substantialEntity
                    }, GridHelper.MaxChunkDistanceView, true).Wait();
                    
                    logicEvent.ToHere?.BroadcastEventAsync(new SubstantialEntityDeclaration
                    {
                        RealSender = substantialEntity
                    }, GridHelper.MaxChunkDistanceView);
                    break;
            }
            return Task.CompletedTask;
        }
    }
}