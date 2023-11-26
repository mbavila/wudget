// <copyright file="LoginViewModel.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Domain.Users.ViewModels
{
    public class LoginViewModel
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public int ExpiresIn { get; set; }

        public bool IsAdmin { get; set; }
    }
}
