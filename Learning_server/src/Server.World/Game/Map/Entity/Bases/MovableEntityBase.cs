using Server.World.Game.Map.Entity.Components.Interfaces;
using Server.World.Game.Map.Entity.Interfaces;
using Server.World.Game.Map.Interfaces;

namespace Server.World.Game.Map.Entity.Bases
{
    public abstract class MovableEntityBase : SubstantialEntityBase, IMovableEntity
    {
        private ushort _movementSpeed = 1;

        public virtual ushort MovementSpeed
        {
            get => _movementSpeed;
            private set => _movementSpeed = (ushort) (value == 0 ? 1 : value);
        }

        private ushort _weight = 1;

        public ushort Weight
        {
            get => _weight;
            private set => _weight = (ushort) (value == 0 ? 1 : value);
        }

        private float _friction = 0.5f;

        public float Friction
        {
            get => _friction;
            private set
            {
                if (value < 0.05f)
                {
                    value = 0.05f;
                }
                else if (value > 1f)
                {
                    value = 1f;
                }

                _friction = value;
            }
        }

        public abstract IMovableComponent MovableComponent { get; }

        protected MovableEntityBase(IMap currentMap) : base(currentMap)
        {
        }
    }
}