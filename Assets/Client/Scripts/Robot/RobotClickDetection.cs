using Client.Scripts.Robot.Kinematics;
using Client.Scripts.Ui.Editors;
using UnityEngine;

namespace Client.Scripts.Robot
{
    public enum SelectedObjectType
    {
        None,
        Beam,
        RotaryJoint,
        RevoluteJoint,
        Tip,
    }

    public class RobotClickDetection : MonoBehaviour
    {
        private Camera _camera;

        private SelectedObjectType _selectedObjectType = SelectedObjectType.None;
        private Transform _selectedObject;

        private void Start()
        {
            _camera = Camera.main;
            if (!_camera)
                Debug.LogWarning("Main camera is undefined");
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit))
                {
                    // If we clicked on the same object
                    if (hit.transform == _selectedObject)
                    {
                        UnselectObject();
                        return;
                    }

                    // Save selected object
                    _selectedObject = hit.transform;
                    // Detect type of selected object
                    if (hit.transform.CompareTag("Beam"))
                    {
                        UnselectObject();

                        _selectedObjectType = SelectedObjectType.Beam;
                    }
                    else if (hit.transform.CompareTag("RotaryJoint"))
                    {
                        UnselectObject();

                        _selectedObjectType = SelectedObjectType.RotaryJoint;
                        InfoPanelController.Instance.ShowRotaryJointEditor(hit.transform.GetComponent<RotaryJoint>());
                    }
                    else if (hit.transform.CompareTag("RevoluteJoint"))
                    {
                        UnselectObject();

                        _selectedObjectType = SelectedObjectType.RevoluteJoint;
                        InfoPanelController.Instance.ShowRevoluteJointEditor(
                            hit.transform.GetComponent<RevoluteJoint>());
                    }
                    else if (hit.transform.CompareTag("Tip"))
                    {
                        UnselectObject();

                        _selectedObjectType = SelectedObjectType.Tip;
                    }
                    else
                    {
                        _selectedObject = null;
                        _selectedObjectType = SelectedObjectType.None;
                    }
                }
            }
        }

        private void UnselectObject()
        {
            switch (_selectedObjectType)
            {
                case SelectedObjectType.Beam:
                    break;
                case SelectedObjectType.RotaryJoint:
                    InfoPanelController.Instance.HideRotaryJointEditor();
                    break;
                case SelectedObjectType.RevoluteJoint:
                    InfoPanelController.Instance.HideRevoluteJointEditor();
                    break;
                case SelectedObjectType.Tip:
                    break;
                case SelectedObjectType.None:
                default:
                    break;
            }

            _selectedObject = null;
            _selectedObjectType = SelectedObjectType.None;
        }
    }
}