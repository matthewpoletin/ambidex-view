using Client.Scripts.Core;
using Client.Scripts.Robot.Parts.Common;
using Client.Scripts.Robot.Parts.Common.Tips;
using Client.Scripts.Robot.Parts.Kinematics;
using Client.Scripts.Ui.Editors;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Client.Scripts.Robot
{
    public enum SelectedObjectType
    {
        None,
        Beam,
        RotaryJoint,
        RevoluteJoint,
        BasicTip,
    }

    public class PartSelection : MonoBehaviour
    {
        #region Singleton

        public static PartSelection Instance { get; private set; } = null;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }

        #endregion

        private Camera _camera;
        private CameraController _cameraController;

        private SelectedObjectType _selectedObjectType = SelectedObjectType.None;
        private Transform _selectedObject;

        private void Start()
        {
            _camera = Camera.main;
            if (!_camera)
                Debug.LogWarning("Main camera is undefined");

            _cameraController = _camera.GetComponent<CameraController>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Check if the mouse was clicked over a UI element
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }

                if (!Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out var hit))
                {
                    // Did not hit anything
                    UnselectObject();
                    CameraOnDefault();
                }
                else
                {
                    // If we clicked on the same object
                    if (hit.transform == _selectedObject)
                    {
                        UnselectObject();
                        return;
                    }

                    // If we clicked on different object
                    UnselectObject();
                    // Save selected object
                    _selectedObject = hit.transform;
                    // Detect type of selected object
                    switch (hit.transform.tag)
                    {
                        case "Beam":
                            _selectedObjectType = SelectedObjectType.Beam;
                            _selectedObject.GetComponent<Beam>().Select();
                            CameraOnSelected();
                            break;
                        case "Tip":
                            _selectedObjectType = SelectedObjectType.BasicTip;
                            _selectedObject.GetComponent<BasicTip>().Select();
                            CameraOnSelected();
                            break;
                        case "RotaryJoint":
                            _selectedObjectType = SelectedObjectType.RotaryJoint;
                            var rotaryJoint = hit.transform.GetComponent<RotaryJoint>();
                            rotaryJoint.Select();
                            InfoPanelController.Instance.ShowRotaryJointEditor(rotaryJoint);
                            CameraOnSelected();
                            break;
                        case "RevoluteJoint":
                            _selectedObjectType = SelectedObjectType.RevoluteJoint;
                            var revoluteJoint = hit.transform.GetComponent<RevoluteJoint>();
                            revoluteJoint.Select();
                            InfoPanelController.Instance.ShowRevoluteJointEditor(revoluteJoint);
                            CameraOnSelected();
                            break;
                        default:
                            CameraOnDefault();
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Remove selection of current active object
        /// </summary>
        public void UnselectObject()
        {
            // Nothing was selected
            if (_selectedObject == null)
                return;

            switch (_selectedObjectType)
            {
                case SelectedObjectType.Beam:
                    _selectedObject.GetComponent<Beam>().Deselect();
                    break;
                case SelectedObjectType.BasicTip:
                    _selectedObject.GetComponent<BasicTip>().Deselect();
                    break;
                case SelectedObjectType.RotaryJoint:
                    InfoPanelController.Instance.HideRotaryJointEditor();
                    _selectedObject.GetComponent<RotaryJoint>().Deselect();
                    break;
                case SelectedObjectType.RevoluteJoint:
                    InfoPanelController.Instance.HideRevoluteJointEditor();
                    _selectedObject.GetComponent<RevoluteJoint>().Deselect();
                    break;
                case SelectedObjectType.None:
                default:
                    break;
            }

            _selectedObject = null;
            _selectedObjectType = SelectedObjectType.None;

            CameraOnDefault();
        }

        /// <summary>
        /// Set camera to selected target
        /// </summary>
        private void CameraOnSelected()
        {
            // Nothing is selected
            if (_selectedObject == null)
                return;

            _cameraController.target = _selectedObject;
            _cameraController.useOffset = false;
        }

        /// <summary>
        /// Set camera to default target
        /// </summary>
        public void CameraOnDefault()
        {
            _cameraController.target = transform;
            _cameraController.useOffset = true;
        }
    }
}