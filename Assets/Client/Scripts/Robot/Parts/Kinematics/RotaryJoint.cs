﻿using System;
using Client.Scripts.Service.Model;
using Client.Scripts.Ui;
using UnityEngine;

namespace Client.Scripts.Robot.Parts.Kinematics
{
    public class RotaryJoint : KinematicJoint, IPart
    {
        private Guid _id;

        private float _rotationY;

        public AngleSelectorController angleSelectorController;

        private float _currentAngle = 180f;

        public float CurrentAngle
        {
            get => _currentAngle;
            set
            {
                SetAngle(value);
                _currentAngle = value;
            }
        }

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

        public void Setup(float mnAngle, float mxAngle)
        {
            _minAngle = mnAngle;
            _maxAngle = mxAngle;
        }

        protected override void Init()
        {
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
            if (firstObject == null || secondObject == null)
                return;

            desiredAngle = Mathf.Clamp(desiredAngle, _minAngle, _maxAngle);
            secondObject.transform.RotateAround(firstObject.transform.position, transform.right,
                desiredAngle - _currentAngle);
            _currentAngle = desiredAngle;
        }

        public new void Initialize(Transform body1, Transform body2)
        {
            base.Initialize(body1, body2);
        }

        public void Select()
        {
            transform.Find("Mesh").GetComponent<MeshRenderer>().material.SetFloat("_IsEnabled", 1f);
        }

        public void Deselect()
        {
            transform.Find("Mesh").GetComponent<MeshRenderer>().material.SetFloat("_IsEnabled", 0f);
        }

        public void Deserialize(PartData data)
        {
            _id = data.Id;
            _rotationY = data.RotationY;
            MinAngle = data.MinAngle;
            MaxAngle = data.MaxAngle;
        }

        public PartData Serialize()
        {
            return new PartData
            {
                Id = _id,
                Type = "RotaryJoint",
                RotationY = _rotationY,
                MinAngle = _minAngle,
                MaxAngle = _maxAngle,
            };
        }
    }
}