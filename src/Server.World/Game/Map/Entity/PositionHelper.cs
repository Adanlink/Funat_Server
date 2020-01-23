using System;
using DotNetty.Common.Internal;
using Server.World.Game.Map.Entity.Interfaces;

namespace Server.World.Game.Map.Entity
{
    public static class PositionHelper
    {
        public static float DistanceBetweenEntities(ISubstantialEntity substantialEntity1, ISubstantialEntity substantialEntity2)
        {
            var xVector = substantialEntity1.X - substantialEntity2.X;
            var yVector = substantialEntity1.Y - substantialEntity2.Y;
            return MathF.Sqrt(MathF.Pow(xVector, 2) + MathF.Pow(yVector, 2));
        }
    }
}