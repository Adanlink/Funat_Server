using System;
using System.Threading.Tasks;
using Server.World.Game.Map.Chunks.Interfaces;
using Server.World.Game.Map.Entity.Logic.Interfaces;
using Server.World.Game.Map.Interfaces;

namespace Server.World.Game.Map.Entity.Interfaces
{
    public interface IEntity// : IDisposable
    {
        Guid Id { get; set; }

        IMap CurrentMap { get; }

        /// <summary>
        ///     Notify a system of the entity manager to be executed.
        /// </summary>
        /// <typeparam name="T">System type</typeparam>
        /// <param name="e">Arguments</param>
        void EmitEvent<T>(T e) where T : ILogicEvent;

        Task EmitEventAsync<T>(T e) where T : ILogicEvent;

        /// <summary>
        ///     Will transfer the Entity to another entity manager
        /// </summary>
        /// <param name="map"></param>
        void TransferEntity(IMap map);
    }
}