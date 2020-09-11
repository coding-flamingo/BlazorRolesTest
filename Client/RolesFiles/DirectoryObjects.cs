using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RolesTest.Client.RolesFiles
{
    public class DirectoryObjects
    {
        [JsonPropertyName("@odata.context")]
        public string Context { get; set; }

        [JsonPropertyName("value")]
        public List<Value> Values { get; set; }
    }

    public class Value
    {
        [JsonPropertyName("@odata.type")]
        public string Type { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public class CustomUserAccount : RemoteUserAccount
    {
        [JsonPropertyName("groups")]
        public string[] Groups { get; set; } = new string[] { };

        [JsonPropertyName("roles")]
        public string[] Roles { get; set; } = new string[] { };
    }

}
