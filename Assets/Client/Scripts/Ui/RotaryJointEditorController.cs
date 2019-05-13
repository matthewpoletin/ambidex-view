using System.Globalization;
using Client.Scripts.Robot.Kinematics;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts
{
    public class RotaryJointEditorController : MonoBehaviour
    {
        private RotaryJoint _selectedRotaryJoint = null;
        private AngleSelectorController _angleSelectorController = null;

        public InputField minAngleInputField;
        public Slider minAngleSlider;

        public InputField maxAngleInputField;
        public Slider maxAngleSlider;

        private float _minAngle = 0.0f;

        private float MinAngle
        {
            get => _minAngle;
            set
            {
                _minAngle = Mathf.Clamp(value, 0f, MaxAngle);
                minAngleInputField.text = _minAngle.ToString(CultureInfo.CurrentCulture);
                _angleSelectorController.MinAngle = _minAngle;
                _selectedRotaryJoint.MinAngle = _minAngle;
            }
        }

        private float _maxAngle = 360.0f;

        private float MaxAngle
        {
            get => _maxAngle;
            set
            {
                _maxAngle = Mathf.Clamp(value, MinAngle, 360f);
                maxAngleInputField.text = _maxAngle.ToString(CultureInfo.CurrentCulture);
                _angleSelectorController.MaxAngle = _maxAngle;
                _selectedRotaryJoint.MaxAngle = _maxAngle;
            }
        }

        private void Start()
        {
            minAngleSlider.onValueChanged.AddListener(OnMinAngleSliderValueChange);
            maxAngleSlider.onValueChanged.AddListener(OnMaxAngleSliderValueChange);
        }

        private void OnMinAngleSliderValueChange(float value)
        {
            MinAngle = value * 360f;
        }

        private void OnMaxAngleSliderValueChange(float value)
        {
            MaxAngle = value * 360f;
        }

        public void Activate(RotaryJoint selectedRotaryJoint)
        {
            _selectedRotaryJoint = selectedRotaryJoint;
            _angleSelectorController = selectedRotaryJoint.angleSelectorController;
            _angleSelectorController.gameObject.SetActive(true);

            MinAngle = _selectedRotaryJoint.MinAngle;
            MaxAngle = _selectedRotaryJoint.MaxAngle;

            minAngleSlider.value = MinAngle / 360f;
            maxAngleSlider.value = MaxAngle / 360f;
        }

        public void Deactivate()
        {
            _selectedRotaryJoint = null;
            _angleSelectorController.gameObject.SetActive(false);
            _angleSelectorController = null;
        }
    }
}