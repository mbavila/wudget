// <copyright file="BaseCodeDbContext.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Data
{
    using System;
    using System.Data.Entity;
    using System.Diagnostics;
    using System.Linq;
    using BaseCodeDotNetCore.Data.Entities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using MySql.Data.MySqlClient;

    [DbConfigurationType(typeof(MySqlConfiguration))]
    public class BaseCodeDbContext : IdentityDbContext<IdentityUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCodeDbContext"/> class.
        /// </summary>
        /// <param name="options">Database Context Options.</param>
        public BaseCodeDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual Microsoft.EntityFrameworkCore.DbSet<Employee> Employee { get; set; }

        public virtual Microsoft.EntityFrameworkCore.DbSet<RefreshToken> RefreshToken { get; set; }

        public virtual Microsoft.EntityFrameworkCore.DbSet<User> User { get; set; }

        public virtual Microsoft.EntityFrameworkCore.DbSet<UserRole> UserRole { get; set; }

        public virtual Microsoft.EntityFrameworkCore.DbSet<Category> Category { get; set; }

        public virtual Microsoft.EntityFrameworkCore.DbSet<SubCategory> SubCategory { get; set; }

        public virtual Microsoft.EntityFrameworkCore.DbSet<Budget> Budget { get; set; }

        public virtual Microsoft.EntityFrameworkCore.DbSet<Transaction> Transaction { get; set; }
        public void InsertNew(RefreshToken token)
        {
            try
            {
                var tokenModel = RefreshToken.SingleOrDefault(i => i.Username == token.Username);
                if (tokenModel != null)
                {
                    RefreshToken.Remove(tokenModel);
                    SaveChanges();
                }

                RefreshToken.Add(token);
                SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                 .Property(e => e.DateCreated)
                 .HasDefaultValue(DateTime.Now);

            ModelBuilderExtensions.Seed(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
    }
}
