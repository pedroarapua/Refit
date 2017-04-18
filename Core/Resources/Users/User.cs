using System;
using Newtonsoft.Json;

namespace Core
{
    public class User
    {
        public User()
        {
        }

        [JsonProperty("avatar")]
        public String Avatar { get; set; }
        [JsonProperty("id")]
        public String Id { get; set; }
    }
}
