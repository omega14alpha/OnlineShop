using System;
using System.Collections.Generic;

namespace OnlineShop.DataAccess.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        int GetCount();

        int GetCountByCondition(Func<TEntity, bool> condition);

        IEnumerable<TEntity> GetRange(int startNumber, int count);

        IEnumerable<TEntity> GetRangeWithOrder<EType>(int startNumber, int count, Func<TEntity, EType> condition);

        IEnumerable<TEntity> GetRangeByCondition(int startNumber, int count, Func<TEntity, bool> condition);

        TEntity GetEntityByCondition(Func<TEntity, bool> condition);

        TEntity GetOrAddEntity(TEntity entity, Func<TEntity, bool> condition);

        void AddEntity(TEntity entity);

        void Remove(int id);

        void RemoveRange(Guid guid);

        void Update(TEntity entity);
    }
}
