using System;
using Client.Scripts.Service.Model;
using Client.Scripts.Ui.Logging.View;
using UnityEngine;

namespace Client.Scripts.Core
{
    [RequireComponent(typeof(Collider))]
    public class WaypointController : MonoBehaviour
    {
        public Guid id;

        public float PositionX
        {
            set => transform.localPosition = new Vector3(value, transform.localPosition.y, transform.localPosition.z);
        }

        public float PositionY
        {
            set => transform.localPosition = new Vector3(transform.localPosition.x, value, transform.localPosition.z);
        }

        public float PositionZ
        {
            set => transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, value);
        }

        public void Initialize(WaypointData data)
        {
            transform.localPosition = new Vector3(data.Position.X, data.Position.Y, data.Position.Z);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Tip"))
            {
                LogController.Instance.WriteInfoLog($"{name} collected");
                Destroy(gameObject);
            }
        }

        private void OnMouseDown()
        {
            WaypointManager.Instance.SelectWaypoint(this);
        }
    }
}