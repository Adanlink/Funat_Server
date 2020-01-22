using System;
using System.Collections.Generic;

namespace Server.World.Game.Map.Entity.Interfaces
{
    public interface IEntityManager : IDisposable
    {
        Dictionary<Guid, IPlayerEntity> PlayerEntities { get; }
        
        Dictionary<Guid, IEntity> Entities { get; }
        
        void Update();
        
        /// <summary>
        ///     Register an entity to the entity container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void RegisterEntity<T>(T entity) where T : IEntity;

        /// <summary>
        ///     Unregister the entity from the entity manager
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void UnregisterEntity<T>(T entity) where T : IEntity;
    }
}