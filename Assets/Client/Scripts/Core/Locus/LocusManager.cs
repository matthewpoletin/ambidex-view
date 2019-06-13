using System.Collections.Generic;
using Client.Scripts.Robot;
using Client.Scripts.Robot.Parts.Kinematics;
using Client.Scripts.Service.Model;
using UnityEngine;

namespace Client.Scripts.Core.Locus
{
    /// <summary>
    /// Locus of possible positions
    /// </summary>
    public class LocusManager : MonoBehaviour
    {
        #region Singleton

        public static LocusManager Instance { get; private set; } = null;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }

        #endregion

        [SerializeField]
        private Transform locusHolder = null;

        [SerializeField]
        private GameObject locusPointPrefab = null;

        private LocusPointController _selectedLocusPoint = null;

        private void Start()
        {
            Clear();
        }

        /// <summary>
        /// Add points to locus
        /// </summary>
        /// <param name="data">Data of points</param>
        private void Populate(List<LocusPointData> data)
        {
            foreach (var pointData in data)
            {
                var go = Instantiate(locusPointPrefab, locusHolder);
                go.transform.localPosition =
                    new Vector3(pointData.Position.X, pointData.Position.Y, pointData.Position.Z);
                go.GetComponent<LocusPointController>().Initialize(pointData);
            }
        }

        /// <summary>
        /// Remove all points from locus
        /// </summary>
        private void Clear()
        {
            foreach (Transform point in locusHolder)
                Destroy(point.gameObject);
        }

        public void Show()
        {
            locusHolder.gameObject.SetActive(true);
        }

        public void Hide()
        {
            locusHolder.gameObject.SetActive(false);
        }

        public void LoadAndShow(List<LocusPointData> result)
        {
            Clear();
            Populate(result);
            Show();
        }

        public void SelectLocusPoint(LocusPointController locusPointController)
        {
            if (_selectedLocusPoint != null)
            {
                _selectedLocusPoint = null;
                locusPointController.Deselect();
                return;
            }

            foreach (var partData in locusPointController.data.Configuration)
            {
                var go = RobotController.Instance.GetObjectById(partData.Id);
                switch (partData.Type)
                {
                    case "RotaryJoint":
                        go.GetComponent<RotaryJoint>().InitialAngle = partData.CurrentAngle;
                        break;
                }
            }
        }
    }
}