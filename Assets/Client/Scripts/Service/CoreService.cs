using System;
using System.Collections.Generic;
using System.Linq;
using Client.Scripts.Core;
using Client.Scripts.Core.Locus;
using Client.Scripts.Service.Model;
using Client.Scripts.Ui.Status.View;
using Newtonsoft.Json;
using Proyecto26;

namespace Client.Scripts.Service
{
    public static class CoreService
    {
        private const string CoreUrl = "http://46.39.225.177:9000";

        /// <summary>
        /// Upload design
        /// </summary>
        /// <param name="design">Design data</param>
        public static void UploadDesign(Design design)
        {
            var body = JsonConvert.SerializeObject(design);
            RestClient.Post($"{CoreUrl}/designs", body);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void LoadLocus(Guid designId)
        {
            RestClient.Post($"{CoreUrl}/designs/{designId}/locus", "").Then(response =>
            {
                var result = JsonConvert.DeserializeObject<List<LocusPointData>>(response.Text);
                LocusManager.Instance.LoadAndShow(result);
                StatusUiController.Instance.Modify(1f, "Creating locus...");
            });
        }

        public static void UploadDesignAndLoadLocus(Design design)
        {
            StatusUiController.Instance.Modify(0f, "Uploading design...");
            var body = JsonConvert.SerializeObject(design);
            RestClient.Post($"{CoreUrl}/designs", body).Then(response =>
            {
                StatusUiController.Instance.Modify(0.5f, "Loading locus...");
                LoadLocus(design.Id);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        public static void SyncWaypointPath()
        {
            // TODO: Implement
        }

        /// <summary>
        /// Simulate waypoint path
        /// </summary>
        public static void Simulate()
        {
            RestClient.Get($"{CoreUrl}/simulation").Then(response =>
            {
                var result = JsonConvert.DeserializeObject<SimulationResponse>(response.Text);
                ModelManager.Instance.simulationProcess = new Queue<SimulationStep>(
                    result.Process.OrderBy(step => step.StepId));
            });
        }
    }
}