// <copyright file="User.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Data.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class User
    {
        public string Id { get; set; }

        [Key]
        public int UserID { get; set; }

        public string Username { get; set; }

        public int UserRoleID { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string ContactNo { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime? DateUpdated { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("UserRoleID")]
        public virtual UserRole UserRole { get; set; }
    }
}
