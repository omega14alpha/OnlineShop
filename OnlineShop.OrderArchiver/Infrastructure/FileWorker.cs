using OnlineShop.OrderArchiver.Interfaces;
using OnlineShop.OrderArchiver.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace OnlineShop.OrderArchiver.Infrastructure
{
    public class FileWorker : IFileWorker
    {
        public event EventHandler<FileSystemEventArgs> OnNewFileDetected;

        private readonly IFileWatcher _fileWatcher;

        private readonly FoldersInfoModel _foldersInfo;

        public FileWorker(IFileWatcher fileWatcher, FoldersInfoModel foldersInfo)
        {
            _foldersInfo = foldersInfo;
            _fileWatcher = fileWatcher;
            fileWatcher.OnFileDetected += InvokeOnNewFileDetected;
            fileWatcher.StartWatching(foldersInfo);
        }

        public IEnumerable<string> GetFilesPath() => 
            Directory.GetFiles(_foldersInfo.ObservedFolder, _foldersInfo.ObservedFilesPattern, SearchOption.TopDirectoryOnly);
        

        public IEnumerable<string> ReadFileData(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException($"Argument '{nameof(filePath)}' cannot be equals null.");
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File {nameof(filePath)} not found.");
            }

            using (StreamReader reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    yield return reader.ReadLine();
                }
            }
        }

        public void FileTransfer(FileNameModel fileNameModel)
        {
            if (fileNameModel == null)
            {
                throw new ArgumentNullException($"Argument '{nameof(fileNameModel)}' cannot be equals null.");
            }

            if (!File.Exists(fileNameModel.FullFilePath))
            {
                throw new FileNotFoundException($"File {nameof(fileNameModel.FullFilePath)} not found.");
            }

            CreateFolder(fileNameModel.TargetFilePath);
            File.Move(fileNameModel.FullFilePath, fileNameModel.FullTargetFilePath, true);
        }

        public void Dispose()
        {
            _fileWatcher.OnFileDetected -= InvokeOnNewFileDetected;
            _fileWatcher?.Dispose();
        }

        private void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private void InvokeOnNewFileDetected(object sender, FileSystemEventArgs e)
        {
            OnNewFileDetected?.Invoke(sender, e);
        }
    }
}
