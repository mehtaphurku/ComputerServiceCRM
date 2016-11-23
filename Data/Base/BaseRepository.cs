using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Data.IBase;

namespace Data.Base
{
    /// <summary>
    /// Base class for all SQL based service classes
    /// </summary>
    /// <typeparam name="T">The domain object type</typeparam>
    public class BaseRepository<T> : IBaseRepository<T>
        where T : class
    {
        private readonly IUnitOfWork _unitOfWork;
        internal DbSet<T> DbSet;
        public BaseRepository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
            _unitOfWork = unitOfWork;
            this.DbSet = _unitOfWork.Db.Set<T>();
        }

        /// <summary>
        /// Returns the object with the primary key specifies or throws
        /// </summary>
        /// <typeparam name="TU">The type to map the result to</typeparam>
        /// <param name="primaryKey">The primary key</param>
        /// <returns>The result mapped to the specified type</returns>
        public T Single(object primaryKey)
        {
            var dbResult = DbSet.Find(primaryKey);
            return dbResult;
        }

        /// <summary>
        /// Returns the object with the primary key specifies or the default for the type
        /// </summary>
        /// <param name="primaryKey">The primary key</param>
        /// <returns>The result mapped to the specified type</returns>
        public T SingleOrDefault(object primaryKey)
        {
            var dbResult = DbSet.Find(primaryKey);
            return dbResult;
        }

        /// <summary>
        /// Returns the object with the primary key specifies or the default for the type
        /// </summary>
        /// <param name="expression">The primary key</param>
        /// <returns>The result mapped to the specified type</returns>
        public T FirstOrDefault(Expression<Func<T, bool>> expression)
        {
            var dbResult = DbSet.FirstOrDefault(expression);
            return dbResult;
        }

        //public virtual IEnumerable<T> GetAllWithDeleted()
        //{
        //    var dbresult = _unitOfWork.Db.Fetch<T>("");

        //    return dbresult;
        //}

        public bool Exists(object primaryKey)
        {
            return DbSet.Find(primaryKey) == null ? false : true;
        }

        public virtual int Insert(T entity)
        {
            dynamic obj = DbSet.Add(entity);
            this._unitOfWork.Db.SaveChanges();
            return obj.Id;

        }

        public virtual T InsertObj(T entity)
        {
            var obj = DbSet.Add(entity);
            this._unitOfWork.Db.SaveChanges();
            return obj;

        }

        public virtual void Update(T entity)
        {
            DbSet.Attach(entity);
            _unitOfWork.Db.Entry(entity).State = EntityState.Modified;
            this._unitOfWork.Db.SaveChanges();
        }
        public void Delete(T entity)
        {
            if (_unitOfWork.Db.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            dynamic obj = DbSet.Remove(entity);
            this._unitOfWork.Db.SaveChanges();
        }
        public IUnitOfWork UnitOfWork { get { return _unitOfWork; } }
        internal DbContext Database { get { return _unitOfWork.Db; } }

        

        public IList<T> GetAll()
        {
            return DbSet.AsEnumerable().ToList();
        }

        public IList<T> GetAllOrderByDescending(Expression<Func<T, int>> predicate, bool desc = true)
        {
            return desc ? DbSet.OrderByDescending(predicate).ToList() : DbSet.OrderBy(predicate).ToList();
        }

        /// <summary>
        /// Gets all entities matching the predicate
        /// </summary>
        /// <param name="predicate">The filter clause</param>
        /// <returns>All entities matching the predicate</returns>
        public IList<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return this.DbSet.Where(predicate).ToList();
        }

        /// <summary>
        /// Determines if there are any entities matching the predicate
        /// </summary>
        /// <param name="predicate">The filter clause</param>
        /// <returns>True if a match was found</returns>
        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return this.DbSet.Any(predicate);
        }

        /// <summary>
        /// Determines if there are any entities matching the predicate
        /// </summary>
        /// <param name="predicate">The filter clause</param>
        /// <returns>True if a match was found</returns>
        public int Count(Expression<Func<T, bool>> predicate)
        {
            return this.DbSet.Count(predicate);
        }
    }
}
