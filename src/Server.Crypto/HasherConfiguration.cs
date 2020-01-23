using System;
using System.Collections.Generic;
using System.Text;
using Server.Crypto.Hashers;

namespace Server.Crypto
{
    public class HasherConfiguration
    {
        public int HashingMinimumTimeWait { get; set; } = 650;

        public int HashingMaximumTimeWait { get; set; } = 750;

        public ushort ProcessorsToUse { get; set; }
    }
}
