using Client.Scripts.Service.Model;
using UnityEngine;

namespace Client.Scripts.Robot.Kinematics
{
    [ExecuteInEditMode]
    public abstract class KinematicJoint : MonoBehaviour
    {
        protected Item _item;

        public GameObject firstObject;
        public GameObject secondObject;

        protected abstract void Init();

        public void Setup(Item item)
        {
            _item = item;
        }
        
        public void Initialize(Transform body1, Transform body2)
        {
            // Set regulated objects
            firstObject = body1.gameObject;
            secondObject = body2.gameObject;
        }
    }
}