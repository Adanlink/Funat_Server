using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Server.Database.Models;
using Server.SharedThings.Packets;
using Server.World.Game.Map.Entity.Interfaces;

namespace Server.World.Network.Interfaces
{
    public interface ISession
    {
        IPEndPoint EndPoint { get; }
        
        AccountModel Account { get; }

        bool IsAuthenticated { get; }
        
        bool IsPlaying { get; }
        
        IPlayerEntity Player { get; }
        
        void LoadAccount(AccountModel accountModel);

        void LoadCharacter(CharacterModel characterModel);
        
        void SendPacket<T>(T packet) where T : IPacket;

        void SendPackets<T>(IEnumerable<T> packets) where T : IPacket;
        void SendPackets(IEnumerable<IPacket> packets);

        Task SendPacketAsync<T>(T packet) where T : IPacket;

        Task SendPacketsAsync<T>(IEnumerable<T> packets) where T : IPacket;
        Task SendPacketsAsync(IEnumerable<IPacket> packets);

        void Disconnect();
    }
}
