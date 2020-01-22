using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Server.Core.IoC;
using Server.SharedThings.Packets.ServerPackets.Game;
using Server.World.Configuration;
using Server.World.Game.Map.BroadcastRules;
using Server.World.Game.Map.Chunks;
using Server.World.Game.Map.Entity.Interfaces;
using Server.World.Game.Map.Entity.Logic.Bases;

namespace Server.World.Game.Map.Entity.Logic.Events.Handlers
{
    public class EntityMovementUpdateHandler : GenericLogicHandlerAsync<EntityMovementUpdate>
    {
        protected override Task Handle(EntityMovementUpdate logicEvent, CancellationToken cancellation)
        {
            if (!(logicEvent.Sender is ISubstantialEntity tempSubstantialEntity))
            {
                return Task.CompletedTask;
            }

            var tempDestinationGrid = GridHelper.GetGridByCoordinates(tempSubstantialEntity.X, tempSubstantialEntity.Y);
            if (tempSubstantialEntity.CurrentChunk.Id != tempDestinationGrid)
            {
                tempSubstantialEntity.TransferEntity(tempSubstantialEntity.CurrentMap.GetChunk(tempDestinationGrid));
            }

            switch (logicEvent.Sender)
            {
                case IPlayerEntity player:
                    player.Session.SendPacketAsync(new PlayerUpdateOwnPosition
                    {
                        X = player.X,
                        Y = player.Y
                    });
                    player.CurrentChunk.BroadcastAsync(new EntityUpdatePosition
                    {
                        Id = player.Id.ToString(),
                        X = player.X,
                        Y = player.Y
                    }, new AllExceptOne(player), GridHelper.MaxChunkDistanceView);
                    break;
                case ISubstantialEntity substantialEntity:
                    substantialEntity.CurrentChunk.BroadcastAsync(new EntityUpdatePosition
                    {
                        Id = substantialEntity.Id.ToString(),
                        X = substantialEntity.X,
                        Y = substantialEntity.Y
                    }, GridHelper.MaxChunkDistanceView);
                    break;
            }

            return Task.CompletedTask;
        }
    }
}