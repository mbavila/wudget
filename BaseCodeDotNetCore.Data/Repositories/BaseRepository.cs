// <copyright file="BaseRepository.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Data.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using BaseCodeDotNetCore.Data.Paging;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Query;

    public abstract class BaseRepository<T> : IReadRepository<T>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{T}"/> class.
        /// </summary>
        /// <param name="context">Database Context.</param>`65 43
        public BaseRepository(DbContext context)
        {
            DbContext = context ?? throw new ArgumentException(Messages.ExceptionMessages.DBContextNull);
            DbSet = DbContext.Set<T>();
        }

        public DbContext DbContext { get; }

        protected DbSet<T> DbSet { get; }

        /// <inheritdoc/>
        public virtual IQueryable<T> Query(string sql, params object[] parameters) => DbSet.FromSqlRaw(sql, parameters);

        /// <inheritdoc/>
        public T Search(params object[] keyValues)
        {
            return DbSet.Find(keyValues);
        }

        public BaseCodeDbContext GetDbContext()
        {
            return (BaseCodeDbContext)DbContext;
        }

        /// <inheritdoc/>
        public T SingleEntity(
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disableTracking = true)
        {
            IQueryable<T> query = DbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).FirstOrDefault();
            }

            return query.FirstOrDefault();
        }

        /// <inheritdoc/>
        public IPaginate<T> GetList(
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int index = 0,
            int size = 20,
            bool disableTracking = true)
        {
            IQueryable<T> query = DbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return orderBy != null ? orderBy(query).ToPaginate(index, size) : query.ToPaginate(index, size);
        }

        /// <inheritdoc/>
        public IPaginate<TResult> GetList<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int index = 0,
            int size = 20,
            bool disableTracking = true)
            where TResult : class
        {
            IQueryable<T> query = DbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return orderBy != null
                ? orderBy(query).Select(selector).ToPaginate(index, size)
                : query.Select(selector).ToPaginate(index, size);
        }

        public void Dispose()
        {
            DbContext?.Dispose();
        }
    }
}