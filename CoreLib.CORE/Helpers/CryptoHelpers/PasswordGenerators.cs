#region

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using CoreLib.CORE.Helpers.StringHelpers;

#endregion

namespace CoreLib.CORE.Helpers.CryptoHelpers
{
    public static class PasswordGenerators
    {
        private const string PasswordChars =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz012345679-=!@#$%^&*()_+,.?";

        /// <summary>
        /// Generates password using <see cref="Guid"/>
        /// </summary>
        /// <param name="length">Requested password length. Default value is 8</param>
        /// <returns>Generated <see cref="Guid"/> substring size of <paramref name="length"/></returns>
        public static string GenerateRandomPasswordUsingGuid(int length = 8)
        {
            if (length <= 0)
            {
                return string.Empty;
            }

            var generatedString = Guid.NewGuid().ToString("D").ToRandomCase();

            return generatedString.Substring(0, length > generatedString.Length ? generatedString.Length : length);
        }

        /// <summary>
        /// Generates password using <see cref="Path.GetRandomFileName()"/> with random char case
        /// </summary>
        /// <returns>Generated <see cref="Path.GetRandomFileName()"/> with random char case</returns>
        public static string GenerateRandomPasswordUsingFileName()
        {
            return Path.GetRandomFileName().Remove('.').ToRandomCase();
        }

        /// <summary>
        /// Generates password using <see cref="Random"/> and provided chars
        /// </summary>
        /// <param name="passwordChars">Chars used to generate password</param>
        /// <param name="length">Requested password length. Default value is 8</param>
        /// <returns>Generated password</returns>
        public static string GenerateRandomPassword(this string passwordChars, int length = 8)
        {
            if (length <= 0)
            {
                return string.Empty;
            }

            var randomString = new StringBuilder();
            var random = new Random();

            for (var i = 0; i < length; i++)
            {
                randomString.Append(passwordChars[random.Next(passwordChars.Length)]);
            }

            return randomString.ToString();
        }

        /// <summary>
        /// Generates password using <see cref="Random"/> and <see cref="PasswordChars"/>
        /// </summary>
        /// <param name="length">Requested password length. Default value is 8</param>
        /// <returns>Generated password</returns>
        public static string GenerateRandomPassword(int length = 8)
        {
            return PasswordChars.GenerateRandomPassword(length);
        }

        /// <summary>
        /// Generates strong password using <see cref="RNGCryptoServiceProvider"/> and provided chars
        /// </summary>
        /// <param name="passwordChars">Chars used to generate password</param>
        /// <param name="length">Requested password length. Default value is 8</param>
        /// <returns>Generated password</returns>
        public static string GenerateStrongRandomPassword(this string passwordChars, int length = 8)
        {
            if (length <= 0)
            {
                return string.Empty;
            }

            var data = new byte[length];

            using (var crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }

            var result = new StringBuilder(length);

            foreach (var b in data)
            {
                result.Append(passwordChars[b % passwordChars.Length]);
            }

            return result.ToString();
        }

        /// <summary>
        /// Generates strong password using <see cref="RNGCryptoServiceProvider"/> and <see cref="PasswordChars"/>
        /// </summary>
        /// <param name="length">Requested password length. Default value is 8</param>
        /// <returns>Generated password</returns>
        public static string GenerateStrongRandomPassword(int length = 8)
        {
            return PasswordChars.GenerateStrongRandomPassword(length);
        }
    }
}