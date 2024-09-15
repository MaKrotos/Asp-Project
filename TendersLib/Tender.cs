using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TendersLib
{
    public class Tender
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("DateStart")]
        public string DateStart { get; set; }

        [JsonPropertyName("DateEnd")]
        public string DateEnd { get; set; }

        [JsonPropertyName("URL")]
        public string URL { get; set; }
    }


}
