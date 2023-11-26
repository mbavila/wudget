// <copyright file="ModelBuilderExtensions.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Data
{
    using BaseCodeDotNetCore.Data.Entities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public static class ModelBuilderExtensions
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            SeedRoles(modelBuilder ?? null);
            SeedUserRoles(modelBuilder);
            SeedASPnetUsers(modelBuilder);
            SeedEmployees(modelBuilder);
            SeedClients(modelBuilder);
            SeedCategories(modelBuilder);
            SeedSubCategories(modelBuilder);
        }

        private static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
              new IdentityRole { Name = "Administrator", NormalizedName = "ADMINISTRATOR" },
              new IdentityRole { Name = "User", NormalizedName = "USER" });
        }

        private static void SeedUserRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>().HasData(
               new UserRole { RoleID = 1, RoleName = "Admin", CreatedBy = 1, DateCreated = System.DateTime.Now, UpdatedBy = 1, DateUpdated = System.DateTime.Now, IsActive = true },
               new UserRole { RoleID = 2, RoleName = "User", CreatedBy = 1, DateCreated = System.DateTime.Now, UpdatedBy = 1, DateUpdated = System.DateTime.Now, IsActive = true });
        }

        private static void SeedASPnetUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUser>().HasData(
               new IdentityUser { Id = "U001", UserName = "User001", NormalizedUserName = "USER001", Email = "user1@gmail.com", NormalizedEmail = "user1@GMAIL.COM", EmailConfirmed = true, PasswordHash = "longtext", SecurityStamp = "longtext", ConcurrencyStamp = "longtext", PhoneNumber = "+639987556824", PhoneNumberConfirmed = true, TwoFactorEnabled = true, LockoutEnd = System.DateTimeOffset.Now, LockoutEnabled = false, AccessFailedCount = 0 },
               new IdentityUser { Id = "U002", UserName = "User002", NormalizedUserName = "USER002", Email = "user2@gmail.com", NormalizedEmail = "user2@GMAIL.COM", EmailConfirmed = true, PasswordHash = "ASzrmQDi4KfHhH4", SecurityStamp = "p8IZmrCVt0pNghD", ConcurrencyStamp = "lIs8sIcoeI5FvAU", PhoneNumber = "+639987556824", PhoneNumberConfirmed = true, TwoFactorEnabled = true, LockoutEnd = System.DateTimeOffset.Now, LockoutEnabled = false, AccessFailedCount = 0 });
        }

        private static void SeedCategories(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
               new Category { CategoryId = 1, Name = "Income", CreatedDate = System.DateTime.Now, ModifiedDate = System.DateTime.MaxValue },
               new Category { CategoryId = 2, Name = "Expense", CreatedDate = System.DateTime.Now, ModifiedDate = System.DateTime.MaxValue });
        }

        private static void SeedSubCategories(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubCategory>().HasData(
               new SubCategory { SubCategoryId = 1, CategoryID = 1, Name = "1st Half Salary", CreatedDate = System.DateTime.Now, ModifiedDate = System.DateTime.MaxValue },
               new SubCategory { SubCategoryId = 2, CategoryID = 1, Name = "2nd Half Salary", CreatedDate = System.DateTime.Now, ModifiedDate = System.DateTime.MaxValue },
               new SubCategory { SubCategoryId = 3, CategoryID = 2, Name = "Utility Expense", CreatedDate = System.DateTime.Now, ModifiedDate = System.DateTime.MaxValue },
               new SubCategory { SubCategoryId = 4, CategoryID = 2, Name = "Grocery Expense", CreatedDate = System.DateTime.Now, ModifiedDate = System.DateTime.MaxValue });
        }

        private static void SeedEmployees(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
               new Employee { EmployeeId = 1, Name = "John Doe", Address = "Cebu City", CompanyName = "Riot", Designation = "Developer", DateCreated = System.DateTime.Now, DateUpdated = System.DateTime.MaxValue },
               new Employee { EmployeeId = 2, Name = "Michael Doe", Address = "Carcar", CompanyName = "Ubisoft", Designation = "Janitor", DateCreated = System.DateTime.Now, DateUpdated = System.DateTime.MaxValue },
               new Employee { EmployeeId = 3, Name = "Jane Doe", Address = "Davao City", CompanyName = "Activision", Designation = "Marketing Manager", DateCreated = System.DateTime.Now, DateUpdated = System.DateTime.MaxValue });
        }

        private static void SeedClients(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().HasData(
               new Client { ClientID = "C001", Secret = "Secret", Name = "Charlie Brown", ApplicationType = true, Active = true, RefreshTokenLifeTime = 1000, AllowedOrigin = "Allowed Origin", CreatedBy = "Someone", CreatedDate = System.DateTime.Now, ModifiedBy = "Someone Else", ModifiedDate = System.DateTime.Now },
               new Client { ClientID = "C002", Secret = "Secret", Name = "Millie Brown", ApplicationType = true, Active = true, RefreshTokenLifeTime = 1000, AllowedOrigin = "Allowed Origin", CreatedBy = "Someone", CreatedDate = System.DateTime.Now, ModifiedBy = "Someone Else", ModifiedDate = System.DateTime.Now },
               new Client { ClientID = "C003", Secret = "Secret", Name = "Bobby Brown", ApplicationType = true, Active = true, RefreshTokenLifeTime = 1000, AllowedOrigin = "Allowed Origin", CreatedBy = "Someone", CreatedDate = System.DateTime.Now, ModifiedBy = "Someone Else", ModifiedDate = System.DateTime.Now });
        }

        
    }
}
