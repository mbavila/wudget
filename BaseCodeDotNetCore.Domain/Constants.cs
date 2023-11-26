// <copyright file="Constants.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Domain
{
    public static class Constants
    {
        public static class Config
        {
            public const string SecretKey = "AppSettings:SecretKey";
            public const string TokenPath = "AppSettings:TokenPath";
            public const string Audience = "AppSettings:Audience";
            public const string Issuer = "AppSettings:Issuer";
            public const string ExpirationMinutes = "AppSettings:ExpirationMinutes";
        }

        public static class Swagger
        {
            public const string Title = "My API";
            public const string Version = "v1";
            public const string URL = "/swagger/v1/swagger.json";
            public const string Name = "Base Code API V1";
            public const string SecurityDefinitionDescription = "Enter the request header in the following box to add Jwt To grant authorization Token: Bearer Token";
            public const string SecurityDefinitionName = "Authorization";
            public const string SecurityDefinitionBearerFormat = "JWT";
            public const string SecurityDefinitionScheme = "Bearer";
        }

        public static class Common
        {
            public const string ContentTypeJSON = "application/json";
            public const string TokenExpired = "Token-Expired";
            public const string Subject = "Hello World!";
            public const string Body = "<h1>Hello World!</h1>";
            public const char CommaChar = ',';
            public const string ForwardSlash = "/";
        }

        public static class User
        {
            public const string AdministratorRole = "Administrator";
            public const string UserRole = "User";

            public const int AdministratorId = 1;
        }

        public static class Generic
        {
            public const string TrueString = "true";
            public const string FalseString = "false";
        }

        public static class TFSIntegration
        {
            // @todo: add modified date checking
            public const string SelectQuery = "SELECT * FROM WorkItems WHERE([Work Item Type] = 'Product Backlog Item' OR[Work Item Type] = 'Task' OR[Work Item Type] = 'Bug' ) AND [Billing Status] NOT IN ('1') AND [Billing Status] NOT IN ('') ";
            public const string ModifiedDatePredicate = " AND [System.ChangedDate] >= '{0}'";
            public const string DateTimeString = "yyyy-MM-dd";

            public const string TFSAccessToken = "TFS:AccessToken";

            public const string CredentialsFormat = "{0}:{1}";
            public const string JSONType = "application/json";
            public const string BasicAuthentication = "Basic";
            public const string POST = "POST";
            public const string API = "URL:API";

            public const string GetAsyncURL = "_apis/wit/workitems?ids=";
            public const string ExpandRelations = "&$expand=Relations&api-version=4.0";

            public const string TaskString = "Task";
            public const string BacklogString = "Product Backlog Item";

            public const string ParentString = "System.LinkTypes.Hierarchy-Reverse";

            public const string WIQLURL = "_apis/wit/wiql?api-version=4.0";

            public const string NPRMainCategory = "Non-Project Related";
            public const int InternalProductivityNPRCategoryId = 5;
            public const int InternalProductivityNPRPhaseId = 7;

            // Hangfire Jobs
            public const string NoonSync = "12NNTFSSync";
            public const string EODSync = "8PMTFSSync";
        }
    }
}
