using Microsoft.Extensions.Logging;

namespace OnlineShop.OnlineClient.Infrastructure.Logger
{
    public static class FileLoggerBuilderExtensions
    {
        public static ILoggingBuilder AddFile(this ILoggingBuilder factory, string filePath)
        {
            factory.AddProvider(new FileLoggerProvider(filePath));
            return factory;
        }
    }
}
