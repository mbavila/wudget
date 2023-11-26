// <copyright file="EmployeeViewModel.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Domain.Employees.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;
    using BaseCodeDotNetCore.Data.Entities;
    using BaseCodeDotNetCore.Utils.DtoMapper;

    public class EmployeeViewModel : IMapFrom<Employee>, ICustomMap
    {
        public int EmpId { get; set; }

        [Required]
        public string EmpName { get; set; }

        public string EmpAddress { get; set; }

        public string EmpCompany { get; set; }

        public string EmpPosition { get; set; }

        public DateTime EmpCreatedDate { get; set; }

        public DateTime EmpUpdateDate { get; set; }

        /// <inheritdoc/>
        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration?.CreateMap<Employee, EmployeeViewModel>()
                .ForMember(evm => evm.EmpId, opt => opt.MapFrom(e => e.EmployeeId))
                .ForMember(evm => evm.EmpName, opt => opt.MapFrom(e => e.Name))
                .ForMember(evm => evm.EmpAddress, opt => opt.MapFrom(e => e.Address))
                .ForMember(evm => evm.EmpCompany, opt => opt.MapFrom(e => e.CompanyName))
                .ForMember(evm => evm.EmpPosition, opt => opt.MapFrom(e => e.Designation))
                .ForMember(evm => evm.EmpCreatedDate, opt => opt.MapFrom(e => e.DateCreated))
                .ForMember(evm => evm.EmpUpdateDate, opt => opt.MapFrom(e => e.DateUpdated));

            configuration?.CreateMap<EmployeeViewModel, Employee>()
                .ForMember(evm => evm.EmployeeId, opt => opt.MapFrom(e => e.EmpId))
                .ForMember(evm => evm.Name, opt => opt.MapFrom(e => e.EmpName))
                .ForMember(evm => evm.Address, opt => opt.MapFrom(e => e.EmpAddress))
                .ForMember(evm => evm.CompanyName, opt => opt.MapFrom(e => e.EmpCompany))
                .ForMember(evm => evm.Designation, opt => opt.MapFrom(e => e.EmpPosition))
                .ForMember(evm => evm.DateCreated, opt => opt.MapFrom(e => e.EmpCreatedDate))
                .ForMember(evm => evm.DateUpdated, opt => opt.MapFrom(e => e.EmpUpdateDate));
        }
    }
}
