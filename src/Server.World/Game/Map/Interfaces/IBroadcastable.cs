using System.Collections.Generic;
using System.Threading.Tasks;
using Server.SharedThings.Packets;
using Server.World.Game.Map.Entity.Interfaces;

namespace Server.World.Game.Map.Interfaces
{
    public interface IBroadcastable
    {
        Task BroadcastAsync<T>(T packet) where T : IPacket;

        Task BroadcastAsync<T>(T packet, IBroadcastRule rule) where T : IPacket;

        Task BroadcastAsync<T>(IEnumerable<T> packets) where T : IPacket;

        Task BroadcastAsync<T>(IEnumerable<T> packets, IBroadcastRule rule) where T : IPacket;
        
        Task BroadcastAsync(IEnumerable<IPacket> packets);

        Task BroadcastAsync(IEnumerable<IPacket> packets, IBroadcastRule rule);
    }
}