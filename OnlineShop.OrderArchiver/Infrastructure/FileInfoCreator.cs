using OnlineShop.OrderArchiver.Interfaces;
using OnlineShop.OrderArchiver.Models;
using System;
using System.Globalization;
using System.IO;

namespace OnlineShop.OrderArchiver.Infrastructure
{
    public class FileInfoCreator : IFileInfoCreator
    {
        private const int ManagerNameIndex = 0;

        private const int DateIndex = 1;

        private readonly string _dataFormat = "ddMMyyyy";

        private readonly string _targetFolderPath;

        public FileInfoCreator(string targetFolderPath)
        {
            _targetFolderPath = targetFolderPath;
        }

        public FileNameModel CreateModel(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException($"Argument '{nameof(filePath)}' cannot be empty or equals null.");
            }

            FileInfo fileInfo = new FileInfo(filePath);
            var fileNameArr = fileInfo.Name.Split(new char[] { '_', '.' });
            var date = DateTime.ParseExact(fileNameArr[DateIndex], _dataFormat, CultureInfo.InvariantCulture);

            return new FileNameModel()
            {
                FullFileName = fileInfo.Name,
                FullFilePath = fileInfo.FullName,
                TargetFilePath = Path.Combine(_targetFolderPath, fileNameArr[ManagerNameIndex], date.Year.ToString(), date.Month.ToString()),
                FullTargetFilePath = Path.Combine(_targetFolderPath, fileNameArr[ManagerNameIndex], date.Year.ToString(), date.Month.ToString(), fileInfo.Name),
            };
        }
    }
}
