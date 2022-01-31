using System;

namespace OnlineShop.OrderArchiver.Interfaces
{
    public interface IDataArchiver : IDisposable
    {
        void CheckObservedFolder();
    }
}
