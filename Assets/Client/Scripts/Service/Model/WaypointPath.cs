using System.Collections.Generic;
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

    public class WaypointData
    {
        [JsonProperty("position", Required = Required.Always)]
        public Position Position { get; set; }
    }

    public class WaypointPath
    {
        [JsonProperty("waypoints", Required = Required.Always)]
        public List<WaypointData> Waypoints { get; set; }
    }
}