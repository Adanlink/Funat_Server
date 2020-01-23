using Server.World.Game.Map.Entity.Interfaces;

namespace Server.World.Game.Map.Entity.Logic.Interfaces
{
    public interface ILogicEvent
    {
        IEntity Sender { get; set; }
    }
}