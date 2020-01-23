using Server.World.Game.Map.Chunks.Interfaces;

namespace Server.World.Game.Map.Entity.Interfaces
{
    public interface ISubstantialEntity : IEntity
    {
        IChunk CurrentChunk { get; set; }
        
        float X { get; set; }
        
        float Y { get; set; }

        /// <summary>
        ///     Will transfer the Entity to another entity manager
        /// </summary>
        /// <param name="chunk"></param>
        void TransferEntity(IChunk chunk);
    }
}