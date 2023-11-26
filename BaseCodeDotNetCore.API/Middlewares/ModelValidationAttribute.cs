// <copyright file="ModelValidationAttribute.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.API.Middlewares
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class ModelValidationAttribute : ActionFilterAttribute
    {
        /// <inheritdoc/>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context != null && !context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState); // returns 400 with error
            }
        }
    }
}
