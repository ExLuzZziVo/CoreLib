#region

using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CoreLib.CORE.Helpers.CryptoHelpers;
using CoreLib.CORE.Helpers.StringHelpers;

#endregion

namespace CoreLib.STANDALONE.Types
{
    /// <summary>
    /// A class that can be inherited for use as an application settings file, application data file, etc. Supports creating the backup and encryption using <see cref="CoreLib.CORE.Interfaces.ICryptoService"/>. A class will be serialized using Json
    /// </summary>
    public abstract class ApplicationFile : ViewModelBase
    {
        [field: NonSerialized] [JsonIgnore] private string _backupFolder;
        [field: NonSerialized] [JsonIgnore] private CryptoService _cryptoService;
        [field: NonSerialized] [JsonIgnore] private string _directoryToStore;
        [field: NonSerialized] [JsonIgnore] private string _fileName;
        [field: NonSerialized] [JsonIgnore] private string _filePath;
        [field: NonSerialized] [JsonIgnore] private bool _isBackupEnabled;

        /// <summary>
        /// A class that can be inherited for use as an application settings file, application data file, etc. Supports creating the backup and encryption using <see cref="CoreLib.CORE.Interfaces.ICryptoService"/>
        /// </summary>
        /// <param name="directoryToStore">The path to the directory for storing the file</param>
        /// <param name="fileName">Name of the file</param>
        /// <param name="cryptoService"><see cref="CoreLib.CORE.Interfaces.ICryptoService"/> implementation for file encryption</param>
        /// <param name="isBackupEnabled">A flag indicating that backup files will be created. The backup will be stored in subdirectory named "Backup". The default value is false</param>
        protected ApplicationFile(string directoryToStore, string fileName, CryptoService cryptoService,
            bool isBackupEnabled = false)
        {
            Init(directoryToStore, fileName, cryptoService, isBackupEnabled);
        }

        /// <summary>
        /// Initializes private fields
        /// </summary>
        /// <param name="directoryToStore">The path to the directory for storing the file</param>
        /// <param name="fileName">Name of the file</param>
        /// <param name="cryptoService"><see cref="CoreLib.CORE.Interfaces.ICryptoService"/> implementation for file encryption</param>
        /// <param name="isBackupEnabled">A flag indicating that backup files will be created. The backup will be stored in subdirectory named "Backup". The default value is false</param>
        protected void Init(string directoryToStore, string fileName, CryptoService cryptoService,
            bool isBackupEnabled = false)
        {
            _directoryToStore = directoryToStore;
            _fileName = fileName;
            _filePath = Path.Combine(_directoryToStore, _fileName);
            _backupFolder = Path.Combine(_directoryToStore, "Backup");
            _isBackupEnabled = isBackupEnabled;
            _cryptoService = cryptoService;
        }

        /// <summary>
        /// Saves this object to a file asynchronously
        /// </summary>
        /// <returns>A task that represents the asynchronous operation to save this object to a file</returns>
        public Task SaveAsync()
        {
            return Task.Run(() =>
            {
                if (!Directory.Exists(_directoryToStore))
                {
                    Directory.CreateDirectory(_directoryToStore);
                }

                if (_isBackupEnabled && File.Exists(_filePath))
                {
                    if (!Directory.Exists(_backupFolder))
                    {
                        Directory.CreateDirectory(_backupFolder);
                    }

                    var backupStamp = DateTime.Now;

                    var backUpPath =
                        Path.Combine(_backupFolder,
                            $"{_fileName}_{backupStamp.Day}-{backupStamp.Month}-{backupStamp.Year}-{backupStamp.Hour}-{backupStamp.Minute}-{backupStamp.Second}.bck");

                    if (File.Exists(backUpPath))
                    {
                        File.Delete(backUpPath);
                    }

                    File.Move(_filePath, backUpPath);
                }
                else if (!_isBackupEnabled && File.Exists(_filePath))
                {
                    File.Delete(_filePath);
                }

                using (var fs = new FileStream(_filePath, FileMode.CreateNew))
                {
                    using (var sw = new StreamWriter(fs))
                    {
                        sw.Write(_cryptoService.EncryptString(JsonSerializer.Serialize(this, GetType())));
                    }
                }

                OnSaveFinished();
            });
        }

        /// <summary>
        /// Fired when this object has finished saving
        /// </summary>
        public virtual void OnSaveFinished() { }

        /// <summary>
        /// Cleans up backups created using this class
        /// </summary>
        /// <param name="applicationFolder">The path to the directory for storing the initial file</param>
        /// <param name="fileName">Name of the initial file</param>
        /// <param name="clearBeforeDate">All backup files that were created before this datetime will be deleted. If null, all backup files will be deleted</param>
        /// <returns>A task that represents the asynchronous operation of cleaning up backups created using this class</returns>
        protected internal static Task ClearBackupsAsync(string applicationFolder, string fileName,
            DateTime? clearBeforeDate = null)
        {
            return Task.Run(() =>
            {
                var backupFolder = Path.Combine(applicationFolder, "Backup");

                if (Directory.Exists(backupFolder))
                {
                    foreach (var file in new DirectoryInfo(backupFolder)
                                 .GetFiles($"{fileName}*").Where(f =>
                                     clearBeforeDate == null || f.LastWriteTime < clearBeforeDate))
                    {
                        file.Delete();
                    }
                }
            });
        }

        /// <summary>
        /// Loads this object from a file
        /// </summary>
        /// <typeparam name="T">Inherited type from <see cref="ApplicationFile"/></typeparam>
        /// <param name="applicationFolder">The path to the directory for storing the file</param>
        /// <param name="fileName">Name of the file</param>
        /// <param name="cryptoService"><see cref="CoreLib.CORE.Interfaces.ICryptoService"/> implementation for file decryption</param>
        /// <param name="isBackupEnabled">A flag indicating that backup files will be created. The backup will be stored in subdirectory named "Backup". The default value is false</param>
        /// <returns>A task that represents the asynchronous operation of loading this object from a file. If there was a deserialization or decryption error, it deletes corrupted file and the return value will be null. If <paramref name="isBackupEnabled"/> is set to true, it tries to get an object from backup files</returns>
        protected internal static Task<T> LoadAsync<T>(string applicationFolder, string fileName,
            CryptoService cryptoService, bool isBackupEnabled = false) where T : ApplicationFile
        {
            return Task.Run(() =>
            {
                var filePath = Path.Combine(applicationFolder, fileName);
                var backupFolder = Path.Combine(applicationFolder, "Backup");
                T applicationFile = null;

                if (File.Exists(filePath))
                {
                    try
                    {
                        using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            using (var sr = new StreamReader(fs))
                            {
                                var jsonString = cryptoService.DecryptString(sr.ReadToEnd());

                                if (jsonString.IsNullOrEmptyOrWhiteSpace())
                                {
                                    throw new ArgumentNullException(nameof(jsonString));
                                }

                                applicationFile = JsonSerializer.Deserialize<T>(jsonString);
                            }
                        }
                    }
                    catch
                    {
                        File.Delete(filePath);

                        if (!Directory.Exists(backupFolder))
                        {
                            return null;
                        }

                        foreach (var file in new DirectoryInfo(backupFolder).GetFiles($"{fileName}*")
                                     .OrderByDescending(f => f.LastWriteTime))
                        {
                            try
                            {
                                using (var fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read,
                                           FileShare.Read))
                                {
                                    using (var sr = new StreamReader(fs))
                                    {
                                        var jsonString = cryptoService.DecryptString(sr.ReadToEnd());

                                        if (jsonString.IsNullOrEmptyOrWhiteSpace())
                                        {
                                            throw new ArgumentNullException(nameof(jsonString));
                                        }

                                        applicationFile = JsonSerializer
                                            .Deserialize<T>(jsonString);
                                    }

                                    File.Copy(file.FullName, filePath, true);

                                    break;
                                }
                            }
                            catch
                            {
                                File.Delete(file.FullName);
                            }
                        }
                    }
                }

                applicationFile?.Init(applicationFolder, fileName, cryptoService, isBackupEnabled);

                return applicationFile;
            });
        }
    }
}