using Server.World.Game.Map.Entity.Interfaces;

namespace Server.World.Game.Map.Interfaces
{
    public interface IBroadcastEventRule
    {
        bool Match(ISubstantialEntity substantialEntity);
    }
}