using Server.World.Game.Map.Entity.Interfaces;
using Server.World.Game.Map.Entity.Logic.Interfaces;

namespace Server.World.Game.Map.Entity.Logic.Events
{
    public class EntityMovementUpdate : ILogicEvent
    {
        public IEntity Sender { get; set; }
    }
}