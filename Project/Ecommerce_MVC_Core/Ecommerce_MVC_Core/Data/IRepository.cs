using System;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace Ecommerce_MVC_Core.Data
{
    public interface IRepository<T> where T:BaseEntity
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetListById(int id);
        T GetById(int id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        IEnumerable<T> Search(Expression<Func<T,bool>> predicate);
    }
}
