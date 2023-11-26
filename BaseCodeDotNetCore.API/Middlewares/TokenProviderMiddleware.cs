// <copyright file="TokenProviderMiddleware.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.API.Middlewares
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using BaseCodeDotNetCore.API.Authentication;
    using BaseCodeDotNetCore.Data;
    using BaseCodeDotNetCore.Domain;
    using BaseCodeDotNetCore.Domain.Clients;
    using BaseCodeDotNetCore.Domain.Clients.ViewModels;
    using BaseCodeDotNetCore.Utils;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Messages = BaseCodeDotNetCore.Domain.Messages;

    public class TokenProviderMiddleware
    {
        private readonly RequestDelegate next;
        private readonly TokenProviderOptions options;
        private ClaimsProvider claimsProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenProviderMiddleware"/> class.
        /// </summary>
        /// <param name="next">Delegate representing the middleware in the request pipeline.</param>
        /// <param name="options">Object for token configurations.</param>
        public TokenProviderMiddleware(RequestDelegate next, IOptions<TokenProviderOptions> options)
        {
            this.next = next;
            this.options = options?.Value;
        }

        /// <summary>
        ///     Catches context path for logging in.
        /// </summary>
        /// <param name="context">Handles HTTP-specific information.</param>
        /// <param name="claimsProvider">Class for identity related methods.</param>
        /// /// <param name="clientService">Injection for ClientService.</param>
        /// <returns>Generates user token.</returns>
        public async Task Invoke(HttpContext context, ClaimsProvider claimsProvider, IClientService clientService)
        {
            try
            {
                if (context != null)
                {
                    if (context.Request.Path.Equals(options.Path, StringComparison.Ordinal))
                    {
                        var stream = context.Request.Body;
                        string queryString = new StreamReader(stream).ReadToEnd();

                        if (string.IsNullOrEmpty(queryString))
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            await context.Response.WriteAsync(Messages.LoginMessages.InvalidRequestDetails).ConfigureAwait(false);
                        }
                        else
                        {
                            JObject body = JObject.Parse(queryString);
                            string clientId = ((JValue)body["client_id"]).Value.ToString();
                            ClientViewModel clientViewModel = clientService?.GetClientById(clientId);

                            if (clientViewModel == null)
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                await context.Response.WriteAsync(Messages.LoginMessages.InvalidClientId).ConfigureAwait(false);
                            }
                            else
                            {
                                var username = ((JValue)body["username"]).Value.ToString();
                                var password = ((JValue)body["password"]).Value.ToString();

                                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                                {
                                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                    await context.Response.WriteAsync(Messages.LoginMessages.InvalidRequestDetails).ConfigureAwait(false);
                                }
                                else
                                {
                                    this.claimsProvider = claimsProvider;
                                    await GenerateToken(context, username, password).ConfigureAwait(false);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync(ex.Message).ConfigureAwait(false);
                if(!context.Response.HasStarted)
                {
                    await next(context).ConfigureAwait(false);
                }
            }
            finally
            {
                if (!context.Response.HasStarted)
                {
                    await next(context).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        ///     Handles Access token generation.
        /// </summary>
        /// <param name="context">Handles HTTP-specific information.</param>
        /// <param name="username">Handles username of the requestor.</param>
        /// <param name="password">Handles password of the requestor.</param>
        /// <returns>Returns token generated by JWT.</returns>
        private async Task GenerateToken(HttpContext context, string username, string password)
        {
            try
            {
                var db = context.RequestServices.GetService<BaseCodeDbContext>();
                var identity = await claimsProvider.GetClaimsIdentityAsync(username, password, db).ConfigureAwait(false);
                if (identity == null)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsync(Messages.LoginMessages.IncorrectCredentials).ConfigureAwait(false);
                    if (!context.Response.HasStarted)
                    {
                        return;
                    }
                }
                else
                {
                    var userManager = context.RequestServices.GetService<UserManager<IdentityUser>>();
                    var id = identity.Claims.Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Select(c => c.Value).SingleOrDefault();

                    if (id == null)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                    else
                    {
                        var userDb = await userManager.FindByNameAsync(username).ConfigureAwait(false);
                        var response = AuthResponse.Execute(identity, db, userDb);

                        if (response.AccessToken == null)
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        }
                        else
                        {
                            context.Response.ContentType = Constants.Common.ContentTypeJSON;
                            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented })).ConfigureAwait(false);
                        }
                    }
                }

                return;
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync(ex.Message).ConfigureAwait(false);
                throw;
            }
        }
    }
}
