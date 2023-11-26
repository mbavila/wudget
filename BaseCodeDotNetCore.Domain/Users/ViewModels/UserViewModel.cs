// <copyright file="UserViewModel.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Domain.Users.ViewModels
{
    using AutoMapper;
    using BaseCodeDotNetCore.Data.Entities;
    using BaseCodeDotNetCore.Utils.DtoMapper;

    public class UserViewModel : IMapFrom<User>, ICustomMap
    {
        public string Uname { get; set; }

        public string Password { get; set; }

        public string ConfirmPass { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int UserRoleId { get; set; }

        public string EmailAddress { get; set; }

        /// <inheritdoc/>
        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration?.CreateMap<User, UserViewModel>()
                .ForMember(uvm => uvm.Uname, opt => opt.MapFrom(e => e.Username))
                .ForMember(uvm => uvm.FirstName, opt => opt.MapFrom(e => e.FirstName))
                .ForMember(uvm => uvm.LastName, opt => opt.MapFrom(e => e.LastName))
                .ForMember(uvm => uvm.UserRoleId, opt => opt.MapFrom(e => e.UserRoleID))
                .ForMember(uvm => uvm.EmailAddress, opt => opt.MapFrom(e => e.EmailAddress));

            configuration?.CreateMap<UserViewModel, User>()
                .ForMember(uvm => uvm.Username, opt => opt.MapFrom(e => e.Uname))
                .ForMember(uvm => uvm.FirstName, opt => opt.MapFrom(e => e.FirstName))
                .ForMember(uvm => uvm.LastName, opt => opt.MapFrom(e => e.LastName))
                .ForMember(uvm => uvm.UserRoleID, opt => opt.MapFrom(e => e.UserRoleId))
                .ForMember(uvm => uvm.EmailAddress, opt => opt.MapFrom(e => e.EmailAddress));
        }
    }
}
