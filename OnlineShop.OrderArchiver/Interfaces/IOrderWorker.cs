using System;

namespace OnlineShop.OrderArchiver.Interfaces
{
    public interface IOrderWorker
    {
        void SaveOrder(Guid sessionGuid, string data);

        void DataRollback(Guid sessionGuid);
    }
}
