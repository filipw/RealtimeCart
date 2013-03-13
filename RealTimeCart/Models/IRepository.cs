using System.Linq;

namespace RealTimeCart.Models
{
    public interface IRepository<T>
        where T : Entity
    {
        T Add(T entity);
        T Delete(int id);
        T Get(int id);
        T Update(T entity);
        IQueryable<T> Items { get; }
    }
}