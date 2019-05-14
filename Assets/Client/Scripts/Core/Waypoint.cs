using Client.Scripts.Ui.Logging.View;
using UnityEngine;

namespace Client.Scripts.Core
{
    public class Waypoint : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Tip"))
            {
                LogController.Instance.WriteInfoLog($"{name} collected");
                Destroy(gameObject);
            }
        }
    }
}