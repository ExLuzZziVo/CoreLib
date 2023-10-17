#region

using System.IO;
using System.Threading.Tasks;

#endregion

namespace CoreLib.CORE.Interfaces
{
    public interface ICryptoService
    {
        /// <summary>
        /// Encrypts string asynchronously
        /// </summary>
        /// <param name="str">String to encrypt</param>
        /// <returns>A task that represents the asynchronous string encryption operation</returns>
        Task<string> EncryptStringAsync(string str);

        /// <summary>
        /// Encrypts string
        /// </summary>
        /// <param name="str">String to encrypt</param>
        /// <returns>Encrypted string</returns>
        string EncryptString(string str);

        /// <summary>
        /// Decrypts string asynchronously
        /// </summary>
        /// <param name="str">String to decrypt</param>
        /// <returns>A task that represents the asynchronous string decryption operation. If decryption was failed, the resulted string will be null</returns>
        Task<string> DecryptStringAsync(string str);

        /// <summary>
        /// Decrypts string
        /// </summary>
        /// <param name="str">String to decrypt</param>
        /// <returns>Decrypted string. If decryption was failed, returns null</returns>
        string DecryptString(string str);

        /// <summary>
        /// Encrypts object asynchronously
        /// </summary>
        /// <param name="s">Data stream</param>
        /// <param name="obj">Object to encrypt</param>
        /// <returns>A task that represents the asynchronous object encryption operation</returns>
        Task EncryptObjectAsync<T>(Stream s, T obj);

        /// <summary>
        /// Encrypts object
        /// </summary>
        /// <param name="s">Data stream</param>
        /// <param name="obj">Object to encrypt</param>
        void EncryptObject<T>(Stream s, T obj);

        /// <summary>
        /// Decrypts object asynchronously
        /// </summary>
        /// <param name="s">Data stream</param>
        /// <returns>A task that represents the asynchronous object decryption operation</returns>
        Task<T> DecryptObjectAsync<T>(Stream s);

        /// <summary>
        /// Decrypts object
        /// </summary>
        /// <param name="s">Data stream</param>
        /// <returns>Decrypted object</returns>
        T DecryptObject<T>(Stream s);
    }
}