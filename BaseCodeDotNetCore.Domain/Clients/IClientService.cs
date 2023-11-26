// <copyright file="IClientService.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Domain.Clients
{
    using BaseCodeDotNetCore.Domain.Clients.ViewModels;

    public interface IClientService
    {
        ClientViewModel GetClientById(string clientId);
    }
}
