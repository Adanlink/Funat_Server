using System.Collections.Generic;
using Server.World.Game.Map.Chunks.Struct;
using Server.World.Game.Map.Entity.Interfaces;
using Server.World.Game.Map.Interfaces;

namespace Server.World.Game.Map.Chunks.Interfaces
{
    public interface IChunk : ISubstantialEntityManager, IBroadcastableChunk
    {
        Grid Id { get; }
        
        IMap Map { get; }

        //Dictionary<Grid, IChunk> NeighborChunks { get; }

        //void RegisterChunk(IChunk chunk);

        //void UnregisterChunk(IChunk chunk);
    }
}