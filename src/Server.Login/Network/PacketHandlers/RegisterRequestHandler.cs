using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Crypto;
using Server.Crypto.Hashers;
using Server.Database.Models;
using Server.Database.Services.Interfaces;
using Server.Network;
using Server.Network.Packets;
using Server.SharedThings.Packets.ClientPackets;
using Server.SharedThings.Packets.ServerPackets;
using Server.SharedThings.Packets.ServerPackets.Enums;
using ISession = Server.Login.Network.Interfaces.ISession;

namespace Server.Login.Network.PacketHandlers
{
    public class RegisterRequestHandler : GenericPacketHandlerAsync<RegisterRequest>
    {
        private readonly IAccountService _accountService;
        private readonly Argon2Hasher _argon2;
        private readonly CryptoRandom _rng = new CryptoRandom();

        public RegisterRequestHandler(IAccountService accountService, Argon2Hasher argon2)
        {
            _accountService = accountService;
            _argon2 = argon2;
        }

        protected override Task Handle(RegisterRequest packet, ISession session)
        {
            if (packet.Key != "alohamora321")
            {
                return Return(session);
            }

            var account = _accountService.GetByUsername(packet.Username);

            if (account != null)
            {
                return Return(session);
            }

            var salt = new byte[16];
            _rng.NextBytes(salt);
            _argon2.Salt = salt;
            _argon2.Hash = _argon2.HashWithSpec(packet.PasswordHash);

            _accountService.Save(new AccountModel
            {
                Username = packet.Username,
                EncodedHash = _argon2.GetEncodedHash(),
                TimeOfCreation = DateTime.UtcNow
            });

            session.SendPacket(new RegisterResponse { RegisterResponseType = RegisterResponseType.Registered});

            return Task.CompletedTask;
        }

        private static Task Return(ISession session)
        {
            session.SendPacket(new RegisterResponse { RegisterResponseType = RegisterResponseType.BadKeyOrTakenUsername });
            return Task.CompletedTask;
        }
    }
}
