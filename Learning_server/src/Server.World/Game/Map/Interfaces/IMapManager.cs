using System.Collections.Generic;

namespace Server.World.Game.Map.Interfaces
{
    public interface IMapManager
    {
        IReadOnlyDictionary<long, IMap> Maps { get; }

        IReadOnlyDictionary<long, IInstancedMap> InstancedMaps { get; }

        IMap GetMap (long id);
        
        IInstancedMap GetInstancedMap (long id);

        void Update();
    }
}