using System.Collections.Generic;
using Newtonsoft.Json;

namespace Client.Scripts.Service.Model
{
    public class LocusPointData
    {
        [JsonProperty("position", Required = Required.Always)]
        public Position Position;

        [JsonProperty("configuration", Required = Required.Always)]
        public List<PartData> Configuration;
    }
}