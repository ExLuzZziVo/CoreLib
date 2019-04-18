#region

using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using CoreLib.CORE.Helpers.CryptoHelpers;

#endregion

namespace CoreLib.STANDALONE.CustomObjects
{
    [DataContract]
    [Serializable]
    public abstract class ApplicationFile : INotifyPropertyChanged
    {
        [field: NonSerialized] private static string _applicationFolder;
        [field: NonSerialized] private static string _backupFolder;
        [field: NonSerialized] private static string _fileName;
        [field: NonSerialized] private static string _filePath;
        [field: NonSerialized] private static bool _isBackupEnabled;
        [field: NonSerialized] private static CryptoService _cryptoService;

        protected ApplicationFile(string applicationFolder, string fileName, CryptoService cryptoService,
            bool isBackupEnabled = false)
        {
            Init(applicationFolder, fileName, cryptoService, isBackupEnabled);
        }

        private static void Init(string applicationFolder, string fileName, CryptoService cryptoService,
            bool isBackupEnabled = false)
        {
            _applicationFolder = applicationFolder;
            _fileName = fileName;
            _filePath = Path.Combine(_applicationFolder, _fileName);
            _backupFolder = Path.Combine(_applicationFolder, _fileName, "Backup");
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

            using (var fs = new FileStream(_filePath, FileMode.CreateNew))
            {
                _cryptoService.EncryptObject(fs, this,
                    new DataContractJsonSerializerSettings {EmitTypeInformation = EmitTypeInformation.Never});
            }

            OnSaveFinished();
        }

        public virtual void OnSaveFinished()
        {
        }

        public static void ClearBackups(string applicationFolder, string fileName,
            DateTime clearBeforeDate = new DateTime())
        {
            Init(applicationFolder, fileName, null);
            if (Directory.Exists(_backupFolder))
                foreach (var file in new DirectoryInfo(_backupFolder)
                    .GetFiles($"{fileName}*").Where(f =>
                        clearBeforeDate == new DateTime() || f.LastWriteTime < clearBeforeDate))
                    file.Delete();
        }

        public static ApplicationFile Load(string applicationFolder, string fileName, CryptoService cryptoService,
            bool isBackupEnabled = false)
        {
            Init(applicationFolder, fileName, cryptoService, isBackupEnabled);
            if (File.Exists(_filePath))
                try
                {
                    using (var fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        return _cryptoService.DecryptObject<ApplicationFile>(fs,
                            new DataContractJsonSerializerSettings {EmitTypeInformation = EmitTypeInformation.Never});
                    }
                }
                catch
                {
                    File.Delete(_filePath);
                    if (_isBackupEnabled)
                    {
                        if (!Directory.Exists(_backupFolder))
                            return null;

                        foreach (var file in new DirectoryInfo(_backupFolder).GetFiles($"{_fileName}*")
                            .OrderByDescending(f => f.LastWriteTime))
                            try
                            {
                                using (var fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read,
                                    FileShare.Read))
                                {
                                    var objectToReturn = _cryptoService.DecryptObject<ApplicationFile>(fs,
                                        new DataContractJsonSerializerSettings
                                            {EmitTypeInformation = EmitTypeInformation.Never});
                                    using (var wfs = new FileStream(_filePath, FileMode.CreateNew))
                                    {
                                        _cryptoService.DecryptObject<ApplicationFile>(wfs,
                                            new DataContractJsonSerializerSettings
                                                {EmitTypeInformation = EmitTypeInformation.Never});
                                    }

                                    return objectToReturn;
                                }
                            }
                            catch
                            {
                                //
                            }
                    }

                    return null;
                }

            return null;
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