using Server.SharedThings.Packets.ClientPackets.Game;
using Server.SharedThings.Packets.ServerPackets.Game;
using Server.World.Game.Map.BroadcastRules;
using Server.World.Game.Map.Chunks;
using Server.World.Network.Bases;
using Server.World.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Server.World.Network.PacketHandlers.Character
{
    public class ClientSendChatMessageHandler : CharacterPacketHandlerAsync<ClientSendChatMessage>
    {
        protected override Task Handle(ClientSendChatMessage packet, ISession session)
        {
            session.Player.CurrentChunk.BroadcastAsync(new ServerSendChatMessage
            {
                Message = $"<{session.Player.Character.Nickname}> {packet.Message}"
            }, new AllExceptOne(session.Player), GridHelper.MaxChunkDistanceView);

            return Task.CompletedTask;
        }
    }
}