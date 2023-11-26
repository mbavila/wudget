// <copyright file="EmployeeService.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Domain.Employees
{
    using System;
    using System.Linq;
    using AutoMapper;
    using BaseCodeDotNetCore.Data.Entities;
    using BaseCodeDotNetCore.Data.Paging;
    using BaseCodeDotNetCore.Data.Repositories;
    using BaseCodeDotNetCore.Domain.Employees.ViewModels;

    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork uow;
        private readonly IRepository<Employee> employeeRepository;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeService"/> class.
        /// </summary>
        /// <param name="unit">Instance of UnitOfWork that will be assigned to _uow.</param>
        /// <param name="mapper">Instance of mapper object.</param>
        public EmployeeService(IUnitOfWork unit, IMapper mapper)
        {
            uow = unit;
            employeeRepository = uow == null ? null : uow.GetRepository<Employee>();

            this.mapper = mapper;
        }

        /// <inheritdoc/>
        public EmployeeViewModel GetEmployeeByID(int id)
        {
            EmployeeViewModel empViewModel = mapper.Map<EmployeeViewModel>(employeeRepository.GetByID(id));
            return empViewModel;
        }

        /// <inheritdoc/>
        public EmployeeViewModel GetEmployeeByName(string name)
        {
            return mapper.Map<EmployeeViewModel>(employeeRepository.SingleEntity(e => e.Name.Equals(name)));
        }

        /// <summary>
        /// Service method for adding new employee entry to the database.
        /// </summary>
        /// <param name="employee">Employee object that would be inserted in the database.</param>
        /// <returns> Returns the id of the employee after successful saving. Else if the employee already exists based on its name, return -1. </returns>
        public int AddNewEmployee(EmployeeViewModel employee)
        {
            int returnValue = 0;

            if (employeeRepository.SingleEntity(e => e.Name.Equals(employee.EmpName)) == null)
            {
                Employee entity = mapper.Map<Employee>(employee);
                entity.DateCreated = DateTime.Now;
                entity.DateUpdated = null;

                employeeRepository.Add(entity);
                uow.SaveChanges();

                returnValue = entity.EmployeeId;
            }
            else
            {
                returnValue = -1;
            }

            return returnValue;
        }

        /// <summary>
        /// Service method for editing and existsing employee entry from the database.
        /// </summary>
        /// <param name="employee">Employee view model that holds the details of the record to be updated.</param>
        /// <returns>
        ///     Returns the id of the employee after successful update.
        ///     Return -1 if the employee already exists.
        ///     Return 0 if the employee to be edited does not exist.
        /// </returns>
        public int UpdateEmployee(EmployeeViewModel employee)
        {
            int returnValue = 0;

            Employee dbEmployee = employeeRepository.SingleEntity(e => e.EmployeeId == employee.EmpId);

            if (dbEmployee != null)
            {
                Employee dbExisting = employeeRepository.SingleEntity(
                    e => e.EmployeeId != employee.EmpId &&
                    e.Name.Equals(employee.EmpName));

                if (dbExisting == null)
                {
                    dbEmployee = mapper.Map<Employee>(employee);
                    dbEmployee.DateUpdated = DateTime.Now;

                    employeeRepository.Update(dbEmployee);
                    uow.SaveChanges();

                    returnValue = dbEmployee.EmployeeId;
                }
                else
                {
                    returnValue = -1;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Delete the employee record with ID equal to the value of parameter id.
        /// </summary>
        /// <param name="id">ID of the employee to be deleted.</param>
        /// <returns>
        ///     Returns the id of the employee after successful delete.
        ///     Return -1 if the employee already exists.
        ///     Return 0 if the employee to be deleted does not exist. </returns>
        public int DeleteEmployee(int id)
        {
            int returnValue = 0;

            Employee dbEmployee = employeeRepository.SingleEntity(e => e.EmployeeId == id);

            if (dbEmployee != null)
            {
                employeeRepository.Delete(id);
                uow.SaveChanges();

                returnValue = 1;
            }

            return returnValue;
        }

        /// <inheritdoc/>
        public IPaginate<EmployeeViewModel> GetEmployeePaginated(EmployeeSearchViewModel search)
        {
            IPaginate<EmployeeViewModel> employess = null;

            if (search != null)
            {
                    employess = employeeRepository.GetList(
                    selector: source => mapper.Map<EmployeeViewModel>(source),
                    predicate: source => (string.IsNullOrEmpty(search.Name) || source.Name.Contains(search.Name)) &&
                                         (string.IsNullOrEmpty(search.Address) || source.Address.Contains(search.Address)) &&
                                         (string.IsNullOrEmpty(search.Company) || source.CompanyName.Contains(search.Company)) &&
                                         (string.IsNullOrEmpty(search.Position) || source.Name.Contains(search.Position)),
                    orderBy: source => source.OrderByDescending(x => x.Name),
                    index: search.Pagination == null ? 0 : search.Pagination.Page,
                    size: search.Pagination == null ? 10 : search.Pagination.PageSize);
            }

            return employess;
        }
    }
}
