using OnlineShop.OrderArchiver.Interfaces;
using OnlineShop.OrderArchiver.Models;
using System;
using System.IO;

namespace OnlineShop.OrderArchiver.Infrastructure
{
    public class FileWatcher : IFileWatcher
    {
        public event EventHandler<FileSystemEventArgs> OnFileDetected;

        private FileSystemWatcher _watcher;

        public void StartWatching(FoldersInfoModel foldersInfo)
        {
            _watcher = new FileSystemWatcher(foldersInfo.ObservedFolder);
            _watcher.Filter = foldersInfo.ObservedFilesPattern;
            _watcher.Created += InviteOnFileDetected;
            _watcher.EnableRaisingEvents = true;
        }

        private void InviteOnFileDetected(object sender, FileSystemEventArgs e)
        {
            OnFileDetected?.Invoke(sender, e);
        }

        public void Dispose()
        {
            _watcher.Created -= InviteOnFileDetected;
        }
    }
}
