using UnityEngine;

namespace Client.Scripts.Robot.Kinematics
{
    public abstract class KinematicJoint : MonoBehaviour
    {
        public GameObject firstObject;
        public GameObject secondObject;

        protected abstract void Init();
    }
}