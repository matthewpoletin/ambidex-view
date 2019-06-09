using System;
using System.Collections.Generic;

namespace Client.Scripts.Service.Model
{
    [Serializable]
    public class RobotConfiguration
    {
        public bool Modeled;
        public List<Item> Items;
    }

    [Serializable]
    public class Item
    {
        public string Type;
        public float Length;
        public float Diameter;
        public float MinAngle;
        public float MaxAngle;
        public float InitialAngle;
        public float RotationY;
    }
}