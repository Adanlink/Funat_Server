using Server.World.Game.Map.Entity.Interfaces;

namespace Server.World.Game.Map.Interfaces
{
    public interface IBroadcastRule
    {
        bool Match(IPlayerEntity player);
    }
}