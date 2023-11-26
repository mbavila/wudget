// <copyright file="UserRole.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Data.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class UserRole
    {
        [Key]
        public int RoleID { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string RoleName { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? DateCreated { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime? DateUpdated { get; set; }

        public bool IsActive { get; set; }
    }
}
