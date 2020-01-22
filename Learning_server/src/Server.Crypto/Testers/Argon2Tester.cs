using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Server.Crypto.Hashers;
using ChickenAPI.Core.Logging;
using Server.Core.Logging;

namespace Server.Crypto.Testers
{
    public static class Argon2Tester
    {
        private static readonly ILogger Log = Logger.GetLogger("Argon2Tester");

        /// <summary>
        /// Returns the ArgonHasher with the optimal specs for this PC.
        /// </summary>
        public static Argon2Hasher GetBestSettings(HasherConfiguration hasherConfiguration)
        {
            /*int tmp;

            if (hasherConfiguration.RamToUseIn_kB == null)
            {
                tmp = HashUtilities.GetActualFreeMemory() * 25 / 100 / Environment.ProcessorCount;
            }
            else
            {
                tmp = (int)hasherConfiguration.RamToUseIn_kB / Environment.ProcessorCount;
            }
            var m = tmp > minimumMemory ? tmp : minimumMemory;*/
            const int minimumMemory = 64 * 1024;

            var argon2 = new Argon2Hasher
            {
                DegreeOfParallelism = hasherConfiguration.ProcessorsToUse == 0 ? Environment.ProcessorCount / 2 : hasherConfiguration.ProcessorsToUse,
                MemorySize = 1024,
                Iterations = 1,
            };

            Log.Info("Calculating the optimal parameters...");

            var tempArray = new byte[16];
            long elapsedMs = -1;
            while (!(hasherConfiguration.HashingMinimumTimeWait <= elapsedMs && argon2.MemorySize > minimumMemory))
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                argon2.HashWithSpec(tempArray, tempArray);
                watch.Stop();
                elapsedMs = watch.ElapsedMilliseconds;

                argon2.MemorySize = elapsedMs < hasherConfiguration.HashingMaximumTimeWait ?
                    Convert.ToInt32(argon2.MemorySize * 1.1) : Convert.ToInt32(argon2.MemorySize * 0.9);
            }
            Log.Info($"The resultant parameters in {elapsedMs}ms are: {argon2.GetEncodedHash()}");
            return argon2;
        }
    }
}
