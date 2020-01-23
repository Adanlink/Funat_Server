using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Database.Models;
using Server.SharedThings.Packets;
using Server.World.Game.Map.Chunks;
using Server.World.Game.Map.Chunks.Interfaces;
using Server.World.Game.Map.Chunks.Struct;
using Server.World.Game.Map.Entity.Interfaces;
using Server.World.Game.Map.Interfaces;

namespace Server.World.Game.Map
{
    public class Map : IMap
    {
        public Map(MapModel mapModel)
        {
            Id = mapModel.Id;
        }
        
        public Dictionary<Grid, IChunk> Chunks { get; } = new Dictionary<Grid, IChunk>();

        public Dictionary<Guid, IPlayerEntity> PlayerEntities { get; } = new Dictionary<Guid, IPlayerEntity>();
        public Dictionary<Guid, IEntity> Entities { get; } = new Dictionary<Guid, IEntity>();
        
        public long Id { get; }
        
        public void Dispose()
        {
            Save();
        }

        private void Save()
        {
            foreach (var playerEntity in PlayerEntities)
            {
                playerEntity.Value.Save();
            }
        }

        public void Update()
        {
            foreach (var player in PlayerEntities)
            {
                player.Value.Update();
            }
        }

        public void RegisterEntity<T>(T entity) where T : IEntity
        {
            switch (entity)
            {
                case null:
                    return;
                case IPlayerEntity playerEntity:
                    TryToAdd(PlayerEntities, playerEntity);
                    TryToAdd(Entities, playerEntity);
                    break;
                case ISubstantialEntity substantialEntity:
                    TryToAdd(Entities, substantialEntity);
                    break;
                default:
                    TryToAdd(Entities, entity);
                    break;
            }

            if (!(entity is ISubstantialEntity substantialEntity2)) 
            {
                return;
            }
            
            substantialEntity2.TransferEntity(
                GetChunk(
                    GridHelper.GetGridByCoordinates(substantialEntity2.X, substantialEntity2.Y)));
        }

        public IChunk GetChunk(Grid grid)
        {
            Chunks.TryGetValue(grid, out var chunk);

            if (chunk != default)
            {
                return chunk;
            }
            
            chunk = new Chunk(grid, this);
            Chunks.Add(grid, chunk);
            /*foreach (var neighborGrid in GridHelper.NeighborGrids(grid))
                {
                    if (!Chunks.TryGetValue(neighborGrid + chunk.Id, out var neighborChunk))
                    {
                        continue;
                    }
                        
                    neighborChunk.RegisterChunk(chunk);
                    chunk.RegisterChunk(neighborChunk);
                }*/

            return chunk;
        }

        private static void TryToAdd<TEntity>(Dictionary<Guid, TEntity> list, TEntity entity) where TEntity : IEntity
        {
            var test = false;
            while (!test)
            {
                test = list.TryAdd(entity.Id, entity);
                if (test)
                {
                    return;
                }
                entity.Id = Guid.NewGuid();
            }
        }

        public void UnregisterEntity<T>(T entity) where T : IEntity
        {
            switch (entity)
            {
                case null:
                    return;
                case IPlayerEntity playerEntity:
                    PlayerEntities.Remove(playerEntity.Id);
                    Entities.Remove(playerEntity.Id);
                    break;
                default:
                    Entities.Remove(entity.Id);
                    break;
            }
        }

        public Task BroadcastAsync<T>(T packet) where T : IPacket => BroadcastAsync(packet, null);

        public Task BroadcastAsync<T>(T packet, IBroadcastRule rule) where T : IPacket
        {
            return Task.WhenAll(PlayerEntities.Select(s =>
                Task.Run(() => rule == null || rule.Match(s.Value) ? s.Value.Session.SendPacketAsync(packet) : Task.CompletedTask)));
        }

        public Task BroadcastAsync<T>(IEnumerable<T> packets) where T : IPacket => BroadcastAsync(packets, null);

        public Task BroadcastAsync<T>(IEnumerable<T> packets, IBroadcastRule rule) where T : IPacket
        {
            return Task.WhenAll(PlayerEntities.Select(s =>
                Task.Run(() => rule == null || rule.Match(s.Value) ? s.Value.Session.SendPacketsAsync(packets) : Task.CompletedTask)));
        }
        
        public Task BroadcastAsync(IEnumerable<IPacket> packets) => BroadcastAsync(packets, null);

        public Task BroadcastAsync(IEnumerable<IPacket> packets, IBroadcastRule rule)
        {
            return Task.WhenAll(PlayerEntities.Select(s =>
                Task.Run(() => rule == null || rule.Match(s.Value) ? s.Value.Session.SendPacketsAsync(packets) : Task.CompletedTask)));
        }
    }
}