using Autofac;
using Server.Core.IoC;
using Server.World.Configuration;
using Server.World.Game.Map.Entity;
using Server.World.Game.Map.Entity.Interfaces;
using Server.World.Game.Map.Interfaces;

namespace Server.World.Game.Map.BroadcastEventRules
{
    public class AllExceptOneNear : IBroadcastEventRule
    {
        private readonly ISubstantialEntity _sender;

        private static readonly ushort MaxVisionRange = UsefulContainer.Instance.Resolve<WorldConfiguration>().CharacterConfiguration.MaxVisionRange;

        public AllExceptOneNear(ISubstantialEntity sender)
        {
            _sender = sender;
        }

        public bool Match(ISubstantialEntity substantialEntity)
        {
            return _sender != substantialEntity && PositionHelper.DistanceBetweenEntities(_sender, substantialEntity) < MaxVisionRange;
        }
    }
}