// <copyright file="IRepositoryReadOnly.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Data.Repositories
{
    public interface IRepositoryReadOnly<T> : IReadRepository<T>
        where T : class
    {
    }
}