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
            using (var shaManaged = new SHA256Managed())
            {
                return Encoding.Unicode.GetString(shaManaged.ComputeHash(Encoding.Unicode.GetBytes(source)));
            }
        }

        public static string GetSHA512Hash(this string source)
        {
            using (var shaManaged = new SHA512Managed())
            {
                return Encoding.Unicode.GetString(shaManaged.ComputeHash(Encoding.Unicode.GetBytes(source)));
            }
        }
    }
}