using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Client.Scripts.Service.Model
{
    public class SimulationStep
    {
        [JsonProperty("stepId", Required = Required.Always)]
        public int StepId;

        [JsonProperty("configuration", Required = Required.Always)]
        public List<PartData> Configuration;
    }

    public class SimulationResponse
    {
        [JsonProperty("designId", Required = Required.Always)]
        public Guid DesignId;

        [JsonProperty("process", Required = Required.Always)]
        public List<SimulationStep> Process;
    }
}