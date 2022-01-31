using OnlineShop.OrderArchiver.Interfaces;
using OnlineShop.OrderArchiver.Models;
using System;
using System.IO;
using System.Threading;

namespace OnlineShop.OrderArchiver
{
    public class DataArchiver : IDataArchiver
    {
        private const int ManagerNameIndex = 0;

        private readonly IFileWorker _fileWorker;

        private readonly IOrderWorker _orderWorker;

        private readonly IFileInfoCreator _fileInfoCreator;

        public DataArchiver(IOrderWorker orderWorker, IFileWorker fileWorker, IFileInfoCreator fileInfoCreator)
        {
            _orderWorker = orderWorker;
            _fileInfoCreator = fileInfoCreator;
            _fileWorker = fileWorker;
            _fileWorker.OnNewFileDetected += FileDetected;
        }

        public void CheckObservedFolder()
        {
            var files = _fileWorker.GetFilesPath();
            foreach (var file in files)
            {
                var task = new Thread(Process);
                task.Start(file);
            }
        }

        private void FileDetected(object sender, FileSystemEventArgs e)
        {
            var task = new Thread(Process);
            task.Start(e.FullPath);
        }

        private void Process(object path)
        {
            var filePath = path as string;
            Guid sessionGuid = Guid.NewGuid();
            var fileInfo = GetFileInfo(filePath);
            var result = SaveData(sessionGuid, fileInfo);
            if (result)
            {
                _fileWorker.FileTransfer(fileInfo);
            }
            else
            {
                _orderWorker.DataRollback(sessionGuid);
            }
        }

        private FileNameModel GetFileInfo(string filePath)
        {
            return _fileInfoCreator.CreateModel(filePath);
        }

        private bool SaveData(Guid sessionGuid, FileNameModel fileNameModel)
        {
            string managerName = fileNameModel.FullFileName.Split('_')[ManagerNameIndex];
            foreach (var data in _fileWorker.ReadFileData(fileNameModel.FullFilePath))
            {
                try
                {
                    _orderWorker.SaveOrder(sessionGuid, $"{managerName};{data}");
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return true;
        }

        public void Dispose()
        {
            _fileWorker.OnNewFileDetected -= FileDetected;
            _fileWorker?.Dispose();
        }
    }
}
