using OnlineShop.OrderArchiver.Models;

namespace OnlineShop.OrderArchiver.Interfaces
{
    public interface IFileInfoCreator
    {
        FileNameModel CreateModel(string filePath);
    }
}
