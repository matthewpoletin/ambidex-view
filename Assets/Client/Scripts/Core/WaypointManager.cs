using System;
using System.Collections.Generic;
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
            foreach (var pair in _waypoints)
                Destroy(pair.Value.Item1);

            _waypoints.Clear();
        }

        /// <summary>
        /// Build waypoints from waypoint data
        /// </summary>
        /// <param name="waypoints">List of waypoint data</param>
        public void Populate(List<WaypointData> waypoints)
        {
            foreach (var dataItem in waypoints)
            {
                CreateWaypoint(dataItem);
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
                Waypoints = SerializeData(),
            };
        }

        public List<WaypointData> SerializeData()
        {
            var data = new List<WaypointData>();
            foreach (var pair in _waypoints)
                data.Add(pair.Value.Item2.Serialize());

            return data;
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
            InfoPanelController.Instance.HideWaypointEditor();
            _selectedWaypoint = null;
        }

        #endregion

        public WaypointController CreateWaypoint()
        {
            return CreateWaypoint(new WaypointData
            {
                Id = Guid.NewGuid(),
                Position = new Position
                {
                    X = 0f,
                    Y = 0f,
                    Z = 0f,
                }
            });
        }

        private WaypointController CreateWaypoint(WaypointData data)
        {
            var waypointGo = Instantiate(waypointPrefab, waypointsHolder);
            var waypointController = waypointGo.GetComponent<WaypointController>();
            waypointController.Initialize(data);
            _waypoints.Add(data.Id, Tuple.Create(waypointGo, waypointController));
            return waypointController;
        }

        public void DeleteWaypoint(Guid id)
        {
            if (!_waypoints.TryGetValue(id, out var tuple))
                return;

            Destroy(tuple.Item1);
            _waypoints.Remove(id);
        }

        public void CollectWaypoint(Guid id)
        {
            if (!_waypoints.TryGetValue(id, out var tuple))
                return;

            tuple.Item1.SetActive(false);
            // TODO: Calculate this is the last
        }

        public void SaveData()
        {
            var data = new List<WaypointData>();
            foreach (var pair in _waypoints)
            {
                data.Add(pair.Value.Item2.Serialize());
            }

            _lastLoadedData = data;
        }

        #region Activity

        public bool WaypointsShown
        {
            set
            {
                if (value)
                    ShowAll();
                else
                    HideAll();
            }
        }

        public void ShowAll()
        {
            waypointsHolder.gameObject.SetActive(true);
        }

        public void HideAll()
        {
            waypointsHolder.gameObject.SetActive(false);
        }

        #endregion
    }
}