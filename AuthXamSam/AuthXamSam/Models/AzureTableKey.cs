using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace AuthXamSam.Models
{
    public class AzureTableKey
    {
        [JsonProperty("PartitionKey", Required = Required.Always)]
        public string PartitionKey { get; set; }

        [JsonProperty("RowKey", Required = Required.Always)]
        public string RowKey { get; set; }

        [JsonProperty("Timestamp")]
        public DateTimeOffset TimeStamp { get; set; }
    }
}
