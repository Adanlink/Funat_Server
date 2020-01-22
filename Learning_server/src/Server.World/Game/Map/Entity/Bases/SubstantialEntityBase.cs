using Server.World.Game.Map.Chunks.Interfaces;
using Server.World.Game.Map.Entity.Interfaces;
using Server.World.Game.Map.Entity.Logic.Events;
using Server.World.Game.Map.Entity.Logic.Interfaces;
using Server.World.Game.Map.Interfaces;

namespace Server.World.Game.Map.Entity.Bases
{
    public abstract class SubstantialEntityBase : EntityBase, ISubstantialEntity
    {
        public IChunk CurrentChunk { get; set; }
        
        public float X { get; set; }
        
        public float Y { get; set; }
        
        public void TransferEntity(IChunk chunk)
        {
            if (/*CurrentChunk == chunk ||*/ chunk == null)
            {
                return;
            }
            
            CurrentChunk?.UnregisterEntity(this);
            chunk.RegisterEntity(this);
            
            ILogicEvent tempEvent;

            if (CurrentChunk == null || CurrentChunk.Map != chunk.Map)
            {
                tempEvent = new EntityChangesMap
                {
                    FromHere = CurrentChunk,
                    ToHere = chunk
                };
            }
            else
            {
                tempEvent = new EntityTransferBetweenChunks
                {
                    FromHere = CurrentChunk,
                    ToHere = chunk
                };
            }
            EmitEventAsync(tempEvent);

            CurrentChunk = chunk;
        }

        protected SubstantialEntityBase(IMap currentMap) : base(currentMap)
        {
        }
    }
}