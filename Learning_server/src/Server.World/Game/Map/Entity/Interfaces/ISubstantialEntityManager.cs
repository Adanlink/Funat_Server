using System;
using System.Collections.Generic;

namespace Server.World.Game.Map.Entity.Interfaces
{
    public interface ISubstantialEntityManager : IDisposable
    {
        Dictionary<Guid, IPlayerEntity> PlayerEntities { get; }
        
        Dictionary<Guid, ISubstantialEntity> SubstantialEntities { get; }
        
        void Update();
        
        /// <summary>
        ///     Register an entity to the entity container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void RegisterEntity<T>(T entity) where T : ISubstantialEntity;

        /// <summary>
        ///     Unregister the entity from the entity manager
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void UnregisterEntity<T>(T entity) where T : ISubstantialEntity;
    }
}