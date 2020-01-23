using System;
using Autofac;
using Server.Core.IoC;
using Server.Database.Models;
using Server.Database.Services.Interfaces;
using Server.Network;
using Server.World.Game.Map.Entity.Bases;
using Server.World.Game.Map.Entity.Components;
using Server.World.Game.Map.Entity.Components.Interfaces;
using Server.World.Game.Map.Entity.Interfaces;
using Server.World.Game.Map.Entity.Logic.Events;
using Server.World.Game.Map.Entity.Logic.Events.Handlers;
using Server.World.Game.Map.Interfaces;
using Server.World.Network.Interfaces;

namespace Server.World.Game.Map.Entity
{
    public class PlayerEntity : MovableEntityBase, IPlayerEntity
    {
        private static readonly ICharacterService CharacterService =
            new Lazy<ICharacterService>(() => UsefulContainer.Instance.Resolve<ICharacterService>()).Value;
        
        private static readonly ISessionManager SessionManager =
            new Lazy<ISessionManager>(() => UsefulContainer.Instance.Resolve<ISessionManager>()).Value;

        public CharacterModel Character { get; }
        
        public ISession Session { get; set; }
        
        public override IMovableComponent MovableComponent { get; }
        
        public PlayerEntity(IMap currentMap, CharacterModel character, ISession session) : base(currentMap)
        {
            Session = session;
            Character = character;
            X = character.MapX;
            Y = character.MapY;
            MovableComponent = new BasicMovableComponent(this);
            
            TransferEntity(currentMap);
        }
        
        public void Update()
        {
            MovableComponent.Update();
        }
        
        public void Save()
        {
            Character.MapId = CurrentMap.Id;
            Character.MapX = X;
            Character.MapY = Y;
            CharacterService.Save(Character);
        }

        public override void Dispose()
        {
            Save();
            EmitEvent(new EntityChangesMap
            {
                FromHere = CurrentChunk
            });
            MovableComponent.Dispose();
            Session = null;
            CurrentMap.UnregisterEntity(this);
            CurrentChunk.UnregisterEntity(this);
            SessionManager.UnregisterCharacterAsync(Character);
        }
    }
}