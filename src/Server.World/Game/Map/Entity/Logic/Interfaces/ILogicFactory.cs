using System;
using System.Threading;
using System.Threading.Tasks;

namespace Server.World.Game.Map.Entity.Logic.Interfaces
{
    public interface ILogicFactory
    {
        Task RegisterAsync(ILogicHandler handler, Type eventType);

        Task UnregisterAsync(ILogicHandler handler, Type eventType);

        Task Handle<TLogicEvent>(TLogicEvent logicEvent, CancellationToken cancellationToken = default) where TLogicEvent : ILogicEvent;
    }
}