using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MG.Settings.Json.Tests
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class Customer
    {
        [JsonProperty("customerContactInfo")]
        public string ContactInfo { get; set; }

        [JsonProperty("customerId")]
        public Guid Id { get; set; }

        [JsonProperty("customerName")]
        public string Name { get; set; }
    }
}
