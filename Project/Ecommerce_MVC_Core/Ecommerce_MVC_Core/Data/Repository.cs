using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ecommerce_MVC_Core.Data;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace Ecommerce_MVC_Core.Data
{
    public class Repository<T>:IRepository<T> where T:BaseEntity
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> entities;
        private string ErrorMessage = string.Empty;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            entities = _context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return entities.AsEnumerable();
        }

        public IEnumerable<T> GetListById(int id)
        {
            return entities.AsEnumerable().Where(s => s.Id == id);
        }

        public T GetById(int id)
        {
            return entities.SingleOrDefault(s => s.Id == id);
        }

        public void Insert(T entity)
        {
            if (entity == null)
            {
                ErrorMessage = "No Database detected";
                throw new ArgumentNullException(nameof(entity));
                
            }
            entities.Add(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                ErrorMessage = "No Database detected";
                throw new ArgumentNullException(nameof(entity));
            }
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            entities.Remove(entity);
            _context.SaveChanges();
        }

        public IEnumerable<T> Search(Expression<Func<T, bool>> predicate)
        {
            return this.GetSet().ToList();
        }

        protected DbSet<T> GetSet()
        {
            return this._context.Set<T>();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
