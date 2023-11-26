// <copyright file="AuthResponse.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.API.Authentication
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using BaseCodeDotNetCore.Data;
    using BaseCodeDotNetCore.Data.Entities;
    using BaseCodeDotNetCore.Domain;
    using BaseCodeDotNetCore.Domain.Users.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.IdentityModel.Logging;
    using Microsoft.IdentityModel.Tokens;

    public static class AuthResponse
    {
        /// <summary>
        ///     Retrieves token options from appconfig.json.
        /// </summary>
        /// <returns>Returns an instance of TokenProviderOptions.</returns>
        public static TokenProviderOptions GetOptions()
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.Config.GetSection(Constants.Config.SecretKey).Value));

            return new TokenProviderOptions
            {
                Path = Configuration.Config.GetSection(Constants.Config.TokenPath).Value,
                Audience = Configuration.Config.GetSection(Constants.Config.Audience).Value,
                Issuer = Configuration.Config.GetSection(Constants.Config.Issuer).Value,
                Expiration = TimeSpan.FromMinutes(Convert.ToInt32(Configuration.Config.GetSection(Constants.Config.ExpirationMinutes).Value)),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
            };
        }

        /// <summary>
        ///     Handles token generation for access and refresh tokens.
        /// </summary>
        /// <param name="identity">Holds the details of the logged in user.</param>
        /// <param name="db">Holds the connection to the database.</param>
        /// <param name="userName">Holds the value of the username.</param>
        /// <param name="refreshToken">Hohlds the refreshtoken of the current user session.</param>
        /// <returns>Returns the validated login details.</returns>
        public static LoginViewModel Execute(ClaimsIdentity identity, BaseCodeDbContext db, IdentityUser userName, RefreshToken refreshToken = null)
        {
            LoginViewModel response = new LoginViewModel();

            try
            {
                var options = GetOptions();
                var now = DateTime.UtcNow;

                if (refreshToken == null)
                {
                    refreshToken = new RefreshToken()
                    {
                        Username = userName?.UserName,
                        Token = Guid.NewGuid().ToString("N"),
                    };

                    db?.InsertNew(refreshToken);
                }

                refreshToken.IssuedUtc = now;
                refreshToken.ExpiresUtc = now.Add(options.Expiration);
                db?.SaveChanges();

                var jwt = new JwtSecurityToken(
                    issuer: options.Issuer,
                    audience: options.Audience,
                    claims: identity?.Claims,
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.Add(options.Expiration),
                    signingCredentials: options.SigningCredentials);

                IdentityModelEventSource.ShowPII = true;
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                response = new LoginViewModel
                {
                    AccessToken = encodedJwt,
                    RefreshToken = refreshToken.Token,
                    ExpiresIn = (int)options.Expiration.TotalSeconds,
                };
            }
            catch (Exception)
            {
                throw;
            }

            return response;
        }
    }
}
