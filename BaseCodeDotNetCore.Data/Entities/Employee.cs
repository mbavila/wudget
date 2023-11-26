// <copyright file="Employee.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Data.Entities
{
    using System;

    public class Employee
    {
        public int EmployeeId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string CompanyName { get; set; }

        public string Designation { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }
    }
}
