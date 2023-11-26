// <copyright file="SMTPSettings.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Utils.Models
{
    public class SMTPSettings
    {
        public string Host { get; set; } = Configuration.Config.GetSection("SMTPSettings:Host").Value;

        public string Port { get; set; } = Configuration.Config.GetSection("SMTPSettings:Port").Value;

        public string EnableSSL { get; set; } = Configuration.Config.GetSection("SMTPSettings:EnableSSL").Value;

        public string DefaultCredentials { get; set; } = Configuration.Config.GetSection("SMTPSettings:DefaultCredentials").Value;

        public string EmailAddress { get; set; } = Configuration.Config.GetSection("SMTPSettings:EmailAddress").Value;

        public string Password { get; set; } = Configuration.Config.GetSection("SMTPSettings:Password").Value;

        public string Name { get; set; } = Configuration.Config.GetSection("SMTPSettings:Name").Value;
    }
}
