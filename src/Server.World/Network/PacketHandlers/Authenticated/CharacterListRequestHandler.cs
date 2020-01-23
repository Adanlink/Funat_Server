using System.Linq;
using System.Threading.Tasks;
using Server.Database.Models;
using Server.SharedThings.Packets.ClientPackets;
using Server.SharedThings.Packets.ServerPackets;
using Server.World.Network.Bases;
using Server.World.Network.Interfaces;

namespace Server.World.Network.PacketHandlers.Authenticated
{
    public class CharacterListRequestHandler : AuthenticatedPacketHandlerAsync<CharacterListRequest>
    {
        protected override Task Handle(CharacterListRequest packet, ISession session)
        {
            var list = session.Account.Characters?.Select(ToClientCharacter).ToList();

            session.SendPacket(new CharacterListResponse
            {
                Characters = list
            });
            
            return Task.CompletedTask;
        }

        private static SharedThings.Packets.Representations.Character ToClientCharacter(CharacterModel characterModel)
        {
            return new SharedThings.Packets.Representations.Character
            {
                Nickname = characterModel.Nickname,
                Authority = characterModel.Authority,
                TimeOfCreation = characterModel.TimeOfCreation,
                LastTimePlayed = characterModel.LastTimePlayed,
                MapId = characterModel.MapId
            };
        }
    }
}