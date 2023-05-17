using CheckoutGateway.DataLayer.Models;
using System.Linq.Expressions;

namespace CheckoutGateway.DataLayer.Repositories
{
    public interface IGenericRepository<T> where T : Auditable
    {
        void Add(T entity);
        void Update(T entity);
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        IEnumerable<T> Find(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);
        void Save();
    }
}