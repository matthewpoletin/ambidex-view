using System;

namespace Client.Scripts.Service.Model
{
    [Serializable]
    public class Design
    {
        public string Name;
        public string Description;
        public string Author;
        public string CreateDate;
        public RobotConfiguration RobotConfiguration;
        public WaypointPath WaypointPath;
    }
}