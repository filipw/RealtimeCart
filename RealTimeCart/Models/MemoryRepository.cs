using System;
using System.Collections.Concurrent;
using System.Linq;

namespace RealTimeCart.Models
{
    public class MemoryRepository<T> : IRepository<T> where T : Entity
    {
        public static ConcurrentDictionary<int, T> Repo = new ConcurrentDictionary<int, T>();

        public IQueryable<T> Items
        {
            get { return Repo.Values.AsQueryable(); }
        }

        public T Get(int id)
        {
            if (!Repo.ContainsKey(id))
            {
                return null;
            }

            T entity;
            var result = Repo.TryGetValue(id, out entity);
            return !result ? null : entity;
        }

        public T Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            var id = Repo.Count > 0 ? Repo.Last().Key : 0;
            id++;
            entity.Id = id;

            var result = Repo.TryAdd(id, entity);
            return result == false ? null : entity;
        }

        public T Delete(int id)
        {
            if (!Repo.ContainsKey(id))
            {
                return null;
            }

            T removed;
            var result = Repo.TryRemove(id, out removed);
            return !result ? null : removed;
        }

        public T Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            if (!Repo.ContainsKey(entity.Id))
            {
                return null;
            }

            Repo[entity.Id] = entity;
            return entity;
        }
    }
}