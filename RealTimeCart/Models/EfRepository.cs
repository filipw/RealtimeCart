using System;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace RealTimeCart.Models
{
    public class EfRepository<T> : IRepository<T> where T : Entity
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public EfRepository(DbContext dbContext)
        {
            _dbContext = dbContext;

            if (dbContext == null)
            {

                throw new ArgumentNullException("dbContext");
            }

            _dbSet = dbContext.Set<T>();
        }

        public T Add(T entity)
        {
            _dbSet.Add(entity);
            _dbContext.SaveChanges();
            return entity;
        }

        public T Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity == null) return null;

            _dbSet.Remove(entity);
            _dbContext.SaveChanges();

            return entity;

        }

        public T Get(int id)
        {
            return _dbSet.Find(id);
        }

        public T Update(T entity)
        {
            if (entity == null) return null;

            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return entity;
        }

        public IQueryable<T> Items
        {
            get { return _dbSet; }
        }

        public class BookStoreContext : DbContext
        {
            public DbSet<Book> Books { get; set; }
            public DbSet<Order> Orders { get; set; }
        }
    }
}