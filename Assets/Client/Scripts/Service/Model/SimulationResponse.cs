using System;
using System.Collections.Generic;

namespace Client.Scripts.Service.Model
{
    [Serializable]
    public class SimulationResponseItem
    {
        public Guid itemId;
        public float angle;
    }

    [Serializable]
    public class SimulationStep
    {
        public int stepId;
        public List<SimulationResponseItem> items;
    }

    [Serializable]
    public class SimulationResponse
    {
        public Guid designId;
        public List<SimulationStep> process;
    }
}