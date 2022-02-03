using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OnlineShop.DataAccess;
using OnlineShop.OrderArchiver;
using OnlineShop.OrderArchiver.Infrastructure;
using OnlineShop.OrderArchiver.Interfaces;
using OnlineShop.OrderArchiver.Models;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineShop.OnlineClient
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private readonly IDataArchiver _dataArchiver;

        private readonly object locker = new object();

        public Worker(ILogger<Worker> logger, IOptions<FoldersInfoModel> appOptions, DataBaseUoW uow)
        {
            _dataArchiver = CreateArchiver(appOptions.Value, uow);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _dataArchiver?.CheckObservedFolder();
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _dataArchiver?.Dispose();
            return Task.CompletedTask;
        }

        private IDataArchiver CreateArchiver(FoldersInfoModel foldersInfo, DataBaseUoW uow)
        {
            IOrderWorker orderWorker = new OrderWorker(uow, locker);
            IFileWorker fileWorker = new FileWorker(new FileWatcher(), foldersInfo);
            IFileInfoCreator fileInfoCreator = new FileInfoCreator(foldersInfo.TargetFolder);
            return new DataArchiver(orderWorker, fileWorker, fileInfoCreator);
        }
    }
}
