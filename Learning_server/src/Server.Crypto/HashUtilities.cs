using System;
using System.Collections.Generic;
using System.Management;
using System.Text;

namespace Server.Crypto
{
    public static class HashUtilities
    {
        public static byte[] ToByteArray(string text)
        {
            return Convert.FromBase64String(text);
        }

        public static string ToString(byte[] ByteArray)
        {
            if (ByteArray == null)
            {
                return "";
            }
            return Convert.ToBase64String(ByteArray);
            /*StringBuilder builder = new StringBuilder();
            for (int i = 0; i < ByteArray.Length; i++)
            {
                builder.Append(ByteArray[i].ToString("x2"));
            }
            return builder.ToString();*/
        }

        public static int GetActualFreeMemory()
        {
            ObjectQuery wql = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wql);

            try
            {
                foreach (ManagementObject item in searcher.Get())
                {
                    return Convert.ToInt32(item["FreeVirtualMemory"]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return 0;
        }
    }
}
