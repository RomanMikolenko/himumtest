﻿using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace HiMum.ApiGateway.Domain.Models
{
    public class JsonContent : StringContent
    {
        public JsonContent(object obj) :
            base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
        { 
        }
    }
}
