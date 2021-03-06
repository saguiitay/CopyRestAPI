﻿using Newtonsoft.Json;

namespace CopyRestAPI.Models
{
    public class User : UserUpdate
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "storage")]
        public Storage Storage { get; set; }        

        [JsonProperty(PropertyName = "developer")]
        public string Developer { get; set; }

        [JsonProperty(PropertyName = "created_time")]
        public int CreatedTime { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "emails")]
        public UserEmail[] UserEmails { get; set; }
    }
}
