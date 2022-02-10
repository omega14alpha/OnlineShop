using OnlineShop.DataAccess.Contexts;
using OnlineShop.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineShop.DataAccess.Repositories
{
    public class DbRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbOrderContext _context;

        public DbRepository(DbOrderContext context)
        {
            _context = context;
        }

        public int GetCount() => 
            _context.Set<TEntity>().Count();

        public int GetCountByCondition(Func<TEntity, bool> condition) =>
            _context.Set<TEntity>().Where(condition).Count();

        public TEntity GetEntityByCondition(Func<TEntity, bool> condition) =>
            _context.Set<TEntity>()?.First(condition);

        public TEntity GetOrAddEntity(TEntity entity, Func<TEntity, bool> condition) => 
            _context.Set<TEntity>().FirstOrDefault(condition) ?? _context.Set<TEntity>().Add(entity).Entity;

        public IEnumerable<TEntity> GetEntities() => _context.Set<TEntity>().ToList();

        public IEnumerable<TEntity> GetRange(int startNumber, int count) =>
            _context.Set<TEntity>().Skip(startNumber).Take(count);

        public IEnumerable<TEntity> GetRangeWithOrder<EType>(int startNumber, int count, Func<TEntity, EType> condition) =>
            _context.Set<TEntity>().OrderBy(condition).Skip(startNumber).Take(count);

        public IEnumerable<TEntity> GetRangeByCondition(int startNumber, int count, Func<TEntity, bool> condition) =>
            _context.Set<TEntity>().Where(condition).Skip(startNumber).Take(count).ToList();

        public IEnumerable<TEntity> GetRangeByConditionWithOrder<EType>(int startNumber, int count, Func<TEntity, bool> condition, Func<TEntity, EType> orderCondition) =>
            _context.Set<TEntity>().Where(condition).OrderBy(orderCondition).Skip(startNumber).Take(count).ToList();

        public void AddEntity(TEntity entity) => _context.Set<TEntity>().Add(entity);

        public void Remove(int id)
        {
            var entity = _context.Set<TEntity>().Find(id);
            if (entity != null)
            {
                _context.Remove(entity);
            }
        }

        public void RemoveRange(Guid guid)
        {
            var entities = _context.Set<TEntity>().Find(guid);
            if (entities != null)
            {
                _context.RemoveRange(entities);
            }
        }

        public void Update(TEntity entity) => 
            _context.Set<TEntity>().Update(entity);
    }
}
