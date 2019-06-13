using System.Collections.Generic;
using Newtonsoft.Json;

namespace Client.Scripts.Service.Model
{
    public class WaypointData
    {
        [JsonProperty("position", Required = Required.Always)]
        public Position Position { get; set; }
    }

    public class WaypointsConfiguration
    {
        [JsonProperty("waypoints", Required = Required.Always)]
        public List<WaypointData> Waypoints { get; set; }
    }
}