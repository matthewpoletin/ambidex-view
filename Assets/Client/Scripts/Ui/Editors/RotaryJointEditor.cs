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

        public InputField initialAngleInputField;
        public Slider initialAngleSlider;

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

        private float _initialAngle = 180f;

        public float InitialAngle
        {
            get => _initialAngle;
            set
            {
                _initialAngle = Mathf.Clamp(value, MinAngle, MaxAngle);
                initialAngleInputField.text = _initialAngle.ToString(CultureInfo.CurrentCulture);
                _angleSelectorController.InitialAngle = _initialAngle;
                _selectedRotaryJoint.InitialAngle = _initialAngle;
                initialAngleSlider.value = _initialAngle / 360f;
            }
        }

        private void Start()
        {
            minAngleSlider.onValueChanged.AddListener(OnMinAngleSliderValueChange);
            maxAngleSlider.onValueChanged.AddListener(OnMaxAngleSliderValueChange);
            initialAngleSlider.onValueChanged.AddListener(OnInitialAngleSliderValueChange);
        }

        private void OnMinAngleSliderValueChange(float value)
        {
            MinAngle = value * 360f;
        }

        private void OnMaxAngleSliderValueChange(float value)
        {
            MaxAngle = value * 360f;
        }

        private void OnInitialAngleSliderValueChange(float value)
        {
            InitialAngle = value * 360f;
        }

        public void Activate(RotaryJoint selectedRotaryJoint)
        {
            _selectedRotaryJoint = selectedRotaryJoint;
            _angleSelectorController = selectedRotaryJoint.angleSelectorController;
            _angleSelectorController.gameObject.SetActive(true);

            MinAngle = _selectedRotaryJoint.MinAngle;
            MaxAngle = _selectedRotaryJoint.MaxAngle;
            InitialAngle = _selectedRotaryJoint.InitialAngle;

            minAngleSlider.value = MinAngle / 360f;
            maxAngleSlider.value = MaxAngle / 360f;
            initialAngleSlider.value = InitialAngle / 360f;
        }

        public void Deactivate()
        {
            _selectedRotaryJoint = null;
            _angleSelectorController.gameObject.SetActive(false);
            _angleSelectorController = null;
        }
    }
}