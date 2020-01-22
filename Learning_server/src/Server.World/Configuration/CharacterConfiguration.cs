using System;
using System.Collections.Generic;
using System.Text;

namespace Server.World.Configuration
{
    public class CharacterConfiguration
    {
        public byte MaxAccountCharacters { get; set; } = 8;
        
        public byte MaxVisionRange { get; set; } = 128;
        
        /// <summary>
        /// Chunks that can be seen.
        /// </summary>
        public byte MaxChunkDistanceView { get; set; } = 2;
    }
}
