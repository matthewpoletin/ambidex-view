using System.Collections.Generic;
using System.Linq;
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

        private List<WaypointData> _lastLoadedData;

        /// <summary>
        /// Remove all waypoint game objects
        /// </summary>
        public void Clear()
        {
            foreach (Transform child in waypointsHolder)
                Destroy(child.gameObject);
        }

        /// <summary>
        /// Build waypoints from waypoint data
        /// </summary>
        /// <param name="waypoints">List of waypoint data</param>
        public void Populate(List<WaypointData> waypoints)
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
        public bool Load(List<WaypointData> waypoints)
        {
            if (waypoints == null)
                return false;

            _lastLoadedData = waypoints;

            Clear();
            Populate(_lastLoadedData);
            return true;
        }

        public void Reload()
        {
            Populate(_lastLoadedData);
        }

        public void Unload()
        {
            _lastLoadedData = null;
            Clear();
        }

        #region Serializaion

        /// <summary>
        /// Save current waypoint data
        /// </summary>
        /// <returns>Configuration of waypoints</returns>
        public WaypointsConfiguration Serialize()
        {
            return new WaypointsConfiguration
            {
                Waypoints = _lastLoadedData.ToList(),
            };
        }

        #endregion
    }
}