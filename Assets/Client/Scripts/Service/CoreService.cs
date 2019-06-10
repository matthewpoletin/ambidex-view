using System.Collections.Generic;
using System.Linq;
using Client.Scripts.Core;
using Client.Scripts.Service.Model;
using Newtonsoft.Json;
using Proyecto26;

namespace Client.Scripts.Service
{
    public static class CoreService
    {
        private const string CoreUrl = "http://46.39.225.177:9000";

        public static void Simulate()
        {
            RestClient.Get($"{CoreUrl}/simulation").Then(response =>
            {
                ModelManager.Instance.simulationProcess = new Queue<SimulationStep>(
                    JsonConvert.DeserializeObject<SimulationResponse>(response.Text).process
                        .OrderBy(step => step.stepId));
            });
        }
    }
}