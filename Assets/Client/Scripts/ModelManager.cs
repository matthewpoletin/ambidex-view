using Client.Scripts.Robot.Kinematics;
using UnityEngine;

namespace Client.Scripts
{
    public class ModelManager : MonoBehaviour
    {
        private void Update()
        {
            var joint = FindObjectOfType<RotaryJoint>();
            joint.ChangeAngle(0.5f);
        }
    }
}