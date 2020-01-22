using Server.SharedThings.Packets.ClientPackets.Enums;

namespace Server.World.Game.Map.Entity.Components.Interfaces
{
    public interface IMovableComponent : IComponent
    {
        MovementDirection MovementDirection { set; }
    }
}