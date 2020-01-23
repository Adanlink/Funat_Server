using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Database.Models;
using Server.Database.Services.Interfaces;
using Server.SharedThings.Packets.ClientPackets;
using Server.SharedThings.Packets.ServerPackets;
using Server.SharedThings.Packets.ServerPackets.Enums;
using Server.World.Configuration;
using Server.World.Network.Bases;
using Server.World.Network.Interfaces;

namespace Server.World.Network.PacketHandlers.Authenticated
{
    public class CharacterCreateRequestHandler : AuthenticatedPacketHandlerAsync<CharacterCreateRequest>
    {
        private readonly ICharacterService _characterService;
        private readonly IAccountService _accountService;
        private readonly CharacterConfiguration _characterConfiguration;

        public CharacterCreateRequestHandler(ICharacterService characterService, IAccountService accountService, WorldConfiguration worldConfiguration)
        {
            _characterService = characterService;
            _accountService = accountService;
            _characterConfiguration = worldConfiguration.CharacterConfiguration;
        }

        protected override Task Handle(CharacterCreateRequest packet, ISession session)
        {
            if (session.Account.Characters == null)
            {
                session.Account.Characters = new List<CharacterModel>();
            }
            
            if (session.Account.Characters.Count >= _characterConfiguration.MaxAccountCharacters)
            {
                session.SendPacket(new CharacterCreateResponse
                {
                    CharacterCreateResponseType = CharacterCreateResponseType.MaximumCapacityReached
                });
                return Task.CompletedTask;
            }
            
            if (_characterService.GetByNickname(packet.Nickname) != default)
            {
                session.SendPacket(new CharacterCreateResponse
                {
                    CharacterCreateResponseType = CharacterCreateResponseType.NicknameTaken
                });
                return Task.CompletedTask;
            }

            session.Account.Characters.Add(new CharacterModel
            {
                OwnerAccount = session.Account,
                TimeOfCreation = DateTime.UtcNow,
                LastTimePlayed = DateTime.UtcNow,
                Nickname = packet.Nickname
            });
            
            _accountService.Save(session.Account);
            
            //TODO to check if you can do something about this
            /*_characterService.Save(new CharacterModel
            {
                OwnerAccount = session.Account,
                Nickname = packet.Nickname
            });*/
            
            session.SendPacket(new CharacterCreateResponse
            {
                CharacterCreateResponseType = CharacterCreateResponseType.CharacterCreated
            });

            return Task.CompletedTask;
        }
    }
}