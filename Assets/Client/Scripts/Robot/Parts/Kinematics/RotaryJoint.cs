using System;
using Client.Scripts.Ui;
using UnityEngine;

namespace Client.Scripts.Robot.Parts.Kinematics
{
    public class RotaryJoint : KinematicJoint, ISelectablePart
    {
        public AngleSelectorController angleSelectorController;

        public float initialAngle = 180.0f;
        private float _currentAngle;

        private float _minAngle;

        public float MinAngle
        {
            get => _minAngle;
            set => _minAngle = Mathf.Clamp(value, 0f, MaxAngle);
        }

        private float _maxAngle;

        public float MaxAngle
        {
            get => _maxAngle;
            set => _maxAngle = Mathf.Clamp(value, MinAngle, 360f);
        }

        private void Start()
        {
            Init();

            angleSelectorController.gameObject.SetActive(false);
        }

        public void Setup(float mnAngle, float mxAngle, float iAngle)
        {
            _minAngle = mnAngle;
            _maxAngle = mxAngle;
            initialAngle = iAngle;

            Init();
        }

        protected override void Init()
        {
            if (!(_minAngle < initialAngle && initialAngle < _maxAngle))
                Debug.LogWarning("");

            _currentAngle = initialAngle;
        }

        /// <summary>
        /// Изменить угол на заданную величину
        /// </summary>
        /// <param name="angleChange">Значение изменения угла</param>
        public void ChangeAngle(float angleChange)
        {
            var desiredAngle = _currentAngle + angleChange;
            desiredAngle = Mathf.Clamp(desiredAngle, _minAngle, _maxAngle);
            if (Math.Abs(desiredAngle - _currentAngle) > 0.01f)
                SetAngle(desiredAngle);
        }

        /// <summary>
        /// Уставновить угол заданной величиной
        /// </summary>
        public void SetAngle(float desiredAngle)
        {
            secondObject.transform.RotateAround(firstObject.transform.position, transform.right,
                desiredAngle - _currentAngle);
            _currentAngle = desiredAngle;
        }

        public new void Initialize(Transform body1, Transform body2)
        {
            base.Initialize(body1, body2);

            // Set initial angle
            SetAngle(Item.InitialAngle);
        }

        public void Select()
        {
            transform.Find("Mesh").GetComponent<MeshRenderer>().material.SetFloat("_IsEnabled", 1f);
        }

        public void Deselect()
        {
            transform.Find("Mesh").GetComponent<MeshRenderer>().material.SetFloat("_IsEnabled", 0f);
        }
    }
}