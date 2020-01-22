using System;
using System.Threading;
using System.Threading.Tasks;
using Server.Crypto;
using Server.Crypto.Hashers;
using Server.Database.Services.Interfaces;
using Server.Network;
using Server.Network.Packets;
using Server.SharedThings.Packets.ClientPackets;
using Server.SharedThings.Packets.ServerPackets;
using Server.SharedThings.Packets.ServerPackets.Enums;
using ISession = Server.Login.Network.Interfaces.ISession;

namespace Server.Login.Network.PacketHandlers
{
    public class LoginRequestHandler : GenericPacketHandlerAsync<LoginRequest>
    {
        private readonly IAccountService _accountService;
        private Argon2Hasher _argon2;
        private readonly HasherConfiguration _hasherConfiguration;
        private readonly CryptoRandom _rng = new CryptoRandom();

        public LoginRequestHandler(IAccountService accountService, Argon2Hasher argon2, LoginConfiguration loginConfiguration)
        {
            _accountService = accountService;
            _argon2 = argon2;
            _hasherConfiguration = loginConfiguration.HasherConfiguration;
        }

        protected override Task Handle(LoginRequest packet, ISession session)
        {
            var userAccount = _accountService.GetByUsername(packet.Username);
            if (userAccount == null)
            {
                Thread.Sleep(
                    _rng.Next(
                        _hasherConfiguration.HashingMinimumTimeWait, _hasherConfiguration.HashingMaximumTimeWait));
                return Failed(session);
            }

            var serverArgon2 = new Argon2Hasher(userAccount.EncodedHash);

            if (!serverArgon2.CheckString(packet.PasswordHash))
            {
                return Failed(session);
            }

            var randomGuid = Guid.NewGuid();

            userAccount.SessionId = randomGuid;
            userAccount.SessionIdExpiryDate = DateTime.UtcNow.AddMinutes(5d);
            _accountService.Save(userAccount);

            session.SendPacket(
                new LoginSucceeded
                {
                    SessionId = randomGuid.ToString()
                });

            return Task.CompletedTask;
        }

        private static Task Failed(ISession session)
        {
            session.SendPacket(new LoginFailed { LoginFailedType = LoginFailedType.ErroneousCredentials });
            return Task.CompletedTask;
        }
    }
}
