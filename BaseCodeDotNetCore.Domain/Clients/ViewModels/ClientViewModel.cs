// <copyright file="ClientViewModel.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Domain.Clients.ViewModels
{
    using AutoMapper;
    using BaseCodeDotNetCore.Data.Entities;
    using BaseCodeDotNetCore.Utils.DtoMapper;

    public class ClientViewModel : IMapFrom<Client>, ICustomMap
    {
        public string Id { get; set; }

        public string ClientSecret { get; set; }

        public string ClientName { get; set; }

        public bool? ClientApplicationType { get; set; }

        public bool? IsActive { get; set; }

        public int? ClientRefreshTokenLifeTime { get; set; }

        public string ClientAllowedOrigin { get; set; }

        /// <inheritdoc/>
        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration?.CreateMap<Client, ClientViewModel>()
                .ForMember(evm => evm.Id, opt => opt.MapFrom(e => e.ClientID))
                .ForMember(evm => evm.ClientSecret, opt => opt.MapFrom(e => e.Secret))
                .ForMember(evm => evm.ClientName, opt => opt.MapFrom(e => e.Name))
                .ForMember(evm => evm.ClientApplicationType, opt => opt.MapFrom(e => e.ApplicationType))
                .ForMember(evm => evm.IsActive, opt => opt.MapFrom(e => e.Active))
                .ForMember(evm => evm.ClientRefreshTokenLifeTime, opt => opt.MapFrom(e => e.RefreshTokenLifeTime))
                .ForMember(evm => evm.ClientAllowedOrigin, opt => opt.MapFrom(e => e.AllowedOrigin));

            configuration?.CreateMap<ClientViewModel, Client>()
                .ForMember(evm => evm.ClientID, opt => opt.MapFrom(e => e.Id))
                .ForMember(evm => evm.Secret, opt => opt.MapFrom(e => e.ClientSecret))
                .ForMember(evm => evm.Name, opt => opt.MapFrom(e => e.ClientName))
                .ForMember(evm => evm.ApplicationType, opt => opt.MapFrom(e => e.ClientApplicationType))
                .ForMember(evm => evm.Active, opt => opt.MapFrom(e => e.IsActive))
                .ForMember(evm => evm.RefreshTokenLifeTime, opt => opt.MapFrom(e => e.ClientRefreshTokenLifeTime))
                .ForMember(evm => evm.AllowedOrigin, opt => opt.MapFrom(e => e.ClientAllowedOrigin));
        }
    }
}
