using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OnlineShop.DataAccess.Contexts;
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

        private readonly DataArchiver _dataArchiver;

        private readonly object locker = new object();

        public Worker(ILogger<Worker> logger, IOptions<FoldersInfoModel> appOptions, DbContextOptions<DbOrderContext> context)
        {
            var foldersInfo = appOptions.Value;
            IOrderWorker orderWorker = new OrderWorker(new DbOrderContext(context), locker);
            IFileWorker fileWorker = new FileWorker(new FileWatcher(), foldersInfo);
            IFileInfoCreator fileInfoCreator = new FileInfoCreator(foldersInfo.TargetFolder);
            _dataArchiver = new DataArchiver(orderWorker, fileWorker, fileInfoCreator);
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
    }
}
