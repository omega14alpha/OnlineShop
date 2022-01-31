using OnlineShop.OrderArchiver.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace OnlineShop.OrderArchiver.Interfaces
{
    public interface IFileWorker : IDisposable
    {
        event EventHandler<FileSystemEventArgs> OnNewFileDetected;

        IEnumerable<string> GetFilesPath();

        IEnumerable<string> ReadFileData(string filePath);

        void FileTransfer(FileNameModel fileNameModel);
    }
}
