// <copyright file="EmployeeController.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.API.Controllers
{
    using System;
    using BaseCodeDotNetCore.Domain;
    using BaseCodeDotNetCore.Domain.Employees;
    using BaseCodeDotNetCore.Domain.Employees.ViewModels;
    using BaseCodeDotNetCore.Utils.Response;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// API for handling Employee Module API Method.
    /// </summary>

    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> logger;
        private readonly IEmployeeService employeeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeController"/> class.
        /// Employee controller constructor.
        /// </summary>
        /// <param name="logger">Instance of ILogger. For logging error or events.</param>
        /// <param name="employeeService">Instance of employee service. For call employee module business layer.</param>
        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            this.logger = logger;
            this.employeeService = employeeService;
        }

        /// <summary>
        /// Retrieves the list of employess from a specific page number.
        /// </summary>
        /// <param name="search">Holds the search parameter values.</param>
        /// <returns>Returns the list of employees and the pagination details.</returns>
        [HttpGet(Name = "employeelistbypage")]
        public IActionResult GetEmployeeList([FromQuery]EmployeeSearchViewModel search)
        {
            var returnList = employeeService.GetEmployeePaginated(search);
            return CreatedAtRoute("employeelistbypage", new ResponseModel(0, returnList, string.Empty));
        }

        /// <summary>
        /// Retrieve employee details given the employee id.
        /// </summary>
        /// <param name="id">Id of the Employee.</param>
        /// <returns>Returns the Employee Details.</returns>
        [Authorize(Roles = "Administrator")]
        [HttpGet("{id}", Name = "employeebyid")]
        public IActionResult GetEmployeeById(int id)
        {
            return CreatedAtRoute("employeebyid", new { data = employeeService.GetEmployeeByID(id) });
        }

        /// <summary>
        /// API for adding new Employee entry.
        /// </summary>
        /// <param name="employee">Details of the new employee.</param>
        /// <returns>Returns the id of the newly created employee. Else, return -1 when employee already exists.</returns>
        [HttpPost(Name = "create")]
        public IActionResult CreateEmployee([FromBody]EmployeeViewModel employee)
        {
            try
            {
                var id = employeeService.AddNewEmployee(employee);
                var message = id > 0 ? Messages.EmployeeMessages.EmployeeSaved : Messages.EmployeeMessages.EmployeeExist;

                return CreatedAtRoute("create", new ResponseModel(id, string.Empty, string.Format(message, employee?.EmpName)));
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format(Messages.ExceptionMessages.ExceptionGeneric, "CreateEmployee", ex.Message));
                return StatusCode(500, Messages.HttpErrorMessages.Error500);
            }
        }

        [HttpPut(Name = "update")]
        public IActionResult UpdateEmployee([FromBody]EmployeeViewModel employee)
        {
            try
            {
                var returnCode = employeeService.UpdateEmployee(employee);
                var message = string.Empty;

                if (returnCode > 0)
                {
                    message = string.Format(Messages.EmployeeMessages.EmployeeUpdated, employee?.EmpName);
                }
                else if (returnCode == 0)
                {
                    message = Messages.EmployeeMessages.EmployeeDoesNotExist;
                }
                else
                {
                    message = string.Format(Messages.EmployeeMessages.EmployeeExist, employee?.EmpName);
                }

                return CreatedAtRoute("update", new ResponseModel(returnCode, string.Empty, message));
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format(Messages.ExceptionMessages.ExceptionGeneric, "UpdateEmployee", ex.Message));
                return StatusCode(500, Messages.HttpErrorMessages.Error500);
            }
        }

        [HttpDelete("{id}", Name = "delete")]
        public IActionResult DeleteEmployee(int id)
        {
            try
            {
                var returnCode = employeeService.DeleteEmployee(id);
                var message = string.Empty;

                if (returnCode > 0)
                {
                    message = string.Format(Messages.EmployeeMessages.EmployeeDeleted, string.Empty);
                }
                else if (returnCode == 0)
                {
                    message = Messages.EmployeeMessages.EmployeeDoesNotExist;
                }
                else
                {
                    message = string.Format(Messages.EmployeeMessages.EmployeeExist, string.Empty);
                }

                return CreatedAtRoute("delete", new ResponseModel(returnCode, string.Empty, message));
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format(Messages.ExceptionMessages.ExceptionGeneric, "DeleteEmployee", ex.Message));
                return StatusCode(500, Messages.HttpErrorMessages.Error500);
            }
        }
    }
}