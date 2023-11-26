// <copyright file="IRepositoryAsync.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using BaseCodeDotNetCore.Data.Paging;
    using Microsoft.EntityFrameworkCore.Query;

    public interface IRepositoryAsync<T>
        where T : class
    {
        /// <summary>
        /// Get a single entry from the database.
        /// </summary>
        /// <param name="predicate">Where clause conditions.</param>
        /// <param name="orderBy">Order By conditions.</param>
        /// <param name="include">Joined tables.</param>
        /// <param name="disableTracking">Turn on or off tracking.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<T> SingleAsync(
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disableTracking = true);

        IEnumerable<T> GetAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Get paged list of entries.
        /// </summary>
        /// <param name="predicate">Where clause conditions.</param>
        /// <param name="orderBy">Order By conditions.</param>
        /// <param name="include">Joined tables.</param>
        /// <param name="index">Page number to be retrieved.</param>
        /// <param name="size">Number of records per page.</param>
        /// <param name="disableTracking">Turn on or off tracking.</param>
        /// <param name="cancellationToken">Notification that the operation will be cancelled.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<IPaginate<T>> GetListAsync(
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int index = 0,
            int size = 20,
            bool disableTracking = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronous Add operation for new database entries.
        /// </summary>
        /// <param name="entity">List of entities that will be inserted to the database.</param>
        /// <param name="cancellationToken">Notification that the operation will be cancelled.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<T>> AddAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronous Add operation for new database entries.
        /// </summary>
        /// <param name="entities">List of entities that will be inserted to the database.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task AddAsync(params T[] entities);

        /// <summary>
        /// Asynchronous Add operation for new database entries.
        /// </summary>
        /// <param name="entities">List of entities that will be inserted to the database.</param>
        /// <param name="cancellationToken">Notification that the operation will be cancelled.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task AddAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        void UpdateAsync(T entity);
    }
}