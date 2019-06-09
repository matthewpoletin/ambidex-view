using System;
using System.Collections.Generic;

namespace Client.Scripts.Service.Model
{
    [Serializable]
    public struct Position
    {
        public float X;
        public float Y;
        public float Z;
    }

    [Serializable]
    public class WaypointData
    {
        public Position Position;
    }

    [Serializable]
    public class WaypointPath
    {
        public List<WaypointData> Waypoints;
    }
}