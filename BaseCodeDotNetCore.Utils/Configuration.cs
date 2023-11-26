namespace BaseCodeDotNetCore.Utils
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
    }
}
