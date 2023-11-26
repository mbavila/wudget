// <copyright file="Helper.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Utils
{
    using System.Linq;
    using System.Web;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public static class Helper
    {
        public static JObject GetRequestBody(string queryString)
        {
            var body = new JObject();

            if (queryString != null)
            {
                var dict = HttpUtility.ParseQueryString(queryString);
                var json = JsonConvert.SerializeObject(dict.Cast<string>().ToDictionary(k => k, v => dict[v]));
                body = (JObject)JsonConvert.DeserializeObject(json);
            }

            return body;
        }
    }
}
