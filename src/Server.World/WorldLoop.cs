using System;
using System.Collections.Generic;
using System.Text;
using Server.Network;
using System.Threading;
using Autofac;
using ChickenAPI.Core.Logging;
using NLog.Fluent;
using Server.Core.IoC;
using Server.World.Configuration;
using Server.World.Game.Map;
using Server.World.Game.Map.Interfaces;
using System.Threading.Tasks;

namespace Server.World
{
    internal class WorldLoop : ILoop
    {
        private static readonly IMapManager MapManager = UsefulContainer.Instance.Resolve<IMapManager>();

        private static readonly ILogger Log = UsefulContainer.Instance.Resolve<ILogger>();

        private static readonly float Tps = 1_000 / UsefulContainer.Instance.Resolve<WorldConfiguration>().Tps;
        
        public void Loop()
        {
            while (true)
            {
                try
                {
                    var next = DateTime.UtcNow.AddMilliseconds(Tps);
                    MapManager.Update();
                    var after = DateTime.UtcNow;

                    if (next > after)
                    {
                        Thread.Sleep((next - after).Milliseconds);
                    }
                }
                catch (Exception e)
                {
                    Log.Fatal("[WorldLoop]", e);
                }
            }
        }

        private Task Collect()
        {
            GC.Collect();
            return Task.CompletedTask;
        }
    }
}
