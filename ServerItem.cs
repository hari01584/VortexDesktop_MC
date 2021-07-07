using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vortex_Desktop
{
    public class ServerItem
    {
        [JsonProperty("server_name")]
        public string Sname { get; set; }

        [JsonProperty("host_id")]
        public string host { get; set; }

        [JsonProperty("host_secret")]
        public string HostSecret { get; set; }

        [JsonProperty("host_extip")]
        public string HostExtip { get; set; }

        [JsonProperty("host_port")]
        public string HostPort { get; set; }

        [JsonProperty("isPassword")]
        public string IsPassword { get; set; }

        public string player { get; set; }
        public string isOnline { get; set; }

        public ServerItem(string v1, string v2, string v3, string v4)
        {
            this.Sname = v1;
            this.host = v2;
            this.player = v3;
            this.isOnline = v4;
        }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 

    public class ServerResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public List<ServerItem> Data { get; set; }
    }


}
