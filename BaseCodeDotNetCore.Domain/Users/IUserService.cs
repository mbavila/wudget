// <copyright file="IUserService.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Domain.Users
{
    using System.Threading.Tasks;
    using BaseCodeDotNetCore.Data.Entities;
    using BaseCodeDotNetCore.Domain.Users.ViewModels;
    using Microsoft.AspNetCore.Identity;

    public interface IUserService
    {
        User FindByUsername(string username);

        /// <summary>
        /// Definition for retrieving User details.
        /// </summary>
        /// <param name="username">Holds the username of the user.</param>
        /// <param name="password">Holds the password of the user.</param>
        /// <returns>A <see cref="Task{User}"/> representing the result of the asynchronous operation.</returns>
        Task<User> FindUserAsync(string username, string password);

        /// <summary>
        /// Definition for adding new User to the database.
        /// </summary>
        /// <param name="userViewModel">Object that contains user details to be saved.</param>
        /// <returns>A <see cref="Task{User}"/> representing the result of the asynchronous operation.</returns>
        public Task<IdentityResult> AddNewUser(UserViewModel userViewModel);

        public UserSearchViewModel GetUserByID(int id);

        public UserViewModel GetUserByName(string username);
    }
}
