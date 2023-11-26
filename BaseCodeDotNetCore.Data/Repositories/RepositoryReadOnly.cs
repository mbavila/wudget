// <copyright file="RepositoryReadOnly.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Data.Repositories
{
    using Microsoft.EntityFrameworkCore;

    public class RepositoryReadOnly<T> : BaseRepository<T>, IRepositoryReadOnly<T>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryReadOnly{T}"/> class.
        /// </summary>
        /// <param name="context">Holds the connection to the database.</param>
        public RepositoryReadOnly(DbContext context)
            : base(context)
        {
        }
    }
}