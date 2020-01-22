using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Server.World.Game.Map.Entity.Logic.Interfaces;

namespace Server.World.Game.Map.Entity.Logic
{
    public class LogicFactory : ILogicFactory
    {
        private readonly IDictionary<Type, ILogicHandler> _logicHandlers = new Dictionary<Type, ILogicHandler>();
        
        public Task RegisterAsync(ILogicHandler handler, Type eventType)
        {
            if (_logicHandlers.ContainsKey(eventType))
            {
                return Task.CompletedTask;
            }
            _logicHandlers[eventType] = handler;
            return Task.CompletedTask;
        }

        public Task UnregisterAsync(ILogicHandler handler, Type eventType)
        {
            _logicHandlers.Remove(eventType);
            return Task.CompletedTask;
        }

        public Task Handle<TLogicEvent>(TLogicEvent logicEvent, CancellationToken cancellationToken = default) where TLogicEvent : ILogicEvent
        {
            if (!_logicHandlers.TryGetValue(logicEvent.GetType(), out var handler))
            {
                return Task.CompletedTask;
            }
            
            handler.Handle(logicEvent, cancellationToken);
            return Task.CompletedTask;
        }
    }
}