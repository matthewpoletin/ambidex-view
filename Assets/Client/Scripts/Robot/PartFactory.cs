using System;
using Client.Scripts.Robot.Parts.Kinematics;
using Client.Scripts.Service.Model;
using UnityEngine;

namespace Client.Scripts.Robot
{
    public class PartFactory : MonoBehaviour
    {
        #region Singleton

        public static PartFactory Instance { get; private set; } = null;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }

        #endregion

        private static Vector3 _resultingScale = Vector3.one;

        #region Prefabs

        [Header("Prefabs")]
        public GameObject beamPrefab;

        public GameObject tipPrefab;

        public GameObject rotaryJointPrefab;
        public GameObject revoluteJointPrefab;

        #endregion

        public Tuple<GameObject, Transform> BuildBeam(PartData partData, Transform parent)
        {
            // Create object
            var beamGo = Instantiate(beamPrefab, parent);

            // Find next parent
            Transform nextParent = null;
            foreach (Transform child in beamGo.transform)
            {
                if (child.name == "End")
                {
                    nextParent = child;
                    break;
                }
            }

            // Set size
            var newScale = new Vector3(1, partData.Length / _resultingScale.y, 1);
            beamGo.transform.localScale = newScale;
            _resultingScale = new Vector3(
                newScale.x * _resultingScale.x,
                newScale.y * _resultingScale.y,
                newScale.z * _resultingScale.z
            );

            return Tuple.Create(beamGo, nextParent);
        }

        public GameObject BuildTip(PartData partData, Transform parent)
        {
            var tipGo = Instantiate(tipPrefab, parent);
            return tipGo;
        }

        public Tuple<GameObject, Transform> BuildRotaryJoint(PartData partData, Transform parent)
        {
            var jointGo = Instantiate(rotaryJointPrefab, parent);
            jointGo.transform.Rotate(0, partData.RotationY, 0);
            jointGo.GetComponent<RotaryJoint>().Setup(partData.MinAngle, partData.MaxAngle);
            return Tuple.Create(jointGo, parent);
        }

        public Tuple<GameObject, Transform> BuildRevoluteJoint(PartData partData, Transform parent)
        {
            var jointGo = Instantiate(revoluteJointPrefab, parent);
            jointGo.transform.Rotate(0, partData.RotationY, 0);
            jointGo.GetComponent<RevoluteJoint>().Setup(partData.MinAngle, partData.MaxAngle, partData.InitialAngle);
            return Tuple.Create(jointGo, parent);
        }
    }
}