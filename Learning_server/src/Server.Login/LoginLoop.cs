using System;
using System.Collections.Generic;
using System.Text;
using Server.Network;
using System.Threading;

namespace Server.Login
{
    internal class LoginLoop : ILoop
    {
        public void Loop()
        {
            while (Console.ReadLine() != "stop")
            {
                Thread.Sleep(2000);
            }
        }
    }
}
