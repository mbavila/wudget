// <copyright file="IRepositoryFactory.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Data.Repositories
{
    public interface IRepositoryFactory
    {
        IRepository<T> GetRepository<T>()
            where T : class;

        IRepositoryAsync<T> GetRepositoryAsync<T>()
            where T : class;

        IRepositoryReadOnly<T> GetReadOnlyRepository<T>()
            where T : class;

        void Dispose();
    }
}