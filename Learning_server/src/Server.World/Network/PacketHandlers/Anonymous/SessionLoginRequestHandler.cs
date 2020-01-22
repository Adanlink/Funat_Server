using System;
using System.Threading.Tasks;
using Autofac;
using Server.Core.IoC;
using Server.Database.Services.Interfaces;
using Server.SharedThings.Packets.ClientPackets;
using Server.SharedThings.Packets.ServerPackets;
using Server.SharedThings.Packets.ServerPackets.Enums;
using Server.World.Configuration;
using Server.World.Network.Bases;
using Server.World.Network.Interfaces;

namespace Server.World.Network.PacketHandlers.Anonymous
{
    public class SessionLoginRequestHandler : AnonymousPacketHandlerAsync<SessionLoginRequest>
    {
        private static readonly IAccountService AccountService = new Lazy<IAccountService>(() => UsefulContainer.Instance.Resolve<IAccountService>()).Value;
        
        protected override Task Handle(SessionLoginRequest packet, ISession session)
        {
            if (session.IsAuthenticated)
            {
                //TODO apply only return
                session.SendPacket(
                    new SessionLoginResponse
                    {
                        SessionLoginResponseType = SessionLoginResponseType.AlreadyLoggedIn
                    });
                return Task.CompletedTask;
            }
            
            var userAccount = AccountService.GetByUsernameFresh(packet.Username);
            if (userAccount == default
                || packet.SessionId != userAccount.SessionId.ToString()
                || DateTime.Compare(userAccount.SessionIdExpiryDate, DateTime.UtcNow) < 0)
            {
                session.SendPacket(
                new SessionLoginResponse
                {
                    SessionLoginResponseType = SessionLoginResponseType.FailedLogin
                });
                session.Disconnect();

                return Task.CompletedTask;
            }
            
            userAccount.SessionIdExpiryDate = DateTime.MinValue;
            AccountService.Save(userAccount);
            session.LoadAccount(userAccount);
            
            session.SendPacket(
                new SessionLoginResponse
                {
                    SessionLoginResponseType = SessionLoginResponseType.SuccessfulLogin
                });

            return Task.CompletedTask;
        }
    }
}
