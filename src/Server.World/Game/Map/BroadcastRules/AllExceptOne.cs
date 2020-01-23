using Autofac;
using Server.Core.IoC;
using Server.World.Configuration;
using Server.World.Game.Map.Entity;
using Server.World.Game.Map.Entity.Interfaces;
using Server.World.Game.Map.Interfaces;

namespace Server.World.Game.Map.BroadcastRules
{
    public class AllExceptOne : IBroadcastRule
    {
        private readonly IPlayerEntity _sender;

        public AllExceptOne(IPlayerEntity sender)
        {
            _sender = sender;
        }

        public bool Match(IPlayerEntity player)
        {
            return _sender != player;
        }
    }
}