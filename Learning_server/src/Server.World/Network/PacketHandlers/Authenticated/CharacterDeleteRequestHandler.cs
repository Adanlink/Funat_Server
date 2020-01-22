using System.Linq;
using System.Threading.Tasks;
using Server.Database.Services.Interfaces;
using Server.SharedThings.Packets.ClientPackets;
using Server.SharedThings.Packets.ServerPackets;
using Server.SharedThings.Packets.ServerPackets.Enums;
using Server.World.Network.Bases;
using Server.World.Network.Interfaces;

namespace Server.World.Network.PacketHandlers.Authenticated
{
    public class CharacterDeleteRequestHandler : AuthenticatedPacketHandlerAsync<CharacterDeleteRequest>
    {
        private readonly ICharacterService _characterService;

        public CharacterDeleteRequestHandler(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        protected override Task Handle(CharacterDeleteRequest packet, ISession session)
        {
            var foundCharacter = session.Account.Characters.ToList().Find(c => c.Nickname == packet.Nickname);
            if (foundCharacter == default)
            {
                session.SendPacket(new CharacterDeleteResponse
                {
                    CharacterDeleteResponseType = CharacterDeleteResponseType.CouldNotFindCharacter
                });
                return Task.CompletedTask;
            }
            
            _characterService.DeleteByModel(foundCharacter);
            session.SendPacket(new CharacterDeleteResponse
            {
                CharacterDeleteResponseType = CharacterDeleteResponseType.CharacterDeleted
            });
            return Task.CompletedTask;
        }
    }
}