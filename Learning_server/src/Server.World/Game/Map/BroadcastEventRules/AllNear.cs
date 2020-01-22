﻿using Autofac;
using Server.Core.IoC;
using Server.World.Configuration;
using Server.World.Game.Map.Entity;
using Server.World.Game.Map.Entity.Interfaces;
using Server.World.Game.Map.Interfaces;

namespace Server.World.Game.Map.BroadcastEventRules
{
    public class AllNear : IBroadcastEventRule
    {
        private readonly ISubstantialEntity _sender;

        private static readonly ushort MaxVisionRange = UsefulContainer.Instance.Resolve<WorldConfiguration>().CharacterConfiguration.MaxVisionRange;

        public AllNear(ISubstantialEntity sender)
        {
            _sender = sender;
        }

        public bool Match(ISubstantialEntity player)
        {
            return PositionHelper.DistanceBetweenEntities(_sender, player) < MaxVisionRange;
        }
    }
}