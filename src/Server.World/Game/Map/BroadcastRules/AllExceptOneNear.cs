using Autofac;
using Server.Core.IoC;
using Server.World.Configuration;
using Server.World.Game.Map.Entity;
using Server.World.Game.Map.Entity.Interfaces;
using Server.World.Game.Map.Interfaces;

namespace Server.World.Game.Map.BroadcastRules
{
    public class AllExceptOneNear : IBroadcastRule
    {
        private readonly IPlayerEntity _sender;

        private static readonly ushort MaxVisionRange = UsefulContainer.Instance.Resolve<WorldConfiguration>().CharacterConfiguration.MaxVisionRange;

        public AllExceptOneNear(IPlayerEntity sender)
        {
            _sender = sender;
        }

        public bool Match(IPlayerEntity player)
        {
            return _sender != player && PositionHelper.DistanceBetweenEntities(_sender, player) < MaxVisionRange;
        }
    }
}