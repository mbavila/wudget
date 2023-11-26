// <copyright file="IUnitOfWork.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Data.Repositories
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;

    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class;

        IRepositoryAsync<TEntity> GetRepositoryAsync<TEntity>()
            where TEntity : class;

        IRepositoryReadOnly<TEntity> GetReadOnlyRepository<TEntity>()
            where TEntity : class;

        int SaveChanges();

        public IDbContextTransaction CreateTransaction();
    }

    public interface IUnitOfWork<TContext> : IUnitOfWork
        where TContext : DbContext
    {
        TContext Context { get; }
    }
}