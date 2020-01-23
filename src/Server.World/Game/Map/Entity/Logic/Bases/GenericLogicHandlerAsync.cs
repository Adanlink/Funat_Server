using System.Threading;
using System.Threading.Tasks;
using Server.World.Game.Map.Entity.Logic.Interfaces;

namespace Server.World.Game.Map.Entity.Logic.Bases
{
    public abstract class GenericLogicHandlerAsync<TLogicEvent> : ILogicHandler
        where TLogicEvent : ILogicEvent
    {
        public Task Handle(ILogicEvent logicEvent, CancellationToken cancellation)
        {
            return logicEvent is TLogicEvent e ? Handle(e, cancellation) : Task.CompletedTask;
        }

        protected abstract Task Handle(TLogicEvent logicEvent, CancellationToken cancellation);
    }
}