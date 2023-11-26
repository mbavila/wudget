// <copyright file="Client.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Data.Entities
{
    using System;

    public class Client
    {
        public string ClientID { get; set; }

        public string Secret { get; set; }

        public string Name { get; set; }

        public bool? ApplicationType { get; set; }

        public bool? Active { get; set; }

        public int? RefreshTokenLifeTime { get; set; }

        public string AllowedOrigin { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
