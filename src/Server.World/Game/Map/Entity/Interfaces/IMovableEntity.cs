using Server.World.Game.Map.Entity.Components.Interfaces;

namespace Server.World.Game.Map.Entity.Interfaces
{
    public interface IMovableEntity : ISubstantialEntity
    {
        ushort MovementSpeed { get; }
        
        ushort Weight { get; }

        float Friction { get; }
        
        IMovableComponent MovableComponent { get; }
    }
}