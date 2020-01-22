using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using DotNetty.Transport.Channels;
using DotNetty.Buffers;
using DotNetty.Transport.Channels.Sockets;

namespace Server.Network.Packets
{
    public interface ICustomTcpChannelHandler : IChannelHandler
    {
        /// <summary>
        /// Returns the same object with the socketChannel already defined.
        /// </summary>
        /// <param name="socketChannel"></param>
        /// <returns>The same object with the socketChannel already defined.</returns>
        ICustomTcpChannelHandler GetNewWithChannelHandler(ISocketChannel socketChannel);
    }
}
