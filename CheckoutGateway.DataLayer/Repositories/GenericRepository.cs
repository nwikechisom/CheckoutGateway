using CheckoutGateway.DataLayer.Context;
using CheckoutGateway.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CheckoutGateway.DataLayer.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : Auditable
{
    protected readonly DatabaseContext _context;
    public GenericRepository(DatabaseContext context)
    {
        _context = context;
    }
    public void Add(T entity)
    {
        entity.Created = DateTime.Now;
        _context.Set<T>().Add(entity);
    }
    public void AddRange(IEnumerable<T> entities)
    {
        _context.Set<T>().AddRange(entities);
    }
    public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>().Where(expression);
    }
    //Overloading Method Find for Includes case
    public IEnumerable<T> Find(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _context.Set<T>();

        // Apply includes
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        // Apply filter expression
        query = query.Where(expression);

        return query.ToList();
    }
    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }
    public T GetById(int id)
    {
        return _context.Set<T>().Find(id);
    }
    public void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
    public void RemoveRange(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
    }
    public void Save()
    {
        _context.SaveChanges();
    }

    public void Update(T entity)
    {
        entity.Modified = DateTime.Now;
        _context.Update(entity);
    }
}
