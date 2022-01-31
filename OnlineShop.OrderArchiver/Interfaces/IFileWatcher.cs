using OnlineShop.OrderArchiver.Models;
using System;
using System.IO;

namespace OnlineShop.OrderArchiver.Interfaces
{
    public interface IFileWatcher : IDisposable
    {
        event EventHandler<FileSystemEventArgs> OnFileDetected;

        void StartWatching(FoldersInfoModel foldersInfo);
    }
}
