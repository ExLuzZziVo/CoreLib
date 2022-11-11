using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using OpenGost.Security.Cryptography;

namespace CoreLib.CORE.Helpers.CryptoHelpers
{
    /// <summary>
    /// Класс для шифрования строк и объектов с помощью шифра Кузнечик (ГОСТ 34.12-2015). Чтобы использовать этот класс необходимо вызвать метод <see cref="OpenGostCryptoConfig.ConfigureCryptographicServices"/> при запуске приложения
    /// </summary>
    public class GostCryptoService : CryptoService
    {
        public GostCryptoService(string key, string salt) : base(key, salt) { }

        public GostCryptoService(string key, byte[] salt) : base(key, salt) { }

        /// <summary>
        /// Method that creates GOST encryptor
        /// </summary>
        /// <param name="s">Data stream</param>
        /// <returns>GOST encryptor</returns>
        protected override ICryptoTransform CreateEncryptor(Stream s)
        {
            using (var gostAlg = Grasshopper.Create())
            {
                gostAlg.Key = GenerateKey(gostAlg.KeySize / 8);
                s.Write(BitConverter.GetBytes(gostAlg.IV.Length), 0, sizeof(int));
                s.Write(gostAlg.IV, 0, gostAlg.IV.Length);

                return gostAlg.CreateEncryptor(gostAlg.Key, gostAlg.IV);
            }
        }

        /// <summary>
        /// Method that creates GOST decryptor
        /// </summary>
        /// <param name="s">Data stream</param>
        /// <returns>GOST decryptor</returns>
        protected override ICryptoTransform CreateDecryptor(Stream s)
        {
            using (var gostAlg = Grasshopper.Create())
            {
                gostAlg.Key = GenerateKey(gostAlg.KeySize / 8);
                gostAlg.IV = ReadByteArray(s);

                return gostAlg.CreateDecryptor(gostAlg.Key, gostAlg.IV);
            }
        }

        /// <summary>
        /// Генерирует ключ шифрования на основе заданного ключа и соли
        /// </summary>
        /// <param name="keySize">Размер ключа</param>
        /// <returns>Ключ</returns>
        private byte[] GenerateKey(int keySize)
        {
            using (var hash = Streebog512.Create())
            {
                var keyBytes = Encoding.Unicode.GetBytes(Key);
                
                var keyWithSalt = new byte[Salt.Length + keyBytes.Length];
                
                Salt.CopyTo(keyWithSalt, 0);
                keyBytes.CopyTo(keyWithSalt, Salt.Length);
                
                var gostKey = new byte[keySize];
                Buffer.BlockCopy(hash.ComputeHash(keyWithSalt), 0, gostKey, 0, keySize);

                return gostKey;
            }
        }
    }
}