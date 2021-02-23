using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace The_Game_Client
{
    class Settings
    {
        [JsonPropertyName("baseAddress")]
        public string BaseAddress { get; set; }
    }
}
