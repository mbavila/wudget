// <copyright file="UserService.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Domain.Users
{
    using System.Threading.Tasks;
    using AutoMapper;
    using BaseCodeDotNetCore.Data.Entities;
    using BaseCodeDotNetCore.Data.Repositories;
    using BaseCodeDotNetCore.Domain.Users.ViewModels;
    using Microsoft.AspNetCore.Identity;

    public class UserService : IUserService
    {
        private readonly IUnitOfWork uow;
        private readonly IRepository<User> userRepository;
        private readonly IMapper mapper;
        private UserManager<IdentityUser> userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="unit">Holds the instance of Unit of Work.</param>
        /// <param name="mapper">Mapper instance.</param>
        /// <param name="userManager">Instance of UserManager class.</param>
        public UserService(IUnitOfWork unit, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            uow = unit;
            userRepository = uow?.GetRepository<User>();

            this.mapper = mapper;
            this.userManager = userManager;
        }

        /// <inheritdoc/>
        public User FindByUsername(string username)
        {
            return userRepository.SingleEntity(x => x.Username == username);
        }

        /// <inheritdoc/>
        public async Task<User> FindUserAsync(string userName, string password)
        {
            var user = userRepository.SingleEntity(x => x.Username == userName);
            var identity = await userManager.FindByNameAsync(userName).ConfigureAwait(false);
            var isPasswordOK = await userManager.CheckPasswordAsync(identity, password).ConfigureAwait(false);

            if ((identity == null) || (isPasswordOK == false))
            {
                user = null;
            }

            return user;
        }

        /// <inheritdoc/>
        public async Task<IdentityResult> AddNewUser(UserViewModel userViewModel)
        {
            var identity = new IdentityUser
            {
                UserName = userViewModel?.Uname,
            };

            var result = await userManager.CreateAsync(identity, userViewModel.ConfirmPass).ConfigureAwait(false);

            if (result.Succeeded)
            {
                var role = (userViewModel.UserRoleId == 1 /*Constants.User.AdminId*/) ? /*Constants.User.AdministratorRole*/ "ADMINISTRATOR" : /*Constants.User.UserRole*/ "USER";
                result = await userManager.AddToRoleAsync(identity, role).ConfigureAwait(false);

                var user = mapper.Map<User>(userViewModel);
                user.Id = identity.Id;

                userRepository.Add(user);
                uow.SaveChanges();
            }

            return result;
        }

        /// <inheritdoc/>
        public UserViewModel GetUserByName(string username)
        {
            return mapper.Map<UserViewModel>(userRepository.SingleEntity(c => c.Username.Equals(username)));
        }

        /// <inheritdoc/>
        public UserSearchViewModel GetUserByID(int id)
        {
            UserSearchViewModel userViewModel = mapper.Map<UserSearchViewModel>(userRepository.GetByID(id));
            return userViewModel;
        }
    }
}
