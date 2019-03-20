using UnityEngine;

namespace Client.Scripts.View
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        private Camera _camera;

        private void Start()
        {
            _camera = GetComponent<Camera>();
        }
    }
}