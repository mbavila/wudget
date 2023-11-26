// <copyright file="ClientService.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Domain.Clients
{
    using AutoMapper;
    using BaseCodeDotNetCore.Data.Entities;
    using BaseCodeDotNetCore.Data.Repositories;
    using BaseCodeDotNetCore.Domain.Clients.ViewModels;

    public class ClientService : IClientService
    {
        private readonly IUnitOfWork uow;
        private readonly IRepository<Client> clientRepository;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientService"/> class.
        /// </summary>
        /// <param name="unit">Instance of UnitOfWork that will be assigned to uow.</param>
        /// <param name="mapper">Instance of mapper object.</param>
        public ClientService(IUnitOfWork unit, IMapper mapper)
        {
            uow = unit;
            clientRepository = uow == null ? null : uow.GetRepository<Client>();

            this.mapper = mapper;
        }

        /// <inheritdoc/>
        public ClientViewModel GetClientById(string clientId)
        {
            ClientViewModel clientViewModel = mapper.Map<ClientViewModel>(clientRepository.SingleEntity(x => x.ClientID == clientId));
            return clientViewModel;
        }
    }
}
