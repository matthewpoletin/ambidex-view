using System;
using System.Collections.Generic;
using System.Linq;
using Client.Scripts.Service.Model;
using Client.Scripts.Ui.Editors;
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

        private WaypointController _selectedWaypoint = null;

        private readonly Dictionary<Guid, Tuple<GameObject, WaypointController>> _waypoints =
            new Dictionary<Guid, Tuple<GameObject, WaypointController>>();

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
            foreach (var dataItem in waypoints)
            {
                var waypointGo = Instantiate(waypointPrefab, waypointsHolder);
                var waypointController = waypointGo.GetComponent<WaypointController>();
                waypointController.Initialize(dataItem);
                _waypoints.Add(dataItem.Id, Tuple.Create(waypointGo, waypointController));
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
            // TODO: Serialize properly
            return new WaypointsConfiguration
            {
                Waypoints = _lastLoadedData.ToList(),
            };
        }

        #endregion

        #region Selection

        public void SelectWaypoint(WaypointController waypoint)
        {
            if (_selectedWaypoint == waypoint)
            {
                UnselectWaypoint();
                return;
            }

            _selectedWaypoint = waypoint;
            InfoPanelController.Instance.ShowWaypointEditor(waypoint);
        }

        public void UnselectWaypoint()
        {
            _selectedWaypoint = null;
        }

        #endregion

        public WaypointController CreateWaypoint()
        {
            // TODO:
            return null;
        }

        public void DeleteWaypoint(Guid id)
        {
            if (!_waypoints.TryGetValue(id, out var tuple))
                return;

            Destroy(tuple.Item1);
            _waypoints.Remove(id);
        }
    }
}