#region

using System.Security.Cryptography;
using System.Text;

#endregion

namespace CoreLib.CORE.Helpers.CryptoHelpers
{
    public static class HashGenerators
    {
        public static string GetSHA256Hash(this string source)
        {
            return Encoding.Unicode.GetString(new SHA256Managed().ComputeHash(Encoding.Unicode.GetBytes(source)));
        }

        public static string GetSHA512Hash(this string source)
        {
            return Encoding.Unicode.GetString(new SHA512Managed().ComputeHash(Encoding.Unicode.GetBytes(source)));
        }
    }
}