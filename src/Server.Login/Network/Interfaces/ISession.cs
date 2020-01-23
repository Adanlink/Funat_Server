using System.Collections.Generic;
using System.Threading.Tasks;
using Server.SharedThings.Packets;

namespace Server.Login.Network.Interfaces
{
    public interface ISession
    {
        void SendPacket<T>(T packet) where T : IPacket;

        void SendPackets<T>(IEnumerable<T> packets) where T : IPacket;
        void SendPackets(IEnumerable<IPacket> packets);

        Task SendPacketAsync<T>(T packet) where T : IPacket;

        Task SendPacketsAsync<T>(IEnumerable<T> packets) where T : IPacket;
        Task SendPacketsAsync(IEnumerable<IPacket> packets);

        void Disconnect();
    }
}
