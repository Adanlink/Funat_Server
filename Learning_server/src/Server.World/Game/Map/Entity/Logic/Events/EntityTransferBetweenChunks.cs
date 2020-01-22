using Server.World.Game.Map.Chunks.Interfaces;
using Server.World.Game.Map.Chunks.Struct;
using Server.World.Game.Map.Entity.Interfaces;
using Server.World.Game.Map.Entity.Logic.Interfaces;

namespace Server.World.Game.Map.Entity.Logic.Events
{
    public class EntityTransferBetweenChunks : ILogicEvent
    {
        public IEntity Sender { get; set; }
        
        public IChunk FromHere { get; set; }
        
        public IChunk ToHere { get; set; }
    }
}