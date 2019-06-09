using System.Collections.Generic;
using Client.Scripts.Service.Model;
using UnityEngine;

namespace Client.Scripts.Core
{
    public class WaypointManager : MonoBehaviour
    {
        #region Singleton

        public static WaypointManager Instance { get; private set; } = null;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }

        #endregion

        [SerializeField]
        private GameObject waypointPrefab = null;

        public Transform waypointsHolder;

        private IEnumerable<WaypointData> _lastLoadedData;

        /// <summary>
        /// Remove all waypoint game objects
        /// </summary>
        public void ClearRoot()
        {
            foreach (Transform child in waypointsHolder)
                Destroy(child.gameObject);
        }

        /// <summary>
        /// Build waypoints from waypoint data
        /// </summary>
        /// <param name="waypoints">List of waypoint data</param>
        public void Populate(IEnumerable<WaypointData> waypoints)
        {
            foreach (var waypoint in waypoints)
            {
                var waypointGo = Instantiate(waypointPrefab, waypointsHolder);
                waypointGo.transform.localPosition =
                    new Vector3(waypoint.Position.X, waypoint.Position.Y, waypoint.Position.Z);
            }
        }

        /// <summary>
        /// Load new waypoints data
        /// </summary>
        /// <param name="waypoints">List of waypoint data</param>
        public bool Load(IEnumerable<WaypointData> waypoints)
        {
            if (waypoints == null)
                return false;
            
            ClearRoot();
            Populate(waypoints);
            return true;
        }

        public void Reload()
        {
            Populate(_lastLoadedData);
        }

        public void Unload()
        {
            _lastLoadedData = null;
            ClearRoot();
        }

        private void Update()
        {
            if (waypointsHolder.childCount == 0)
            {
                ModelManager.Instance.SimulationComplete = true;
                Destroy(this);
            }
        }
    }
}