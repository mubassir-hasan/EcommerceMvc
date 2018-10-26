using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ecommerce_MVC_Core.Repository
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// 
        /// Query like normal ef query 
        /// 
        /// <code>Query()</code>
        ///</summary>
        IQueryable<T> Query();


        /// <summary>
        /// 
        /// Query like normal ef query 
        /// 

        ///</summary>
        ICollection<T> PaggedList(int? pageSize, int? page, params Expression<Func<T, object>>[] navigationProperties);


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T}"/> class.
        /// </summary>
        Task<ICollection<T>> PaggedListAsync(int? pageSize, int? page, params Expression<Func<T, object>>[] navigationProperties);

        ICollection<T> GetAll();
        Task<ICollection<T>> GetAllAsync();

        T GetById(int? id);
        /// <summary>
        /// return object table data by it's id
        /// </summary>
        Task<T> GetByIdAsync(int? id);

        T Find(Expression<Func<T, bool>> match);
        Task<T> FindAsync(Expression<Func<T, bool>> match);

        ICollection<T> FindAll(Expression<Func<T, bool>> match);
        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match);

        T Insert(T entity);
        Task<T> InsertAsync(T entity);

        void Update(T updated);
        Task<T> UpdateAsync(T updated);

        void Delete(T t);
        Task<int> DeleteAsync(T t);

        int Count();
        Task<int> CountAsync();

        IEnumerable<T> Filter(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "",
            int? page = null,
            int? pageSize = null);

        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);

        bool Exist(Expression<Func<T, bool>> predicate);

        IList<T> GetAllInclude(params Expression<Func<T, object>>[] navigationProperties);
        Task<IList<T>> GetAllIncludeAsync(params Expression<Func<T, object>>[] navigationProperties);

        IList<T> GetIncludeList(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties);

        T GetSingleInclude(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties);
        Task<T> GetSingleIncludeAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties);
    }
}
