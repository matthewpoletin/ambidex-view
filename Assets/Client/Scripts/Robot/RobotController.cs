﻿using System;
using System.Collections.Generic;
using System.IO;
using Client.Scripts.Core;
using Client.Scripts.Robot.Parts.Kinematics;
using Client.Scripts.Service.Model;
using UnityEngine;

namespace Client.Scripts.Robot
{
    public enum JointType
    {
        Undefined = 0,

        Revolute,
        Rotary,
    }

    public class RobotController : MonoBehaviour
    {
        #region Singleton

        public static RobotController Instance { get; private set; } = null;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }

        #endregion

        public Transform bodyRoot;

        private readonly Dictionary<GameObject, JointType> _jointData = new Dictionary<GameObject, JointType>();

        private string _lastFileName = null;

        public void ClearRoot()
        {
            foreach (Transform child in bodyRoot)
                Destroy(child.gameObject);
            _jointData.Clear();
        }

        public void Rebuild()
        {
            if (_lastFileName == null)
                return;

            BuildFromFile(_lastFileName);
        }

        public void Unload()
        {
            _lastFileName = null;
            ClearRoot();
        }

        /// <summary>
        /// Build robot from json configuration file
        /// </summary>
        /// <param name="fileName">Design file</param>
        /// <returns>Build result</returns>
        public bool BuildFromFile(string fileName)
        {
            if (!File.Exists(fileName))
                return false;

            BuildFromText(File.ReadAllText(fileName));

            return true;
        }

        public void BuildFromText(string fileString)
        {
            ClearRoot();

            var data = JsonUtility.FromJson<Design>(fileString);

            var nextParent = bodyRoot;
            for (var i = 0; i < data.RobotConfiguration.Items.Count; i++)
            {
                var item = data.RobotConfiguration.Items[i];

                switch (item.Type)
                {
                    case "Beam":
                        nextParent = PartFactory.Instance.BuildBeam(item, nextParent).Item2;
                        break;
                    case "RotaryJoint":
                        var rotaryJointGo = PartFactory.Instance.BuildRotaryJoint(item, nextParent).Item1;
                        rotaryJointGo.GetComponent<RotaryJoint>().Setup(item);
                        _jointData.Add(rotaryJointGo, JointType.Rotary);
                        break;
                    case "RevoluteJoint":
                        var revoluteJointGo = PartFactory.Instance.BuildRevoluteJoint(item, nextParent).Item1;
                        revoluteJointGo.GetComponent<RevoluteJoint>().Setup(item);
                        _jointData.Add(revoluteJointGo, JointType.Revolute);
                        break;
                    case "Tip":
                        var tipGo = PartFactory.Instance.BuildTip(item, nextParent);
                        break;
                    default:
                        Debug.LogWarning($"Unrecognized item type {item.Type}");
                        break;
                }
            }

            foreach (var jointDataItem in _jointData)
            {
                var jointGo = jointDataItem.Key;
                var type = jointDataItem.Value;
                var body1 = jointGo.transform.parent;
                var body2 = jointGo.transform.parent.GetChild(jointGo.transform.GetSiblingIndex() + 1);
                switch (type)
                {
                    case JointType.Revolute:
                        jointGo.GetComponent<RevoluteJoint>().Initialize(body1, body2);
                        break;
                    case JointType.Rotary:
                        jointGo.GetComponent<RotaryJoint>().Initialize(body1, body2);
                        break;
                    case JointType.Undefined:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            // Load waypoint data
            WaypointManager.Instance.Load(data.WaypointPath.Waypoints);
        }
    }
}