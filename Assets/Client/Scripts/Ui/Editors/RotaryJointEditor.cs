using System.Globalization;
using Client.Scripts.Robot.Parts.Kinematics;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.Ui.Editors
{
    public class RotaryJointEditor : MonoBehaviour
    {
        private RotaryJoint _selectedRotaryJoint = null;
        private AngleSelectorController _angleSelectorController = null;

        public InputField minAngleInputField;
        public Slider minAngleSlider;

        public InputField maxAngleInputField;
        public Slider maxAngleSlider;

        public InputField currentAngleInputField;
        public Slider currentAngleSlider;

        private float _minAngle = 0f;

        private float MinAngle
        {
            get => _minAngle;
            set
            {
                _minAngle = Mathf.Clamp(value, 0f, MaxAngle);
                minAngleInputField.text = _minAngle.ToString(CultureInfo.CurrentCulture);
                _angleSelectorController.MinAngle = _minAngle;
                _selectedRotaryJoint.MinAngle = _minAngle;
                minAngleSlider.value = _minAngle / 360f;
            }
        }

        private float _maxAngle = 360f;

        private float MaxAngle
        {
            get => _maxAngle;
            set
            {
                _maxAngle = Mathf.Clamp(value, MinAngle, 360f);
                maxAngleInputField.text = _maxAngle.ToString(CultureInfo.CurrentCulture);
                _angleSelectorController.MaxAngle = _maxAngle;
                _selectedRotaryJoint.MaxAngle = _maxAngle;
                maxAngleSlider.value = _maxAngle / 360f;
            }
        }

        private float _currentAngle = 180f;

        public float CurrentAngle
        {
            get => _currentAngle;
            set
            {
                _currentAngle = Mathf.Clamp(value, MinAngle, MaxAngle);
                currentAngleInputField.text = _currentAngle.ToString(CultureInfo.CurrentCulture);
                _angleSelectorController.InitialAngle = _currentAngle;
                _selectedRotaryJoint.CurrentAngle = _currentAngle;
                currentAngleSlider.value = _currentAngle / 360f;
            }
        }

        private void Start()
        {
            minAngleSlider.onValueChanged.AddListener(OnMinAngleSliderValueChange);
            maxAngleSlider.onValueChanged.AddListener(OnMaxAngleSliderValueChange);
            currentAngleSlider.onValueChanged.AddListener(OnCurrentAngleSliderValueChange);
        }

        private void OnMinAngleSliderValueChange(float value)
        {
            MinAngle = value * 360f;
        }

        private void OnMaxAngleSliderValueChange(float value)
        {
            MaxAngle = value * 360f;
        }

        private void OnCurrentAngleSliderValueChange(float value)
        {
            CurrentAngle = value * 360f;
        }

        public void Activate(RotaryJoint selectedRotaryJoint)
        {
            _selectedRotaryJoint = selectedRotaryJoint;
            _angleSelectorController = selectedRotaryJoint.angleSelectorController;
            _angleSelectorController.gameObject.SetActive(true);

            MinAngle = _selectedRotaryJoint.MinAngle;
            MaxAngle = _selectedRotaryJoint.MaxAngle;
            CurrentAngle = _selectedRotaryJoint.CurrentAngle;

            minAngleSlider.value = MinAngle / 360f;
            maxAngleSlider.value = MaxAngle / 360f;
            currentAngleSlider.value = CurrentAngle / 360f;
        }

        public void Deactivate()
        {
            _selectedRotaryJoint = null;
            _angleSelectorController.gameObject.SetActive(false);
            _angleSelectorController = null;
        }
    }
}