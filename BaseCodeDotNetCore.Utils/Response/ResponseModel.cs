// <copyright file="ResponseModel.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Utils.Response
{
    public class ResponseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseModel"/> class.
        /// </summary>
        public ResponseModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseModel"/> class.
        /// </summary>
        /// <param name="statusCode">Status Code.</param>
        /// <param name="data">Data to returned.</param>
        /// <param name="message">Message returned from the API.</param>
        public ResponseModel(int statusCode, object data, string message)
        {
            ReturnCode = statusCode;
            Data = data;
            Message = message;
        }

        public int ReturnCode { get; set; }

        public object Data { get; set; }

        public string Message { get; set; }
    }
}
