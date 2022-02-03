using System;
using System.Collections.Generic;

namespace OnlineShop.DataAccess.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        int GetCount();

        IEnumerable<TEntity> GetRange(int startNumber, int count);

        IEnumerable<TEntity> GetRangeWithOrder<EType>(int startNumber, int count, Func<TEntity, EType> condition);

        TEntity GetEntityByCondition(Func<TEntity, bool> condition);

        TEntity GetOrAddEntity(TEntity entity, Func<TEntity, bool> condition);

        void AddEntity(TEntity entity);

        void Remove(int id);

        void RemoveRange(Guid guid);

        void Update(TEntity entity);
    }
}
