using System;
using Autofac;
using Server.Core.IoC;
using Server.SharedThings.Packets.ClientPackets.Enums;
using Server.World.Configuration;
using Server.World.Game.Map.Entity.Components.Interfaces;
using Server.World.Game.Map.Entity.Interfaces;
using Server.World.Game.Map.Entity.Logic.Events;

namespace Server.World.Game.Map.Entity.Components
{
    public class BasicMovableComponent : IMovableComponent
    {
        private static readonly WorldConfiguration worldConfiguration = UsefulContainer.Instance.Resolve<WorldConfiguration>();

        public MovementDirection MovementDirection { private get; set; } = MovementDirection.Null;

        private IMovableEntity _substantialEntity;
        
        private float MovementSpeed => (float) _substantialEntity.MovementSpeed * 10 / worldConfiguration.Tps;

        private ushort Weight => _substantialEntity.Weight;

        private float Friction => _substantialEntity.Friction * 10 / worldConfiguration.Tps;

        private float _xInertia;

        private float _yInertia;

        public BasicMovableComponent(IMovableEntity substantialEntity)
        {
            _substantialEntity = substantialEntity;
        }

        public void Update()
        {
            ApplyMovement();
            
            if (!ApplyFriction())
            {
                return;
            }
            
            ApplyInertia();
            _substantialEntity.EmitEventAsync(new EntityMovementUpdate());
        }

        private void ApplyInertia()
        {
            _substantialEntity.X += _xInertia;
            _substantialEntity.Y += _yInertia;
        }

        private void ApplyMovement()
        {
            if (Math.Sqrt(
                Math.Pow(_xInertia, 2d) + Math.Pow(_yInertia, 2d))
                >= MovementSpeed)
            {
                return;
            }

            switch (MovementDirection)
            {
                case MovementDirection.Null:
                    break;
                
                #region 1AxisMovement
                case MovementDirection.Right:
                    SetMovement(MovementSpeed, true, ref _xInertia);
                    break;
                case MovementDirection.Left:
                    SetMovement(MovementSpeed, false, ref _xInertia);
                    break;
                
                case MovementDirection.Up:
                    SetMovement(MovementSpeed, true, ref _yInertia);
                    break;
                case MovementDirection.Down:
                    SetMovement(MovementSpeed, false, ref _yInertia);
                    break;
                #endregion
                
                #region 2AxisMovement
                case MovementDirection.RightUp:
                    SetMovement(MovementSpeed, true, true,
                        ref _xInertia, ref _yInertia);
                    break;
                case MovementDirection.LeftUp:
                    SetMovement(MovementSpeed, false, true,
                        ref _xInertia, ref _yInertia);
                    break;
                
                case MovementDirection.RightDown:
                    SetMovement(MovementSpeed, true, false,
                        ref _xInertia, ref _yInertia);
                    break;
                case MovementDirection.LeftDown:
                    SetMovement(MovementSpeed, false, false,
                        ref _xInertia, ref _yInertia);
                    break;
                #endregion

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private static void SetMovement(float movementSpeed, bool positive, ref float inertia)
        {
            if (positive)
                inertia += movementSpeed;
            else
                inertia -= movementSpeed;
        }
        
        private static void SetMovement(float movementSpeed, bool xPositive, bool yPositive, ref float xInertia, ref float yInertia)
        {
            var tempValue = (float) (movementSpeed * Math.Cos(45));

            SetMovement(tempValue, xPositive, ref xInertia);
            SetMovement(tempValue, yPositive, ref yInertia);
        }
        
        /// <summary>
        /// Apply friction and returns true when inertia will affect the position
        /// </summary>
        /// <returns></returns>
        private bool ApplyFriction()
        {
            var frictionForceValue = Weight * Friction;
            var asd = SetFriction(frictionForceValue, ref _xInertia);
            var asd2 = SetFriction(frictionForceValue, ref _yInertia);
            return asd || asd2;
        }
        
        /// <summary>
        /// Apply friction and returns true when inertia doesn't equal to 0
        /// </summary>
        /// <param name="frictionForceValue"></param>
        /// <param name="inertia"></param>
        /// <returns></returns>
        private static bool SetFriction(float frictionForceValue, ref float inertia)
        {
            if (Math.Abs(inertia) < 0.001f)
            {
                return false;
            }
            if (inertia > 0)
            {
                inertia -= frictionForceValue;
                if (inertia < 0)
                {
                    inertia = 0;
                    return false;
                }
            }
            else if (inertia < 0)
            {
                inertia += frictionForceValue;
                if (inertia > 0)
                {
                    inertia = 0;
                    return false;
                }
            }

            return true;
        }

        public void Dispose()
        {
            _substantialEntity = null;
        }
    }
}