using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Server.SharedThings.Packets;
using Server.World.Game.Map.Chunks.Enums;
using Server.World.Game.Map.Chunks.Interfaces;
using Server.World.Game.Map.Chunks.Struct;
using Server.World.Game.Map.Entity.Interfaces;
using Server.World.Game.Map.Entity.Logic.Interfaces;
using Server.World.Game.Map.Interfaces;

namespace Server.World.Game.Map.Chunks
{
    public class Chunk : IChunk
    {
        public Grid Id { get; }
        
        public IMap Map { get; }

        //public Dictionary<Grid, IChunk> NeighborChunks { get; } = new Dictionary<Grid, IChunk>();
        
        public Dictionary<Guid, IPlayerEntity> PlayerEntities { get; } = new Dictionary<Guid, IPlayerEntity>();
        
        public Dictionary<Guid, ISubstantialEntity> SubstantialEntities { get; } = new Dictionary<Guid, ISubstantialEntity>();
        
        public Chunk(Grid id, IMap map)
        {
            Id = id;
            Map = map;
        }
        
        public void Dispose()
        {
            Map.Chunks.Remove(Id);
        }
        
        public void Update()
        {
            throw new NotImplementedException();
        }

        public void RegisterEntity<T>(T entity) where T : ISubstantialEntity
        {
            switch (entity)
            {
                case null:
                    return;
                case IPlayerEntity playerEntity:
                    PlayerEntities.Add(playerEntity.Id, playerEntity);
                    SubstantialEntities.Add(entity.Id, entity);
                    break;
                default:
                    SubstantialEntities.Add(entity.Id, entity);
                    break;
            }
        }

        public void UnregisterEntity<T>(T entity) where T : ISubstantialEntity
        {
            switch (entity)
            {
                case null:
                    break;
                case IPlayerEntity playerEntity:
                    PlayerEntities.Remove(playerEntity.Id);
                    SubstantialEntities.Remove(playerEntity.Id);
                    break;
                default:
                    SubstantialEntities.Remove(entity.Id);
                    break;
            }

            if (SubstantialEntities.Count == 0)
            {
                Dispose();
            }
        }
        
        /*public void RegisterChunk(IChunk chunk)
        {
            NeighborChunks.Add(chunk.Id, chunk);
        }

        public void UnregisterChunk(IChunk chunk)
        {
            NeighborChunks.Remove(chunk.Id);
        }*/

        public Task BroadcastAsync<T>(T packet, byte chunkDistance = 0) where T : IPacket =>
            BroadcastAsync(packet, null, chunkDistance);

        public Task BroadcastAsync<T>(T packet, IBroadcastRule rule, byte chunkDistance = 0) where T : IPacket
        {
            //var tempTaskList = BroadcastAsyncHere(logicEvent, rule);

            if (chunkDistance == 0)
            {
                return Task.WhenAll(BroadcastAsyncHere(packet, rule));
            }

            List<Task> tempTaskList = new List<Task>();

            foreach (var grid in GridHelper.GetGridsByDistanceAndOrigin(chunkDistance, Id))
            {
                if (Map.Chunks.TryGetValue(grid, out var chunk))
                {
                    tempTaskList.Add(chunk.BroadcastAsync(packet, rule));
                }
            }
            
            return Task.WhenAll(tempTaskList);
        }

        private List<Task> BroadcastAsyncHere(IPacket packet, IBroadcastRule rule)
        {
            return PlayerEntities.Select(s =>
                Task.Run(() =>
                    rule == null || rule.Match(s.Value)
                        ? s.Value.Session.SendPacketAsync(packet)
                        : Task.CompletedTask)).ToList();
        }

        public Task BroadcastAsync<T>(IEnumerable<T> packets, byte chunkDistance = 0) where T : IPacket =>
            BroadcastAsync(packets, null, chunkDistance);

        public Task BroadcastAsync<T>(IEnumerable<T> packets, IBroadcastRule rule, byte chunkDistance = 0) where T : IPacket
        {
            //var tempTaskList = BroadcastAsyncHere(packets, rule);

            if (chunkDistance == 0)
            {
                return Task.WhenAll(BroadcastAsyncHere(packets, rule));
            }

            List<Task> tempTaskList = new List<Task>();

            foreach (var grid in GridHelper.GetGridsByDistanceAndOrigin(chunkDistance, Id))
            {
                if (Map.Chunks.TryGetValue(grid, out var chunk))
                {
                    tempTaskList.Add(chunk.BroadcastAsync(packets, rule));
                }
            }
            
            return Task.WhenAll(tempTaskList);
        }

        private static List<Task> BroadcastAsyncHere<T>(IEnumerable<T> packets, IBroadcastRule rule) where T : IPacket
        {
            return BroadcastAsyncHere(packets, rule);
        }

        private List<Task> BroadcastAsyncHere(IEnumerable<IPacket> packets, IBroadcastRule rule)
        {
            return PlayerEntities.Select(s =>
                Task.Run(() =>
                    rule == null || rule.Match(s.Value)
                        ? s.Value.Session.SendPacketsAsync(packets)
                        : Task.CompletedTask)).ToList();
        }
        
        public Task BroadcastAsync(IEnumerable<IPacket> packets, byte chunkDistance = 0) =>
            BroadcastAsync(packets, null, chunkDistance);

        public Task BroadcastAsync(IEnumerable<IPacket> packets, IBroadcastRule rule, byte chunkDistance = 0)
        {
            //var tempTaskList = BroadcastAsyncHere(packets, rule);

            if (chunkDistance == 0)
            {
                return Task.WhenAll(BroadcastAsyncHere(packets, rule));
            }

            List<Task> tempTaskList = new List<Task>();

            foreach (var grid in GridHelper.GetGridsByDistanceAndOrigin(chunkDistance, Id))
            {
                if (Map.Chunks.TryGetValue(grid, out var chunk))
                {
                    tempTaskList.Add(chunk.BroadcastAsync(packets, rule));
                }
            }
            
            return Task.WhenAll(tempTaskList);
        }

        public Task BroadcastAsync<T>(T packet, Grid fromHere, byte chunkDistance = 0) where T : IPacket
            => BroadcastAsync(packet, null, fromHere, chunkDistance);

        public Task BroadcastAsync<T>(T packet, IBroadcastRule rule, Grid fromHere, byte chunkDistance = 0) where T : IPacket
        {
            //var tempTaskList = BroadcastAsyncHere(packet, rule);

            if (chunkDistance == 0)
            {
                return Task.WhenAll(BroadcastAsyncHere(packet, rule));
            }

            List<Task> tempTaskList = new List<Task>();

            foreach (var grid in GridHelper.GetNonEqualGridsByDistanceOriginAndDestination(chunkDistance, fromHere, Id))
            {
                if (Map.Chunks.TryGetValue(grid, out var chunk))
                {
                    tempTaskList.Add(chunk.BroadcastAsync(packet, rule));
                }
            }
            
            return Task.WhenAll(tempTaskList);
        }

        public Task BroadcastAsync<T>(IEnumerable<T> packets, Grid fromHere, byte chunkDistance = 0)
            where T : IPacket
            => BroadcastAsync(packets, null, fromHere, chunkDistance);

        public Task BroadcastAsync<T>(IEnumerable<T> packets, IBroadcastRule rule, Grid fromHere, byte chunkDistance = 0) where T : IPacket
        {
            //var tempTaskList = BroadcastAsyncHere(packets, rule);

            if (chunkDistance == 0)
            {
                return Task.WhenAll(BroadcastAsyncHere(packets, rule));
            }

            List<Task> tempTaskList = new List<Task>();

            foreach (var grid in GridHelper.GetNonEqualGridsByDistanceOriginAndDestination(chunkDistance, fromHere, Id))
            {
                if (Map.Chunks.TryGetValue(grid, out var chunk))
                {
                    tempTaskList.Add(chunk.BroadcastAsync(packets, rule));
                }
            }
            
            return Task.WhenAll(tempTaskList);
        }

        public Task BroadcastAsync(IEnumerable<IPacket> packets, Grid fromHere, byte chunkDistance = 0)
            => BroadcastAsync(packets, null, fromHere, chunkDistance);

        public Task BroadcastAsync(IEnumerable<IPacket> packets, IBroadcastRule rule, Grid fromHere, byte chunkDistance = 0)
        {
            //var tempTaskList = BroadcastAsyncHere(packets, rule);

            if (chunkDistance == 0)
            {
                return Task.WhenAll(BroadcastAsyncHere(packets, rule));
            }

            List<Task> tempTaskList = new List<Task>();

            foreach (var grid in GridHelper.GetNonEqualGridsByDistanceOriginAndDestination(chunkDistance, fromHere, Id))
            {
                if (Map.Chunks.TryGetValue(grid, out var chunk))
                {
                    tempTaskList.Add(chunk.BroadcastAsync(packets, rule));
                }
            }
            
            return Task.WhenAll(tempTaskList);
        }
        
        
        public Task BroadcastEventAsync<T>(T logicEvent, byte chunkDistance = 0, bool onlyPlayers = false) where T : ILogicEvent =>
            BroadcastEventAsync(logicEvent, null, chunkDistance, onlyPlayers);

        public Task BroadcastEventAsync<T>(T logicEvent, IBroadcastEventRule rule, byte chunkDistance = 0, bool onlyPlayers = false) where T : ILogicEvent
        {
            //var tempTaskList = BroadcastEventAsyncHere(logicEvent, rule, onlyPlayers);

            if (chunkDistance == 0)
            {
                return Task.WhenAll(BroadcastEventAsyncHere(logicEvent, rule, onlyPlayers));
            }

            List<Task> tempTaskList = new List<Task>();

            foreach (var grid in GridHelper.GetGridsByDistanceAndOrigin(chunkDistance, Id))
            {
                if (Map.Chunks.TryGetValue(grid, out var chunk))
                {
                    tempTaskList.Add(chunk.BroadcastEventAsync(logicEvent, rule));
                }
            }
            
            return Task.WhenAll(tempTaskList);
        }

        private List<Task> BroadcastEventAsyncHere(ILogicEvent logicEvent, IBroadcastEventRule rule, bool onlyPlayers)
        {
            if (onlyPlayers)
            {
                return PlayerEntities.Select(s =>
                    Task.Run(() =>
                        rule == null || rule.Match(s.Value)
                            ? s.Value.EmitEventAsync(logicEvent)
                            : Task.CompletedTask)).ToList();
            }
            
            return SubstantialEntities.Select(s =>
                Task.Run(() =>
                    rule == null || rule.Match(s.Value)
                        ? s.Value.EmitEventAsync(logicEvent)
                        : Task.CompletedTask)).ToList();
        }
        
        public Task BroadcastEventAsync<T>(T logicEvent, Grid fromHere, byte chunkDistance = 0, bool onlyPlayers = false) where T : ILogicEvent
            => BroadcastEventAsync(logicEvent, null, fromHere, chunkDistance);

        public Task BroadcastEventAsync<T>(T logicEvent, IBroadcastEventRule rule, Grid fromHere, byte chunkDistance = 0, bool onlyPlayers = false) where T : ILogicEvent
        {
            //var temTaskList = BroadcastEventAsyncHere(logicEvent, rule, onlyPlayers);
            
            if (chunkDistance == 0)
            {
                return Task.WhenAll(BroadcastEventAsyncHere(logicEvent, rule, onlyPlayers));
            }

            List<Task> tempTaskList = new List<Task>();
            
            foreach (var grid in GridHelper.GetNonEqualGridsByDistanceOriginAndDestination(chunkDistance, fromHere, Id))
            {
                if (Map.Chunks.TryGetValue(grid, out var chunk))
                {
                    tempTaskList.Add(chunk.BroadcastEventAsync(logicEvent, rule));
                }
            }
            
            return Task.WhenAll(tempTaskList);
        }

        /*private static void TryToAdd(ICollection<IChunk> tempChunks, IDictionary<Grid, IChunk> neighborChunks, Grid targetChunk)
        {
            if (neighborChunks.TryGetValue(targetChunk, out var neighbor))
            {
                tempChunks.Add(neighbor);
            }
        }*/
    }
}