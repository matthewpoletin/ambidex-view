using Newtonsoft.Json;

namespace Client.Scripts.Service.Model
{
    public class Position
    {
        [JsonProperty("x", Required = Required.Always)]
        public float X { get; set; }

        [JsonProperty("y", Required = Required.Always)]
        public float Y { get; set; }

        [JsonProperty("z", Required = Required.Always)]
        public float Z { get; set; }
    }
}