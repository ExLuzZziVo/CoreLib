#region

using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using CoreLib.CORE.Helpers.CryptoHelpers;
using CoreLib.CORE.Helpers.StringHelpers;
using Newtonsoft.Json;

#endregion

namespace CoreLib.STANDALONE.CustomObjects
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class ApplicationFile : INotifyPropertyChanged
    {
        [field: NonSerialized] private string _applicationFolder;
        [field: NonSerialized] private string _backupFolder;
        [field: NonSerialized] private CryptoService _cryptoService;
        [field: NonSerialized] private string _fileName;
        [field: NonSerialized] private string _filePath;
        [field: NonSerialized] private bool _isBackupEnabled;
        [field:NonSerialized] private static readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings{ ObjectCreationHandling = ObjectCreationHandling.Replace, TypeNameHandling = TypeNameHandling.Auto };
        protected ApplicationFile(string applicationFolder, string fileName, CryptoService cryptoService,
            bool isBackupEnabled = false)
        {
            Init(applicationFolder, fileName, cryptoService, isBackupEnabled);
        }

        protected void Init(string applicationFolder, string fileName, CryptoService cryptoService,
            bool isBackupEnabled = false)
        {
            _applicationFolder = applicationFolder;
            _fileName = fileName;
            _filePath = Path.Combine(_applicationFolder, _fileName);
            _backupFolder = Path.Combine(_applicationFolder, "Backup");
            _isBackupEnabled = isBackupEnabled;
            _cryptoService = cryptoService;
        }

        public void Save()
        {
            if (!Directory.Exists(_applicationFolder))
                Directory.CreateDirectory(_applicationFolder);
            if (_isBackupEnabled && File.Exists(_filePath))
            {
                if (!Directory.Exists(_backupFolder))
                    Directory.CreateDirectory(_backupFolder);
                var backupStamp = DateTime.Now;
                var backUpPath =
                    Path.Combine(_backupFolder,
                        $"{_fileName}_{backupStamp.Day}-{backupStamp.Month}-{backupStamp.Year}-{backupStamp.Hour}-{backupStamp.Minute}-{backupStamp.Second}.bck");
                if (File.Exists(backUpPath))
                    File.Delete(backUpPath);
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
                    sw.Write(_cryptoService.EncryptString(JsonConvert.SerializeObject(this, _jsonSerializerSettings)));
                }
            }

            OnSaveFinished();
        }

        public virtual void OnSaveFinished()
        {
        }

        protected internal static void ClearBackups(string applicationFolder, string fileName,
            DateTime clearBeforeDate = new DateTime())
        {
            var backupFolder = Path.Combine(applicationFolder, "Backup");
            if (Directory.Exists(backupFolder))
                foreach (var file in new DirectoryInfo(backupFolder)
                    .GetFiles($"{fileName}*").Where(f =>
                        clearBeforeDate == new DateTime() || f.LastWriteTime < clearBeforeDate))
                    file.Delete();
        }

        protected internal static T Load<T>(string applicationFolder, string fileName,
            CryptoService cryptoService, bool isBackupEnabled = false) where T : ApplicationFile
        {
            var filePath = Path.Combine(applicationFolder, fileName);
            var backupFolder = Path.Combine(applicationFolder, "Backup");
            T applicationFile = null;
            if (File.Exists(filePath))
                try
                {
                    using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (var sr = new StreamReader(fs))
                        {
                            var jsonString =cryptoService.DecryptString(sr.ReadToEnd());
                            if (jsonString.IsNullOrEmptyOrWhiteSpace())
                                throw new ArgumentNullException(nameof(jsonString));
                            applicationFile =
                                JsonConvert.DeserializeObject<T>(jsonString, _jsonSerializerSettings);
                        }
                    }
                }
                catch
                {
                    File.Delete(filePath);
                    if (!Directory.Exists(backupFolder))
                        return null;
                    foreach (var file in new DirectoryInfo(backupFolder).GetFiles($"{fileName}*")
                        .OrderByDescending(f => f.LastWriteTime))
                        try
                        {
                            using (var fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read,
                                FileShare.Read))
                            {
                                using (var sr = new StreamReader(fs))
                                {
                                    var jsonString = cryptoService.DecryptString(sr.ReadToEnd());
                                    if (jsonString.IsNullOrEmptyOrWhiteSpace())
                                        throw new ArgumentNullException(nameof(jsonString));
                                    applicationFile =
                                        JsonConvert.DeserializeObject<T>(jsonString, _jsonSerializerSettings);
                                }

                                File.Copy(file.FullName, filePath, true);
                                //using (var wfs = new FileStream(filePath, FileMode.CreateNew))
                                //{
                                //    using (var sw = new StreamWriter(wfs))
                                //    {
                                //        sw.Write(cryptoService.EncryptString(JsonConvert.SerializeObject(applicationFile)));
                                //    }
                                //}
                                break;
                            }
                        }
                        catch
                        {
                            File.Delete(file.FullName);
                        }
                }

            applicationFile?.Init(applicationFolder, fileName, cryptoService, isBackupEnabled);
            return applicationFile;
        }

        #region Implement INotifyPropertyChanged

        [field: NonSerialized] public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}