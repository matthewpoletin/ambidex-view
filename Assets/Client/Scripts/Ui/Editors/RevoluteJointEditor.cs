using System.Globalization;
using Client.Scripts.Robot.Kinematics;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.Ui.Editors
{
    public class RevoluteJointEditor : MonoBehaviour
    {
        private RevoluteJoint _selectedRevoluteJoint = null;
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
                _selectedRevoluteJoint.MinAngle = _minAngle;
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
                _selectedRevoluteJoint.MaxAngle = _maxAngle;
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
                _selectedRevoluteJoint.initialAngle = _initialAngle;
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

        public void Activate(RevoluteJoint selectedRevoluteJoint)
        {
            _selectedRevoluteJoint = selectedRevoluteJoint;
            _angleSelectorController = selectedRevoluteJoint.angleSelectorController;
            _angleSelectorController.gameObject.SetActive(true);

            MinAngle = _selectedRevoluteJoint.MinAngle;
            MaxAngle = _selectedRevoluteJoint.MaxAngle;
            InitialAngle = _selectedRevoluteJoint.MaxAngle;

            minAngleSlider.value = MinAngle / 360f;
            maxAngleSlider.value = MaxAngle / 360f;
        }

        public void Deactivate()
        {
            _selectedRevoluteJoint = null;
            _angleSelectorController.gameObject.SetActive(false);
            _angleSelectorController = null;
        }
    }
}