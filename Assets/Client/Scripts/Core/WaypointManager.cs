using UnityEngine;

namespace Client.Scripts.Core
{
    public class WaypointManager : MonoBehaviour
    {
        public GameObject waypointsHolder;

        private void Update()
        {
            if (waypointsHolder.transform.childCount == 0)
            {
                ModelManager.Instance.SimulationComplete = true;
                Destroy(this);
            }
        }
    }
}