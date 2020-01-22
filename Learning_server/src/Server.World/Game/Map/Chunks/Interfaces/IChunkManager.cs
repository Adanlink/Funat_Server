using System.Collections.Generic;
using Server.World.Game.Map.Chunks.Struct;
using Server.World.Game.Map.Entity.Interfaces;

namespace Server.World.Game.Map.Chunks.Interfaces
{
    public interface IChunkManager : IEntityManager
    {
        Dictionary<Grid, IChunk> Chunks { get; }

        IChunk GetChunk(Grid grid);
    }
}