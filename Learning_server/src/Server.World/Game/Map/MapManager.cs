using System.Collections.Generic;
using Server.Database.Models;
using Server.Database.Services.Interfaces;
using Server.World.Game.Map.Interfaces;

namespace Server.World.Game.Map
{
    public class MapManager : IMapManager
    {
        private readonly IMapService _mapService;
        
        private readonly Dictionary<long, IMap> _maps = new Dictionary<long, IMap>();
        private readonly Dictionary<long, IInstancedMap> _instancedMaps = new Dictionary<long, IInstancedMap>();

        public IReadOnlyDictionary<long, IMap> Maps => _maps;
        public IReadOnlyDictionary<long, IInstancedMap> InstancedMaps => _instancedMaps;

        public MapManager(IMapService mapService)
        {
            _mapService = mapService;
        }

        public IMap GetMap(long id)
        {
            //TODO implement maps database
            if (_maps.TryGetValue(id, out var map))
            {
                return map;
            }

            var tempMap = new Map( /*_mapService.GetById(id) ?? */new MapModel {Id = id});
            return _maps.TryAdd(id, tempMap) ? tempMap : null;
        }

        public IInstancedMap GetInstancedMap(long id)
        {
            throw new System.NotImplementedException();
        }

        public void Update()
        {
            foreach (var map in _maps)
            {
                map.Value.Update();
            }
            
            foreach (var instancedMap in _instancedMaps)
            {
                instancedMap.Value.Update();
            }
        }
    }
}