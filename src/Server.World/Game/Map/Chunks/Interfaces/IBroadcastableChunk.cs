using System.Collections.Generic;
using System.Threading.Tasks;
using Server.SharedThings.Packets;
using Server.World.Game.Map.Chunks.Struct;
using Server.World.Game.Map.Entity.Logic.Interfaces;
using Server.World.Game.Map.Interfaces;

namespace Server.World.Game.Map.Chunks.Interfaces
{
    public interface IBroadcastableChunk
    {
        Task BroadcastAsync<T>(T packet, byte chunkDistance = 0) where T : IPacket;

        Task BroadcastAsync<T>(T logicEvent, IBroadcastRule rule, byte chunkDistance = 0) where T : IPacket;

        Task BroadcastAsync<T>(IEnumerable<T> packets, byte chunkDistance = 0) where T : IPacket;

        Task BroadcastAsync<T>(IEnumerable<T> packets, IBroadcastRule rule, byte chunkDistance = 0) where T : IPacket;
        
        Task BroadcastAsync(IEnumerable<IPacket> packets, byte chunkDistance = 0);

        Task BroadcastAsync(IEnumerable<IPacket> packets, IBroadcastRule rule, byte chunkDistance = 0);
        
        
        Task BroadcastAsync<T>(T packet, Grid fromHere, byte chunkDistance = 0) where T : IPacket;

        Task BroadcastAsync<T>(T packet, IBroadcastRule rule, Grid fromHere, byte chunkDistance = 0) where T : IPacket;

        Task BroadcastAsync<T>(IEnumerable<T> packets, Grid fromHere, byte chunkDistance = 0) where T : IPacket;

        Task BroadcastAsync<T>(IEnumerable<T> packets, IBroadcastRule rule, Grid fromHere, byte chunkDistance = 0) where T : IPacket;
        
        Task BroadcastAsync(IEnumerable<IPacket> packets, Grid fromHere, byte chunkDistance = 0);

        Task BroadcastAsync(IEnumerable<IPacket> packets, IBroadcastRule rule, Grid fromHere, byte chunkDistance = 0);
        
        
        Task BroadcastEventAsync<T>(T logicEvent, byte chunkDistance = 0, bool onlyPlayers = false) where T : ILogicEvent;

        Task BroadcastEventAsync<T>(T packet, IBroadcastEventRule rule, byte chunkDistance = 0, bool onlyPlayers = false) where T : ILogicEvent;
        
        Task BroadcastEventAsync<T>(T logicEvent, Grid fromHere, byte chunkDistance = 0, bool onlyPlayers = false) where T : ILogicEvent;

        Task BroadcastEventAsync<T>(T logicEvent, IBroadcastEventRule rule, Grid fromHere, byte chunkDistance = 0, bool onlyPlayers = false) where T : ILogicEvent;
    }
}