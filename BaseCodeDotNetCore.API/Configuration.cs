// <copyright file="Configuration.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.API
{
    using System.IO;
    using Microsoft.Extensions.Configuration;

    public static class Configuration
    {
        static Configuration()
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json");
            Config = builder.Build();

            Config = Config;
        }

        public static IConfigurationRoot Config { get; set; }

        /// <summary>
        /// Gets and sets the DBConnection.
        /// </summary>
        public static string DbConnection => Config["myconn"];
    }
}
