using System;
using Client.Scripts.Service.Model;

namespace Client.Scripts.Robot.Parts.Kinematics
{
    public class BallJoint : KinematicJoint, IPart
    {
        private void Start()
        {
            Init();
        }

        protected override void Init()
        {
        }

        public void Select()
        {
            throw new NotImplementedException();
        }

        public void Deselect()
        {
            throw new NotImplementedException();
        }

        public void Deserialize(PartData data)
        {
            throw new NotImplementedException();
        }

        public PartData Serialize()
        {
            throw new NotImplementedException();
        }
    }
}