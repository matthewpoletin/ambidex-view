using System;
using Newtonsoft.Json;

namespace Client.Scripts.Service.Model
{
    public class Design
    {
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("description", Required = Required.Always)]
        public string Description { get; set; }

        [JsonProperty("author", Required = Required.Always)]
        public string Author { get; set; }

        [JsonProperty("createDate", Required = Required.Always)]
        public DateTime CreateDate { get; set; }

        [JsonProperty("robotConfiguration", Required = Required.Always)]
        public RobotConfiguration RobotConfiguration { get; set; }

        [JsonProperty("waypointPath", Required = Required.Always)]
        public WaypointsConfiguration WaypointsConfiguration { get; set; }
    }
}