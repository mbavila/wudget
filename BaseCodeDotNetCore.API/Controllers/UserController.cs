// <copyright file="UserController.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.API.Controllers
{
    using System;
    using BaseCodeDotNetCore.Domain.Users;
    using BaseCodeDotNetCore.Domain.Users.ViewModels;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> logger;
        private readonly IUserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="logger">Instance of ILogger. For logging error or events.</param>
        /// <param name="userService">Instance of user service. For call user module business layer.</param>
        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            this.logger = logger;
            this.userService = userService;
        }

        /// <summary>
        /// Retrieve user details given the user id.
        /// </summary>
        /// <param name="id">Id of the User.</param>
        /// <returns>Returns the User Details.</returns>
        [Authorize(Roles = "Administrator")]
        [HttpGet("{UserID}", Name = "userbyid")]
        public IActionResult GetUserById(int id)
        {
            return CreatedAtRoute("userbyid", new { data = userService.GetUserByID(id) });
        }

        /// <summary>
        /// API for adding new User entry.
        /// </summary>
        /// <param name="user">Details of the new user.</param>
        /// <returns>Returns the details of the newly created user.</returns>
        [HttpPost(Name = "createUser")]
        public IActionResult CreateUser([FromBody]UserViewModel user)
        {
            try
            {
                var result = userService.AddNewUser(user);

                return CreatedAtRoute("create", new { data = result });
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside the CreateUser action: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
