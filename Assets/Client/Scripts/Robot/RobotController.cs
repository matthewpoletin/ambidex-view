using System;
using System.Collections.Generic;
using System.IO;
using Client.Scripts.Robot.Parts.Common;
using Client.Scripts.Robot.Parts.Common.Tips;
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

        private string lastFileName = null;

        public void ClearRoot()
        {
            foreach (Transform child in bodyRoot)
                Destroy(child.gameObject);
            ItemData.Clear();
        }

        public void Rebuild()
        {
            if (lastFileName == null)
                return;

            BuildFromFile(lastFileName);
        }

        public void Unload()
        {
            lastFileName = null;
            ClearRoot();
        }

        #region Serialization

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
            var data = JsonConvert.DeserializeObject<Design>(fileString);
            BuildFromData(data.RobotConfiguration.Items);
        }

        public bool BuildFromData(List<PartData> data)
        {
            ClearRoot();

            var nextParent = bodyRoot;
            for (var i = 0; i < data.Count; i++)
            {
                var item = data[i];

                switch (item.Type)
                {
                    case "Beam":
                        GameObject beamGo;
                        (beamGo, nextParent) = PartFactory.Instance.BuildBeam(item, nextParent);
                        beamGo.GetComponent<Beam>().Deserialize(item);
                        AddItem(item.Id, ItemType.Beam, beamGo);
                        break;
                    case "RotaryJoint":
                        var rotaryJointGo = PartFactory.Instance.BuildRotaryJoint(item, nextParent).Item1;
                        rotaryJointGo.GetComponent<RotaryJoint>().Setup(item);
                        rotaryJointGo.GetComponent<RotaryJoint>().Deserialize(item);
                        AddItem(item.Id, ItemType.RotaryJoint, rotaryJointGo);
                        break;
                    case "RevoluteJoint":
                        var revoluteJointGo = PartFactory.Instance.BuildRevoluteJoint(item, nextParent).Item1;
                        revoluteJointGo.GetComponent<RevoluteJoint>().Setup(item);
                        revoluteJointGo.GetComponent<RevoluteJoint>().Deserialize(item);
                        AddItem(item.Id, ItemType.RevoluteJoint, revoluteJointGo);
                        break;
                    case "Tip":
                        var tipGo = PartFactory.Instance.BuildTip(item, nextParent);
                        tipGo.GetComponent<BasicTip>().Deserialize(item);
                        AddItem(item.Id, ItemType.BasicTip, tipGo);
                        break;
                    default:
                        Debug.LogWarning($"Unrecognized item type {item.Type}");
                        break;
                }
            }

            // Initialize joints
            foreach (var item in ItemData)
            {
                var jointGo = item.Value.GameObject;
                var type = item.Value.Type;

                if (!(type == ItemType.RevoluteJoint || type == ItemType.RotaryJoint))
                    continue;

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

            return true;
        }

        /// <summary>
        /// Serialize all robot configuration data
        /// </summary>
        /// <returns>Configuration of robot</returns>
        public RobotConfiguration Serialize()
        {
            var robotConfiguration = new RobotConfiguration
            {
                Modeled = false,
                Items = new List<PartData>(),
            };

            foreach (var pair in ItemData)
            {
                PartData data = null;
                var go = pair.Value.GameObject;
                switch (pair.Value.Type)
                {
                    case ItemType.RevoluteJoint:
                        data = go.GetComponent<RevoluteJoint>().Serialize();
                        break;
                    case ItemType.RotaryJoint:
                        data = go.GetComponent<RotaryJoint>().Serialize();
                        break;
                    case ItemType.Beam:
                        data = go.GetComponent<Beam>().Serialize();
                        break;
                    case ItemType.BasicTip:
                        data = go.GetComponent<BasicTip>().Serialize();
                        break;
                    case ItemType.Undefined:
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                data.Id = pair.Key;
                robotConfiguration.Items.Add(data);
            }

            return robotConfiguration;
        }

        #endregion

        #region ItemData

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

        #endregion
    }
}