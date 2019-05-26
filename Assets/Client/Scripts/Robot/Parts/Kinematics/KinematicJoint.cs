using Client.Scripts.Service.Model;
using UnityEngine;

namespace Client.Scripts.Robot.Parts.Kinematics
{
    public abstract class KinematicJoint : MonoBehaviour
    {
        protected Item Item;

        public GameObject firstObject;
        public GameObject secondObject;

        protected abstract void Init();

        public void Setup(Item item)
        {
            Item = item;
        }
        
        public void Initialize(Transform body1, Transform body2)
        {
            // Set regulated objects
            firstObject = body1.gameObject;
            secondObject = body2.gameObject;
        }
    }
}