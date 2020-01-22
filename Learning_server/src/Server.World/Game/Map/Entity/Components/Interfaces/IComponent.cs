using System;

namespace Server.World.Game.Map.Entity.Components.Interfaces
{
    public interface IComponent : IDisposable
    {
        void Update();
    }
}