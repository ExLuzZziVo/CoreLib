#region

using System.Security.Cryptography;
using System.Text;
using CoreLib.CORE.Helpers.ObjectHelpers;

#endregion

namespace CoreLib.CORE.Helpers.CryptoHelpers
{
    public static class HashGenerators
    {
        /// <summary>
        /// Generates hash of provided string using specified algorithm and salt
        /// </summary>
        /// <param name="source">String to get hash from</param>
        /// <param name="hashAlgorithm">Hash algorithm</param>
        /// <param name="salt">Salt</param>
        /// <returns>Hash of <paramref name="source"/></returns>
        public static string GetHash(this string source, HashAlgorithm hashAlgorithm, byte[] salt = null)
        {
            var input = Encoding.Unicode.GetBytes(source);
            byte[] inputToHash;

            if (salt != null)
            {
                inputToHash = new byte[salt.Length + input.Length];
                salt.CopyTo(inputToHash, 0);
                input.CopyTo(inputToHash, salt.Length);
            }
            else
            {
                inputToHash = input;
            }

            var hashedBytes = hashAlgorithm.ComputeHash(inputToHash);

            return hashedBytes.ToHexString();
        }

        /// <summary>
        /// Generates MD5 hash of provided string
        /// </summary>
        /// <param name="source">String to get MD5 hash from</param>
        /// <param name="salt">Salt</param>
        /// <returns>MD5 hash of <paramref name="source"/></returns>
        public static string GetMD5Hash(this string source, byte[] salt = null)
        {
            using (var md5 = MD5.Create())
            {
                return GetHash(source, md5, salt);
            }
        }

        /// <summary>
        /// Generates SHA1 hash of provided string
        /// </summary>
        /// <param name="source">String to get SHA1 hash from</param>
        /// <param name="salt">Salt</param>
        /// <returns>SHA1 hash of <paramref name="source"/></returns>
        public static string GetSHA1Hash(this string source, byte[] salt = null)
        {
            using (var shaManaged = SHA1.Create())
            {
                return GetHash(source, shaManaged, salt);
            }
        }

        /// <summary>
        /// Generates SHA256 hash of provided string
        /// </summary>
        /// <param name="source">String to get SHA256 hash from</param>
        /// <param name="salt">Salt</param>
        /// <returns>SHA256 hash of <paramref name="source"/></returns>
        public static string GetSHA256Hash(this string source, byte[] salt = null)
        {
            using (var shaManaged = SHA256.Create())
            {
                return GetHash(source, shaManaged, salt);
            }
        }

        /// <summary>
        /// Generates SHA384 hash of provided string
        /// </summary>
        /// <param name="source">String to get SHA384 hash from</param>
        /// <param name="salt">Salt</param>
        /// <returns>SHA384 hash of <paramref name="source"/></returns>
        public static string GetSHA384Hash(this string source, byte[] salt = null)
        {
            using (var shaManaged = SHA384.Create())
            {
                return GetHash(source, shaManaged, salt);
            }
        }

        /// <summary>
        /// Generates SHA512 hash of provided string
        /// </summary>
        /// <param name="source">String to get SHA512 hash from</param>
        /// <param name="salt">Salt</param>
        /// <returns>SHA512 hash of <paramref name="source"/></returns>
        public static string GetSHA512Hash(this string source, byte[] salt = null)
        {
            using (var shaManaged = SHA512.Create())
            {
                return GetHash(source, shaManaged, salt);
            }
        }
    }
}