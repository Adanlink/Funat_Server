using System.Threading.Tasks;
using Autofac;
using System;
using System.Linq;
using Server.Core.IoC;
using Server.SharedThings.Packets.ClientPackets;
using Server.SharedThings.Packets.Representations;
using Server.SharedThings.Packets.ServerPackets;
using Server.SharedThings.Packets.ServerPackets.Enums;
using Server.SharedThings.Packets.ServerPackets.Game;
using Server.World.Network.Bases;
using Server.World.Network.Interfaces;

namespace Server.World.Network.PacketHandlers.Authenticated
{
    public class CharacterSelectRequestHandler : AuthenticatedPacketHandlerAsync<CharacterSelectRequest>
    {
        private static readonly ISessionManager SessionManager = new Lazy<ISessionManager>(() => UsefulContainer.Instance.Resolve<ISessionManager>()).Value;
        
        protected override Task Handle(CharacterSelectRequest packet, ISession session)
        {
            var foundCharacter = session.Account.Characters.ToList().Find(c => c.Nickname == packet.Nickname);
            if (foundCharacter == default)
            {
                session.SendPacketAsync(new CharacterSelectResponse
                {
                    CharacterSelectResponseType = CharacterSelectResponseType.CouldNotFindCharacter
                });
                return Task.CompletedTask;
            }

            if (!SessionManager.RegisterCharacterAsync(foundCharacter).Result)
            {
                session.SendPacketAsync(new CharacterSelectResponse
                {
                    CharacterSelectResponseType = CharacterSelectResponseType.CharacterAlreadyPlaying
                });
                return Task.CompletedTask;
            }
            
            session.SendPacketAsync(new CharacterSelectResponse
            {
                CharacterSelectResponseType = CharacterSelectResponseType.CharacterSelected
            });

            session.SendPacketAsync(new PlayerStart
            {
                OwnPlayer = new OwnPlayer
                {
                    Nickname = foundCharacter.Nickname,
                    X = foundCharacter.MapX,
                    Y = foundCharacter.MapY
                }
            });
            session.LoadCharacter(foundCharacter);
            return Task.CompletedTask;
        }
    }
}