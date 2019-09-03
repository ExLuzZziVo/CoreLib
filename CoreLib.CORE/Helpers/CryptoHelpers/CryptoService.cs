﻿#region

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using CoreLib.CORE.Helpers.AssemblyHelpers;
using CoreLib.CORE.Helpers.ObjectHelpers;
using CoreLib.CORE.Helpers.StringHelpers;
using GOST;

#endregion

namespace CoreLib.CORE.Helpers.CryptoHelpers
{
    public enum CryptoType
    {
        AES,
        GOST
    }

    public class CryptoService
    {
        private readonly string _key;
        private readonly CryptoType _provider;
        private readonly byte[] _salt;

        public CryptoService(string key,
            string salt, CryptoType provider = CryptoType.AES)
        {
            if (key.IsNullOrEmptyOrWhiteSpace() || salt.IsNullOrEmptyOrWhiteSpace())
                throw new CryptographicException("Key and Salt must be specified!");
            _provider = provider;
            _salt = Encoding.Unicode.GetBytes(salt);
            _key = key;
        }
        public CryptoService(string key,
            byte[] salt, CryptoType provider = CryptoType.AES)
        {
            if (key.IsNullOrEmptyOrWhiteSpace() || salt.Length==0)
                throw new CryptographicException("Key and Salt must be specified!");
            _provider = provider;
            _salt = salt;
            _key = key;
        }
        private ICryptoTransform CreateEncryptor(Stream s)
        {
            using (var key = new Rfc2898DeriveBytes(_key, _salt))
            {
                using (var aesAlg = new RijndaelManaged())
                {
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    s.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                    s.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    return aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                }
            }
        }

        private ICryptoTransform CreateDecryptor(Stream s)
        {
            using(var key = new Rfc2898DeriveBytes(_key, _salt))
            {
                using (var aesAlg = new RijndaelManaged())
                {
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    aesAlg.IV = ReadByteArray(s);
                    return aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                }
            }
        }

        private void EncryptGost(Stream s, GostEncryptOperationDelegate actions)
        {
            using (var key = new Rfc2898DeriveBytes(_key, _salt))
            {
                using (var aesAlg = new RijndaelManaged())
                {
                    var gostKey = key.GetBytes(aesAlg.KeySize / 8);
                    var gostIV = aesAlg.IV.Take(8).ToArray();
                    s.Write(BitConverter.GetBytes(gostIV.Length), 0, sizeof(int));
                    s.Write(gostIV, 0, gostIV.Length);
                    using (var gost = new GOSTManaged())
                    {
                        var encrypted = actions(gost, gostKey, gostIV);
                        s.Write(encrypted, 0, encrypted.Length);
                    }
                }
            }
        }

        private byte[] DecryptGost(Stream s, GostDecryptOperationDelegate actions)
        {
            using (var key = new Rfc2898DeriveBytes(_key, _salt))
            {
                using (var aesAlg = new RijndaelManaged())
                {
                    var gostIV = ReadByteArray(s);
                    var gostKey = key.GetBytes(aesAlg.KeySize / 8);
                    var encrypted = new byte[s.Length - s.Position];
                    s.Read(encrypted, 0, encrypted.Length);
                    using (var gost = new GOSTManaged())
                    {
                        return actions(gost, gostKey, gostIV, encrypted);
                    }
                }
            }
        }


        private static byte[] ReadByteArray(Stream s)
        {
            var rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
            }

            var buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
            }

            return buffer;
        }

        public string EncryptString(string str)
        {
            using (var msEncrypt = new MemoryStream())
            {
                switch (_provider)
                {
                    default:
                    case CryptoType.AES:
                        using (var csEncrypt =
                            new CryptoStream(msEncrypt, CreateEncryptor(msEncrypt), CryptoStreamMode.Write))
                        {
                            using (var swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(str);
                            }
                        }

                        return Convert.ToBase64String(msEncrypt.ToArray());
                    case CryptoType.GOST:
                        EncryptGost(msEncrypt,
                            (gost, key, iv) => gost.XOREncode(key, iv, Encoding.Unicode.GetBytes(str)));
                        return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public string DecryptString(string str)
        {
            try
            {
                var bytes = Convert.FromBase64String(str);
                using (var msDecrypt = new MemoryStream(bytes))
                {
                    switch (_provider)
                    {
                        default:
                        case CryptoType.AES:
                            using (var csDecrypt =
                                new CryptoStream(msDecrypt, CreateDecryptor(msDecrypt), CryptoStreamMode.Read))
                            {
                                using (var srDecrypt = new StreamReader(csDecrypt))
                                {
                                    return srDecrypt.ReadToEnd();
                                }
                            }
                        case CryptoType.GOST:
                            return Encoding.Unicode.GetString(DecryptGost(msDecrypt,
                                (gost, key, iv, encrypted) => gost.XORDecode(key, iv, encrypted)));
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public void EncryptObject<T>(Stream s, T obj)
        {
            switch (_provider)
            {
                case CryptoType.AES:
                    using (var cs = new CryptoStream(s, CreateEncryptor(s), CryptoStreamMode.Write))
                    {
                        new BinaryFormatter().Serialize(cs, obj);
                    }

                    break;
                case CryptoType.GOST:
                    EncryptGost(s,
                        (gost, key, iv) => gost.XOREncode(key, iv, obj.ToByteArray()));
                    break;
            }
        }

        public T DecryptObject<T>(Stream s)
        {
            switch (_provider)
            {
                default:
                case CryptoType.AES:
                    using (var cs = new CryptoStream(s, CreateDecryptor(s), CryptoStreamMode.Read))
                    {
                        return (T) new BinaryFormatter
                            {Binder = new SearchAssembliesBinder(Assembly.GetEntryAssembly(), true)}.Deserialize(cs);
                    }
                case CryptoType.GOST:
                {
                    return DecryptGost(s,
                            (gost, key, iv, encrypted) => gost.XORDecode(key, iv, encrypted))
                        .GetObject<T>(Assembly.GetEntryAssembly());
                }
            }
        }

        private delegate byte[] GostEncryptOperationDelegate(GOSTManaged gost, byte[] key, byte[] iv);

        private delegate byte[] GostDecryptOperationDelegate(GOSTManaged gost, byte[] key, byte[] iv,
            byte[] data);
    }
}