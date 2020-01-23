using Server.World.Game.Map.Entity.Interfaces;
using Server.World.Game.Map.Interfaces;

namespace Server.World.Game.Map.BroadcastEventRules
{
    public class AllExceptOne : IBroadcastEventRule
    {
        private readonly ISubstantialEntity _sender;

        public AllExceptOne(ISubstantialEntity sender)
        {
            _sender = sender;
        }

        public bool Match(ISubstantialEntity substantialEntity)
        {
            return _sender != substantialEntity;
        }
    }
}