using Client.Scripts.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.Ui.Editors
{
    public class WaypointEditor : MonoBehaviour
    {
        private WaypointController _selectedWaypoint = null;

        public InputField xValueInputField;
        public InputField yValueInputField;
        public InputField zValueInputField;
        public Button deleteButton;

        private void Start()
        {
            xValueInputField.onValueChanged.AddListener(OnXValueInputFieldChange);
            yValueInputField.onValueChanged.AddListener(OnYValueInputFieldChange);
            zValueInputField.onValueChanged.AddListener(OnZValueInputFieldChange);
            deleteButton.onClick.AddListener(OnDeleteButtonClick);
        }

        private void OnXValueInputFieldChange(string value)
        {
            if (_selectedWaypoint == null)
                return;

            _selectedWaypoint.PositionX = float.Parse(value);
        }

        private void OnYValueInputFieldChange(string value)
        {
            if (_selectedWaypoint == null)
                return;

            _selectedWaypoint.PositionY = float.Parse(value);
        }

        private void OnZValueInputFieldChange(string value)
        {
            if (_selectedWaypoint == null)
                return;

            _selectedWaypoint.PositionZ = float.Parse(value);
        }

        private void OnDeleteButtonClick()
        {
            if (_selectedWaypoint == null)
                return;

            WaypointManager.Instance.DeleteWaypoint(_selectedWaypoint.id);
            _selectedWaypoint = null;
        }

        public void Activate(WaypointController waypointController)
        {
            _selectedWaypoint = waypointController;

            var waypointPosition = waypointController.transform.localPosition;
            xValueInputField.text = waypointPosition.x.ToString();
            yValueInputField.text = waypointPosition.y.ToString();
            zValueInputField.text = waypointPosition.z.ToString();
        }

        public void Deactivate()
        {
            _selectedWaypoint = null;
        }
    }
}