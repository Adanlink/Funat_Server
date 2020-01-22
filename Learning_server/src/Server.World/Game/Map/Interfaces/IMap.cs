using Server.World.Game.Map.Chunks.Interfaces;
using Server.World.Game.Map.Entity.Interfaces;

namespace Server.World.Game.Map.Interfaces
{
    public interface IMap : IChunkManager
    {
        long Id { get; }
    }
}