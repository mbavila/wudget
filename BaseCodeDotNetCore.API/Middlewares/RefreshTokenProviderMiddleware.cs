﻿// <copyright file="RefreshTokenProviderMiddleware.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.API.Middlewares
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web;
    using BaseCodeDotNetCore.API.Authentication;
    using BaseCodeDotNetCore.Data;
    using BaseCodeDotNetCore.Domain;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Messages = BaseCodeDotNetCore.Domain.Messages;

    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class RefreshTokenProviderMiddleware
    {
        private readonly RequestDelegate next;
        private readonly TokenProviderOptions options;
        private ClaimsProvider claimsProvider;

        /// <summary>
        ///  Initializes a new instance of the <see cref="RefreshTokenProviderMiddleware"/> class.
        /// </summary>
        /// <param name="next">Delegate representing the middleware in the request pipeline.</param>
        /// <param name="options">Object for token configurations.</param>
        public RefreshTokenProviderMiddleware(RequestDelegate next, IOptions<TokenProviderOptions> options)
        {
            this.next = next;
            this.options = options == null ? null : options.Value;
        }

        /// <summary>
        ///     Catches context path for requesting refresh token.
        /// </summary>
        /// <param name="context">Handles HTTP-specific information.</param>
        /// <param name="claimsProvider">Class for identity related methods.</param>
        /// <returns>Generates user token.</returns>
        public Task Invoke(HttpContext context, ClaimsProvider claimsProvider)
        {
            if (context != null)
            {
                if (context.Request.Path.Equals(options.RefreshTokenPath, StringComparison.Ordinal))
                {
                    this.claimsProvider = claimsProvider;
                    return GenerateToken(context);
                }
            }

            return next(context);
        }

        /// <summary>
        ///     Handles Refresh token generation.
        /// </summary>
        /// <param name="context">Handles HTTP-specific information.</param>
        /// <returns>Returns token generated by JWT.</returns>
        private async Task GenerateToken(HttpContext context)
        {
            try
            {
                var stream = context.Request.Body;
                string queryString = new StreamReader(stream).ReadToEnd();

                if (string.IsNullOrEmpty(queryString))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
                else
                {
                    JObject user = JObject.Parse(queryString);
                    /*
                    var dict = HttpUtility.ParseQueryString(queryString);
                    string[] values = null;
                    foreach (string key in dict.Keys)
                    {
                        values = dict.GetValues(key);
                        foreach (string value in values)
                        {
                            Debug.WriteLine(key + " - " + value);
                        }
                    }
                    var json = JsonConvert.SerializeObject(dict.Cast<string>().ToDictionary(k => k, v => dict[v]));
                    var user = (JObject)JsonConvert.DeserializeObject(json);

                    if (user == null)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                    else
                    {
                    */

                    var username = ((JValue)user["username"]).Value.ToString();
                    var refreshToken = ((JValue)user["refresh_token"]).Value.ToString();

                    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(refreshToken))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                    else
                    {
                        var db = context.RequestServices.GetService<BaseCodeDbContext>();
                        var userManager = context.RequestServices.GetService<UserManager<IdentityUser>>();

                        var refreshTokenModel = db.RefreshToken.SingleOrDefault(i => i.Token == refreshToken);

                        var userDb = await userManager.FindByNameAsync(username).ConfigureAwait(false);

                        var identity = await claimsProvider.GetClaimsIdentityRefresh(username, db).ConfigureAwait(false);
                        if (identity == null)
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            await context.Response.WriteAsync(Messages.LoginMessages.IncorrectCredentials).ConfigureAwait(false);
                        }
                        else
                        {
                            var id = identity.Claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")
                                    .Select(c => c.Value).SingleOrDefault();
                            if (id == null)
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            }
                            else
                            {
                                context.Response.Clear();
                                var response = AuthResponse.Execute(identity, db, userDb, refreshTokenModel);
                                if (response.AccessToken == null)
                                {
                                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                }
                                else
                                {
                                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                                    context.Response.ContentType = Constants.Common.ContentTypeJSON;
                                    await context.Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }))
                                        .ConfigureAwait(false);
                                }
                            }
                        }
                    }
                }

                if (!context.Response.HasStarted)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync(ex.Message).ConfigureAwait(false);
                if (!context.Response.HasStarted)
                {
                    return;
                }
            }
        }
    }
}
