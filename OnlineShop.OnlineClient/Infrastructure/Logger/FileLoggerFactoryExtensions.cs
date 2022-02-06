using Microsoft.Extensions.Logging;

namespace OnlineShop.OnlineClient.Infrastructure.Logger
{
    public static class FileLoggerFactoryExtensions
    {
        public static ILoggerFactory AddFile(this ILoggerFactory factory, string filePath)
        {
            factory.AddProvider(new FileLoggerProvider(filePath));
            return factory;
        }
    }
}
