using Server.Database.Models;
using Server.Network;
using Server.World.Network.Interfaces;

namespace Server.World.Game.Map.Entity.Interfaces
{
    public interface IPlayerEntity : ITickAwareEntity, IMovableEntity
    {
        CharacterModel Character { get; }
        
        ISession Session { get; }

        void Dispose();
        void Save();
    }
}