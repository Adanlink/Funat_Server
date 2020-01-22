using Server.SharedThings.Packets.Representations;
using Server.World.Game.Map.Entity.Interfaces;

namespace Server.World.Game.Map.Entity
{
    public static class RepresentationHelper
    {
        public static Player ToPlayerRepresentation(this IPlayerEntity playerEntity)
        {
            return new Player
            {
                Id = playerEntity.Id.ToString(),
                Nickname = playerEntity.Character.Nickname,
                X = playerEntity.X,
                Y = playerEntity.Y
            };
        }
        
        public static BasicEntity ToSubstantialEntityRepresentation(this ISubstantialEntity substantialEntity)
        {
            return new BasicEntity
            {
                Id = substantialEntity.Id.ToString(),
                X = substantialEntity.X,
                Y = substantialEntity.Y
            };
        }
    }
}