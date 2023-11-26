// <copyright file="IRepository.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using BaseCodeDotNetCore.Data.Entities;
    using Microsoft.EntityFrameworkCore.Query;

    public interface IRepository<T> : IReadRepository<T>, IDisposable
        where T : class
    {
        T GetByID(int id);

        T Add(T entity);

        void Add(params T[] entities);

        void Add(IEnumerable<T> entities);

        void Delete(T entity);

        void Delete(object id);

        void Delete(params T[] entities);

        void Delete(IEnumerable<T> entities);

        public List<T> MultipleEntities(
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disableTracking = true);

        void Update(T entity);

        void Update(params T[] entities);

        void Update(IEnumerable<T> entities);
    }
}