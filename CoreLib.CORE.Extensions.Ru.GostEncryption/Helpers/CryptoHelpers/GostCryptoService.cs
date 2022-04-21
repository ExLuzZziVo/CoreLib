using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using CoreLib.CORE.Helpers.ObjectHelpers;
using GOSTCore;
using GOSTCore.Gost.Types;

namespace CoreLib.CORE.Helpers.CryptoHelpers
{
    /// <summary>
    /// Класс для шифрования строк и объектов с помощью шифра ГОСТ 28147-89
    /// </summary>
    public class GostCryptoService : CryptoService
    {
        public GostCryptoService(string key, string salt) : base(key, salt) { }

        public GostCryptoService(string key, byte[] salt) : base(key, salt) { }

        public override string EncryptString(string str)
        {
            using (var msEncrypt = new MemoryStream())
            {
                EncryptGost(msEncrypt, Encoding.Unicode.GetBytes(str));

                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }

        public override string DecryptString(string str)
        {
            try
            {
                var bytes = Convert.FromBase64String(str);

                using (var msDecrypt = new MemoryStream(bytes))
                {
                    return Encoding.Unicode.GetString(DecryptGost(msDecrypt));
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public override void EncryptObject<T>(Stream s, T obj)
        {
            EncryptGost(s, obj.ToByteArray());
        }

        public override T DecryptObject<T>(Stream s)
        {
            return DecryptGost(s).GetObject<T>(Assembly.GetEntryAssembly());
        }

        private void EncryptGost(Stream s, byte[] obj)
        {
            using (var key = new Rfc2898DeriveBytes(Key, Salt))
            {
                using (var aesAlg = Aes.Create())
                {
                    var gostKey = key.GetBytes(aesAlg.KeySize / 8);
                    var gostIV = aesAlg.IV.Take(8).ToArray();
                    s.Write(BitConverter.GetBytes(gostIV.Length), 0, sizeof(int));
                    s.Write(gostIV, 0, gostIV.Length);
                    var encryptedObject = Xor.Encode(gostKey, gostIV, obj, SBlockTypes.GOST);
                    s.Write(encryptedObject, 0, encryptedObject.Length);
                }
            }
        }

        private byte[] DecryptGost(Stream s)
        {
            using (var key = new Rfc2898DeriveBytes(Key, Salt))
            {
                using (var aesAlg = Aes.Create())
                {
                    var gostIV = ReadByteArray(s);
                    var gostKey = key.GetBytes(aesAlg.KeySize / 8);
                    var encrypted = new byte[s.Length - s.Position];
                    s.Read(encrypted, 0, encrypted.Length);

                    return Xor.Decode(gostKey, gostIV, encrypted, SBlockTypes.GOST);
                }
            }
        }
    }
}