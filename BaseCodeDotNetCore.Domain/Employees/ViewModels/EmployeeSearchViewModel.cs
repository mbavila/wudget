// <copyright file="EmployeeSearchViewModel.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Domain.Employees.ViewModels
{
    public class EmployeeSearchViewModel
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string Company { get; set; }

        public string Position { get; set; }

        public PaginationViewModel Pagination { get; set; }
    }
}
