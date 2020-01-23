using Server.World.Game.Map.Entity.Interfaces;
using Server.World.Game.Map.Entity.Logic.Interfaces;

namespace Server.World.Game.Map.Entity.Logic.Events
{
    public class PlayerEntityDeclaration : ILogicEvent
    {
        public IEntity Sender { get; set; }
        
        public IPlayerEntity RealSender { get; set; }
    }
}