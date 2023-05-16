using CheckoutGateway.DataLayer.Models;
using System.Linq.Expressions;

namespace CheckoutGateway.DataLayer.Repositories
{
    public interface IGenericRepository<T> where T : BaseModel
    {
        void Add(T entity);
        void Update(T entity);
        void AddRange(IEnumerable<T> entities);
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void Save();
    }
}