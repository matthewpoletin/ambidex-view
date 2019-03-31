using UnityEngine;

namespace Client.Scripts.Robot.Kinematics
{
    [AddComponentMenu("Robot/Kinematics/Joints/RotaryJoint")]
    public class RotaryJoint : KinematicJoint
    {
        public float initialAngle = 0.0f;
        private float _currentAngle;

        [Range(-180.0f, 180.0f)]
        public float minAngle;

        [Range(-180.0f, 180.0f)]
        public float maxAngle;

        private void Start()
        {
            Init();
        }

        protected override void Init()
        {
            _currentAngle = initialAngle;

            if (minAngle < -180.0f || maxAngle > 180.0f || minAngle > maxAngle)
                Debug.LogWarning("");
        }

        public void ChangeAngle(float angleChange)
        {
            _currentAngle += angleChange;
            if (_currentAngle < minAngle)
            {
                _currentAngle = minAngle;
                return;
            }

            if (_currentAngle > maxAngle)
            {
                _currentAngle = maxAngle;
                return;
            }

            secondObject.transform.RotateAround(firstObject.transform.position, Vector3.forward, angleChange);
        }
    }
}