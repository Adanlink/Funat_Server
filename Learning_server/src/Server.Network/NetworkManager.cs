using System;
using System.Collections.Generic;
using System.Text;
using DotNetty.Codecs;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System.Threading.Tasks;
using Server.Core.Logging;
using Server.Network.Packets;
using Server.Network.Codecs;

namespace Server.Network
{
    public static class NetworkManager
    {
        private static readonly Logger Log = Logger.GetLogger("NetworkManager");

        /*public static async Task RunUdpServerAsync(int port, IChannelHandler packetHandler, ILoop loop)
        {
            var group = new MultithreadEventLoopGroup();

            try
            {
                var bootstrap = new Bootstrap();
                bootstrap
                    .Group(group)
                    .Channel<SocketDatagramChannel>()
                    .Handler(new ActionChannelInitializer<IChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;
                        pipeline.AddLast("handler", packetHandler);
                    }));

                var bootstrapChannel = await bootstrap.BindAsync(port).ConfigureAwait(false);

                Log.Info($"Server is listening to port {port}!");
                loop.Loop();

                await bootstrapChannel.CloseAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.Error("", ex);
            }
            finally
            {
                Task.WaitAll(group.ShutdownGracefullyAsync());
            }
        }*/

        public static async Task RunTcpServerAsync(int port, ICustomTcpChannelHandler channelHandler, ILoop loop)
        {
            var bossGroup = new MultithreadEventLoopGroup();
            var workerGroup = new MultithreadEventLoopGroup();

            try
            {
                var bootstrap = new ServerBootstrap();
                bootstrap
                    .Option(ChannelOption.SoBacklog, 16)
                    .Option(ChannelOption.TcpNodelay, true)
                    .Option(ChannelOption.SoKeepalive, true)
                    .Group(bossGroup, workerGroup)
                    .Channel<TcpServerSocketChannel>()
                    .ChildHandler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        var pipeline = channel.Pipeline;

                        pipeline.AddLast("framing-enc", new FunatEncoder());
                        //pipeline.AddLast("framing-dec", new FunatDecoder());

                        pipeline.AddLast("session", channelHandler.GetNewWithChannelHandler(channel));
                    }));

                var bootstrapChannel = await bootstrap.BindAsync(port).ConfigureAwait(false);

                Log.Info($"Server is listening to port {port}!");
                loop.Loop();

                await bootstrapChannel.CloseAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.Error("", ex);
            }
            finally
            {
                Task.WaitAll(workerGroup.ShutdownGracefullyAsync(), bossGroup.ShutdownGracefullyAsync());
            }
        }
    }
}
