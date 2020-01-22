using System;
using System.Threading.Tasks;
using Autofac;
using Server.Core.IoC;
using Server.World.Game.Map.Chunks.Interfaces;
using Server.World.Game.Map.Entity.Interfaces;
using Server.World.Game.Map.Interfaces;
using Server.World.Game.Map.Entity.Logic.Interfaces;

namespace Server.World.Game.Map.Entity.Bases
{
    public abstract class EntityBase : IEntity
    {
        private static readonly ILogicFactory LogicFactory = new Lazy<ILogicFactory>(() => UsefulContainer.Instance.Resolve<ILogicFactory>()).Value;
        
        public Guid Id { get; set; }

        public IMap CurrentMap { get; protected set; }

        protected EntityBase(IMap currentMap)
        {
            Id = Guid.NewGuid();
            CurrentMap = currentMap;
        }
        
        public void EmitEvent<T>(T logicEvent) where T : ILogicEvent
        {
            logicEvent.Sender = this;
            LogicFactory.Handle(logicEvent).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public Task EmitEventAsync<T>(T logicEvent) where T : ILogicEvent
        {
            logicEvent.Sender = this;
            return LogicFactory.Handle(logicEvent);
        }

        public void TransferEntity(IMap map)
        {
            /*if (CurrentMap == map)
            {
                return;
            }*/

            CurrentMap.UnregisterEntity(this);
            map.RegisterEntity(this);
            
            CurrentMap = map;
        }

        public abstract void Dispose();
    }
}