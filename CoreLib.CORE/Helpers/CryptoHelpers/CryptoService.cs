#region

using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CoreLib.CORE.Helpers.AssemblyHelpers;
using CoreLib.CORE.Helpers.StringHelpers;
using CoreLib.CORE.Interfaces;

#endregion

namespace CoreLib.CORE.Helpers.CryptoHelpers
{
    /// <summary>
    /// A helper class that encrypts/decrypts strings or objects using AES
    /// </summary>
    public class CryptoService : ICryptoService
    {
        protected readonly string Key;
        protected readonly byte[] Salt;

        /// <summary>
        /// A helper class that encrypts/decrypts strings or objects using AES
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="salt">Salt</param>
        public CryptoService(string key, string salt)
        {
            if (key.IsNullOrEmptyOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (salt.IsNullOrEmptyOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(salt));
            }

            Salt = Encoding.Unicode.GetBytes(salt);
            Key = key;
        }

        /// <summary>
        /// A helper class that encrypts/decrypts strings or objects using AES
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="salt">Salt</param>
        public CryptoService(string key, byte[] salt)
        {
            if (key.IsNullOrEmptyOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (salt == null || salt.Length == 0)
            {
                throw new ArgumentNullException(nameof(salt));
            }

            Salt = salt;
            Key = key;
        }

        /// <summary>
        /// Method that creates AES encryptor
        /// </summary>
        /// <param name="s">Data stream</param>
        /// <returns>AES encryptor</returns>
        private ICryptoTransform CreateEncryptor(Stream s)
        {
            using (var key = new Rfc2898DeriveBytes(Key, Salt))
            {
                using (var aesAlg = Aes.Create())
                {
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    s.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                    s.Write(aesAlg.IV, 0, aesAlg.IV.Length);

                    return aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                }
            }
        }

        /// <summary>
        /// Method that creates AES decryptor
        /// </summary>
        /// <param name="s">Data stream</param>
        /// <returns>AES decryptor</returns>
        private ICryptoTransform CreateDecryptor(Stream s)
        {
            using (var key = new Rfc2898DeriveBytes(Key, Salt))
            {
                using (var aesAlg = Aes.Create())
                {
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    aesAlg.IV = ReadByteArray(s);

                    return aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                }
            }
        }

        /// <summary>
        /// A helper method that reads the IV from data stream
        /// </summary>
        /// <param name="s">Data stream</param>
        /// <returns>Buffer that contains the IV</returns>
        protected static byte[] ReadByteArray(Stream s)
        {
            var rawLength = new byte[sizeof(int)];

            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }

            var buffer = new byte[BitConverter.ToInt32(rawLength, 0)];

            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new SystemException("Byte array was not read properly");
            }

            return buffer;
        }

        public Task<string> EncryptStringAsync(string str)
        {
            return Task.Run(() => EncryptString(str));
        }

        public virtual string EncryptString(string str)
        {
            using (var msEncrypt = new MemoryStream())
            {
                using (var csEncrypt =
                    new CryptoStream(msEncrypt, CreateEncryptor(msEncrypt), CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(str);
                    }
                }

                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }

        public virtual Task<string> DecryptStringAsync(string str)
        {
            return Task.Run(() => DecryptString(str));
        }

        public virtual string DecryptString(string str)
        {
            try
            {
                var bytes = Convert.FromBase64String(str);

                using (var msDecrypt = new MemoryStream(bytes))
                {
                    using (var csDecrypt =
                        new CryptoStream(msDecrypt, CreateDecryptor(msDecrypt), CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public virtual Task EncryptObjectAsync<T>(Stream s, T obj)
        {
            return Task.Run(() => EncryptObject(s, obj));
        }

        public virtual void EncryptObject<T>(Stream s, T obj)
        {
            using (var cs = new CryptoStream(s, CreateEncryptor(s), CryptoStreamMode.Write))
            {
                new BinaryFormatter().Serialize(cs, obj);
            }
        }

        public virtual Task<T> DecryptObjectAsync<T>(Stream s)
        {
            return Task.Run(() => DecryptObject<T>(s));
        }

        public virtual T DecryptObject<T>(Stream s)
        {
            using (var cs = new CryptoStream(s, CreateDecryptor(s), CryptoStreamMode.Read))
            {
                return (T) new BinaryFormatter
                    {Binder = new SearchAssembliesBinder(Assembly.GetEntryAssembly(), true)}.Deserialize(cs);
            }
        }
    }
}