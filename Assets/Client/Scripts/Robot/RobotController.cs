using System;
using System.Collections.Generic;
using System.IO;
using Client.Scripts.Core;
using Client.Scripts.Robot.Parts.Kinematics;
using Client.Scripts.Service.Model;
using Newtonsoft.Json;
using UnityEngine;

namespace Client.Scripts.Robot
{
    public class ItemData
    {
        public ItemType Type;
        public GameObject GameObject;
    }

    public enum ItemType
    {
        Undefined = 0,

        RevoluteJoint,
        RotaryJoint,
        Beam,
        BasicTip,
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

        public readonly Dictionary<Guid, ItemData> ItemData =
            new Dictionary<Guid, ItemData>();

        private string _lastFileName = null;

        public void ClearRoot()
        {
            foreach (Transform child in bodyRoot)
                Destroy(child.gameObject);
            ItemData.Clear();
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

            var data = JsonConvert.DeserializeObject<Design>(fileString);

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
                        AddItem(item.Id, ItemType.RotaryJoint, rotaryJointGo);
                        break;
                    case "RevoluteJoint":
                        var revoluteJointGo = PartFactory.Instance.BuildRevoluteJoint(item, nextParent).Item1;
                        revoluteJointGo.GetComponent<RevoluteJoint>().Setup(item);
                        AddItem(item.Id, ItemType.RevoluteJoint, revoluteJointGo);
                        break;
                    case "Tip":
                        var tipGo = PartFactory.Instance.BuildTip(item, nextParent);
                        break;
                    default:
                        Debug.LogWarning($"Unrecognized item type {item.Type}");
                        break;
                }
            }

            // Initialize joints
            foreach (var jointDataItem in ItemData)
            {
                var jointGo = jointDataItem.Value.GameObject;
                var type = jointDataItem.Value.Type;
                var body1 = jointGo.transform.parent;
                var body2 = jointGo.transform.parent.GetChild(jointGo.transform.GetSiblingIndex() + 1);
                switch (type)
                {
                    case ItemType.RevoluteJoint:
                        jointGo.GetComponent<RevoluteJoint>().Initialize(body1, body2);
                        break;
                    case ItemType.RotaryJoint:
                        jointGo.GetComponent<RotaryJoint>().Initialize(body1, body2);
                        break;
                    case ItemType.Undefined:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            // Load waypoint data
            WaypointManager.Instance.Load(data.WaypointPath.Waypoints);
        }

        private void AddItem(Guid id, ItemType type, GameObject go)
        {
            ItemData.Add(id, new ItemData {GameObject = go, Type = type});
        }

        public ItemData GetItemById(Guid id)
        {
            return ItemData.ContainsKey(id) ? ItemData[id] : null;
        }

        public GameObject GetObjectById(Guid id)
        {
            return GetItemById(id).GameObject;
        }

        public List<Tuple<Guid, GameObject>> GetAllObjectsByType(ItemType type)
        {
            var result = new List<Tuple<Guid, GameObject>>();
            foreach (var item in ItemData)
                if (item.Value.Type == type)
                    result.Add(Tuple.Create(item.Key, item.Value.GameObject));
            return result;
        }
    }
}