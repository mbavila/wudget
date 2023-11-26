// <copyright file="ClaimsProvider.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.API.Authentication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using BaseCodeDotNetCore.Data;
    using BaseCodeDotNetCore.Data.Entities;
    using BaseCodeDotNetCore.Domain.Users;

    public class ClaimsProvider
    {
        private readonly IUserService userService;

        /// <summary>
        ///  Initializes a new instance of the <see cref="ClaimsProvider"/> class.
        /// </summary>
        /// <param name="userService">Instance of user service. For call user module business layer.</param>
        public ClaimsProvider(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        ///     Adds claims to claimsIdentity.
        /// </summary>
        /// <param name="user">User login details.</param>
        /// <param name="db">Connection to the database.</param>
        /// <returns>Returns an instances of ClaimsIdentify after user details are validated.</returns>
        public static ClaimsIdentity CreateClaimsIdentity(User user, BaseCodeDbContext db)
        {
            var claims = new List<Claim>();

            try
            {
                if (user == null)
                {
                    claims = null;
                }
                else
                {
                    var userRoles = db?.UserRoles.Where(i => i.UserId == user.Id);
                    foreach (var u in userRoles)
                    {
                        var role = db.Roles.Single(i => i.Id == u.RoleId);
                        claims.Add(new Claim(ClaimTypes.Role, role.Name));
                    }

                    claims.Add(new Claim(ClaimTypes.Name, user.Id));
                }
            }
            catch (Exception)
            {
                claims = null;
            }

            return new ClaimsIdentity(claims);
        }

        /// <summary>
        ///     Used to retrieve claimsIdentity for access token generation.
        /// </summary>
        /// <param name="username">Holds the username of the user.</param>
        /// <param name="password">Holds the password of the user.</param>
        /// <param name="db">Holds the DB context of the API.</param>
        /// <returns>Returns the identity details of the user.</returns>
        public async Task<ClaimsIdentity> GetClaimsIdentityAsync(string username, string password, BaseCodeDbContext db)
        {
            ClaimsIdentity claimsIdentity = null;

            try
            {
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && (db != null))
                {
                    var user = await userService.FindUserAsync(username, password).ConfigureAwait(false);
                    if (user != null)
                    {
                        claimsIdentity = CreateClaimsIdentity(user, db);
                    }
                }
            }
            catch (Exception)
            {
                claimsIdentity = null;
            }

            return await Task.FromResult(claimsIdentity).ConfigureAwait(false);
        }

        /// <summary>
        ///      Used to retreive claimsIdentity for refresh token generation.
        /// </summary>
        /// <param name="username">Holds the username of the user.</param>
        /// <param name="db">Holds the DB context of the API.</param>
        /// <returns>Returns the identity details of the user.</returns>
        public async Task<ClaimsIdentity> GetClaimsIdentityRefresh(string username, BaseCodeDbContext db)
        {
            ClaimsIdentity claimsIdentity = null;

            try
            {
                if (!string.IsNullOrEmpty(username) && db != null)
                {
                    var user = userService.FindByUsername(username);
                    if (user == null)
                    {
                        claimsIdentity = null;
                    }
                    else
                    {
                        claimsIdentity = CreateClaimsIdentity(user, db);
                    }
                }
            }
            catch (Exception)
            {
                claimsIdentity = null;
            }

            return await Task.FromResult(claimsIdentity).ConfigureAwait(false);
        }
    }
}
