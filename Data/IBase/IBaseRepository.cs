using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Data.IBase
{
    public interface IBaseRepository<T>
    {
        /// <summary>
        /// Retrieve a single item using it's primary key, exception if not found
        /// </summary>
        /// <param name="primaryKey">The primary key of the record</param>
        /// <returns>T</returns>
        T Single(object primaryKey);

        /// <summary>
        /// Retrieve a single item by it's primary key or return null if not found
        /// </summary>
        /// <param name="primaryKey">Prmary key to find</param>
        /// <returns>T</returns>
        T SingleOrDefault(object primaryKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        T FirstOrDefault(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Returns all the rows for type T
        /// </summary>
        /// <returns></returns>
        IList<T> GetAll();

        IList<T> GetAllOrderByDescending(Expression<Func<T, int>> predicate, bool desc = true);

        /// <summary>
        /// Gets all entities matching the predicate
        /// </summary>
        /// <param name="predicate">The filter clause</param>
        /// <returns>All entities matching the predicate</returns>
        IList<T> GetAll(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Determines if there are any entities matching the predicate
        /// </summary>
        /// <param name="predicate">The filter clause</param>
        /// <returns>True if a match was found</returns>
        bool Any(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// return t count.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        int Count(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Does this item exist by it's primary key
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        bool Exists(object primaryKey);

        /// <summary>
        /// Inserts the data into the table
        /// </summary>
        /// <param name="entity">The entity to insert</param>
        /// <returns></returns>
        int Insert(T entity);

        /// <summary>
        /// Updates this entity in the database using it's primary key
        /// </summary>
        /// <param name="entity">The entity to update</param>
        void Update(T entity);


        /// <summary>
        /// Deletes this entry fro the database
        /// ** WARNING - Most items should be marked inactive and Updated, not deleted
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        /// <returns></returns>
        void Delete(T entity);

        IUnitOfWork UnitOfWork { get; }



    }
}
