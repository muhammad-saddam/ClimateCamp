using Abp.Domain.Entities;
using Abp.Reflection.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;


namespace ClimateCamp
{
    /// <summary>
    /// Extensions methods for the DBSet class.
    /// There is an architecture pattern risk in this method. A "Read before write" can violate data integrity without being put inside a transaction control.
    /// In SQL Server, the direct alternative is to use merge statements, however merge statement is not available in EF.
    /// </summary>
    public static class DbSetExtensions
    {
        /// <summary>
        /// THe methods checks if an entity already exists in the DB using the predicate parameter to express the verification condition. 
        /// If the predicate is null it will add the entity.
        /// If the entity already exists, returns null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="entity">The entity to check for existance and to add.</param>
        /// <param name="predicate">The expression by whihc to check for existance. If null, will add the entity to the DB.</param>
        /// </summary>
        /// <remarks>
        ///     See <see href="https://aka.ms/efcore-docs-change-tracking">EF Core change tracking</see> for more information.
        /// </remarks>
        /// <param name="entity">The entity to add.</param>
        /// <returns>
        ///     The <see cref="EntityEntry{TEntity}" /> for the entity. The entry provides
        ///     access to change tracking information and operations for the entity. 
        ///     Returns null if an entity is considered to exist.
        /// </returns>
        public static EntityEntry<T> AddIfNotExists<T>(this DbSet<T> dbSet, T entity, Expression<Func<T, bool>> predicate = null) where T : class, new()
        {
            var exists = predicate != null ? dbSet.Any(predicate) : dbSet.Any();
            return !exists ? dbSet.Add(entity) : null;
        }

        /// <summary>
        /// The methods checks if an entity already exists in the DB using the predicate parameter to express the verification condition. 
        /// If the predicate is null it will check the existence by Id field.
        /// If the entity already exists, returns null.
        /// </summary>
        /// <typeparam name="T">the type of the entity, assumed implementing IEntity</typeparam>
        /// <typeparam name="TPrimaryKey">the type of the primary key of the entity</typeparam>
        /// <param name="dbSet"></param>
        /// <param name="entity">The entity to check for existance and to add.</param>
        /// <param name="predicate">The expression by whihc to check for existance. If null, check the existence by Id field. </param>
        /// </summary>
        /// <remarks>
        ///     See <see href="https://aka.ms/efcore-docs-change-tracking">EF Core change tracking</see> for more information.
        /// </remarks>
        /// <param name="entity">The entity to add.</param>
        /// <returns>
        ///     The <see cref="EntityEntry{TEntity}" /> for the entity. The entry provides
        ///     access to change tracking information and operations for the entity. 
        ///     Returns null if an entity is considered to exist.
        /// </returns>
        public static EntityEntry<T> AddIfNotExists<T, TPrimaryKey>(this DbSet<T> dbSet, T entity, Expression<Func<T, bool>> predicate = null) where T : class, IEntity<TPrimaryKey>, new() 
        {

            var exists = predicate != null ? dbSet.Any(predicate) : dbSet.Any(x => x.Id.Equals(entity.Id));
            return !exists ? dbSet.Add(entity) : null;
        }
    }

    
}
