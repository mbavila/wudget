// <copyright file="ICustomMap.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Utils.DtoMapper
{
    using AutoMapper;

    public interface ICustomMap
    {
        public void CreateMappings(IMapperConfigurationExpression configuration);
    }
}
