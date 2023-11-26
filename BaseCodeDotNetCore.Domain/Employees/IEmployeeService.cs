// <copyright file="IEmployeeService.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Domain.Employees
{
    using BaseCodeDotNetCore.Data.Paging;
    using BaseCodeDotNetCore.Domain.Employees.ViewModels;

    public interface IEmployeeService
    {
        public EmployeeViewModel GetEmployeeByID(int id);

        public EmployeeViewModel GetEmployeeByName(string name);

        public int AddNewEmployee(EmployeeViewModel employee);

        public int UpdateEmployee(EmployeeViewModel employee);

        public int DeleteEmployee(int id);

        public IPaginate<EmployeeViewModel> GetEmployeePaginated(EmployeeSearchViewModel search);
    }
}
