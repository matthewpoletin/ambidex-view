using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Client.Scripts.Service.Model
{
    public class PartData
    {
        [JsonProperty("id", Required = Required.Always)]
        public Guid Id { get; set; }

        [JsonProperty("type", Required = Required.Always)]
        public string Type { get; set; }

        [JsonProperty("length", Required = Required.Always)]
        public float Length { get; set; }

        [JsonProperty("rotationY", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public float RotationY { get; set; }

        [JsonProperty("minAngle", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public float MinAngle { get; set; }

        [JsonProperty("maxAngle", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public float MaxAngle { get; set; }

        [JsonProperty("initialAngle", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public float InitialAngle { get; set; }

        [JsonProperty("diameter", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public float Diameter { get; set; }
    }

    public class RobotConfiguration
    {
        [JsonProperty("Modeled", Required = Required.Always)]
        public bool Modeled { get; set; }

        [JsonProperty("Items", Required = Required.Always)]
        public List<PartData> Items { get; set; }
    }
}