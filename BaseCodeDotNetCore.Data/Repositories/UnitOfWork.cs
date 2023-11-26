// <copyright file="UnitOfWork.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;

    public class UnitOfWork<TContext> : IRepositoryFactory, IUnitOfWork<TContext>, IUnitOfWork
        where TContext : DbContext, IDisposable
    {
        private Dictionary<Type, object> repositories;
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork{TContext}"/> class.
        /// </summary>
        /// <param name="context">Holds the connection to the database.</param>
        public UnitOfWork(TContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc/>
        public TContext Context { get; }

        /// <inheritdoc/>
        public IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }

            var type = typeof(TEntity);
            if (!repositories.ContainsKey(type))
            {
                repositories[type] = new Repository<TEntity>(Context);
            }

            return (IRepository<TEntity>)repositories[type];
        }

        /// <inheritdoc/>
        public IRepositoryAsync<TEntity> GetRepositoryAsync<TEntity>()
            where TEntity : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }

            var type = typeof(TEntity);
            if (!repositories.ContainsKey(type))
            {
                repositories[type] = new RepositoryAsync<TEntity>(Context);
            }

            return (IRepositoryAsync<TEntity>)repositories[type];
        }

        /// <inheritdoc/>
        public IRepositoryReadOnly<TEntity> GetReadOnlyRepository<TEntity>()
            where TEntity : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }

            var type = typeof(TEntity);
            if (!repositories.ContainsKey(type))
            {
                repositories[type] = new RepositoryReadOnly<TEntity>(Context);
            }

            return (IRepositoryReadOnly<TEntity>)repositories[type];
        }

        /// <inheritdoc/>
        public IDbContextTransaction CreateTransaction()
        {
            return Context.Database.BeginTransaction();
        }

        /// <inheritdoc/>
        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }

            disposed = true;
        }
    }
}