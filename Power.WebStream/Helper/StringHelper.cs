using System;
using System.Security.Cryptography;
using System.Text;

namespace Power.WebStream
{
    public class StringHelper
    {
        public static string Random(int count)
        {
            string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            Random randrom = new Random((int)DateTime.Now.Ticks);
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                str.Append(chars[randrom.Next(chars.Length)]);
            }

            return str.ToString();
        }

        public static string Base64(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            sbyte[] rBytes = new sbyte[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
            {
                rBytes[i] = bytes[i] < 127 ? (sbyte)bytes[i] : (sbyte)(bytes[i] - 256);
            }
            byte[] unsignedByteArray = (byte[])(Array)rBytes;
            return Encoding.Default.GetString(unsignedByteArray);
        }

        public static string SHA256(string str)
        {
            byte[] SHA256Data = Encoding.UTF8.GetBytes(str);
            SHA256Managed Sha256 = new SHA256Managed();
            byte[] hashmessage = Sha256.ComputeHash(SHA256Data);
            sbyte[] rBytes = new sbyte[hashmessage.Length];

            for (int i = 0; i < hashmessage.Length; i++)
            {
                rBytes[i] = hashmessage[i] < 127 ? (sbyte)hashmessage[i] : (sbyte)(hashmessage[i] - 256);
            }

            byte[] unsignedByteArray = (byte[])(Array)rBytes;
            return Encoding.Default.GetString(unsignedByteArray);
        }
    }
}
