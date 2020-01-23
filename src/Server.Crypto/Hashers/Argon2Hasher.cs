using System;
using System.Collections.Generic;
using System.Text;
using Konscious.Security.Cryptography;

namespace Server.Crypto.Hashers
{
    public class Argon2Hasher
    {
        public byte[] Hash { get; set; }

        public int DegreeOfParallelism { get; set; }

        public int MemorySize { get; set; }

        public int Iterations { get; set; }

        public byte[] Salt { get; set; }

        public Argon2Hasher()
        {
        }

        public Argon2Hasher(string encodedHash)
        {
            foreach (var parameter in encodedHash.Split('$'))
            {
                if (string.IsNullOrEmpty(parameter))
                {
                    continue;
                }

                var refParameter = parameter.Remove(0, 2);

                switch(parameter[0])
                {
                    case 'm':
                        MemorySize = int.Parse(refParameter);
                        break;
                    case 'i':
                        Iterations = int.Parse(refParameter);
                        break;
                    case 'p':
                        DegreeOfParallelism = int.Parse(refParameter);
                        break;
                    case 's':
                        Salt = HashUtilities.ToByteArray(refParameter);
                        break;
                    case 'h':
                        Hash = HashUtilities.ToByteArray(refParameter);
                        break;
                }
            }
        }

        /// <summary>
        /// Hashes the string with the actual specs and compares it with the hash already contained.
        /// Returns true in the case that they coincide.
        /// </summary>
        /// <param name="toHash"></param>
        /// <returns></returns>
        public bool CheckString(byte[] toHash)
        {
            return Convert.ToBase64String(Hash) == Convert.ToBase64String(HashWithSpec(toHash, Salt));
        }

        public bool SpecEquals(Argon2Hasher argon2)
        {
            return DegreeOfParallelism == argon2.DegreeOfParallelism
                && MemorySize == argon2.MemorySize
                && Iterations == argon2.Iterations;
        }

        public string GetEncodedHash()
        {
            return $"$t=argon2id$v=19$m={MemorySize}$i={Iterations}$p={DegreeOfParallelism}$s={HashUtilities.ToString(Salt)}$h={HashUtilities.ToString(Hash)}";
        }

        public byte[] HashWithSpec(byte[] toHash, byte[] salt, int hashLength)
        {
            return HashThis(toHash, DegreeOfParallelism, MemorySize, Iterations, salt, hashLength);
        }

        public byte[] HashWithSpec(byte[] toHash, byte[] salt)
        {
            return HashThis(toHash, DegreeOfParallelism, MemorySize, Iterations, salt);
        }

        public byte[] HashWithSpec(byte[] toHash)
        {
            return HashThis(toHash, DegreeOfParallelism, MemorySize, Iterations, Salt);
        }

        private static byte[] HashThis(byte[] toHash, int degreeOfParallelism, int memorySize, int iterations, byte[] salt, int hashLength = 32, string associatedData = null, string knownSecret = "Funat")
        {
            using var argon2 = new Argon2id(toHash)
            {
                DegreeOfParallelism = degreeOfParallelism,
                MemorySize = memorySize,
                Iterations = iterations,
                Salt = salt,
            };

            if (associatedData != null)
            {
                argon2.AssociatedData = Encoding.Default.GetBytes(associatedData);
            }

            if (knownSecret != null)
            {
                argon2.KnownSecret = Encoding.Default.GetBytes(knownSecret);
            }

            return argon2.GetBytes(hashLength);
        }
    }
}
