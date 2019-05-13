﻿using UnityEngine;

namespace Client.Scripts.Core
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        public Transform target;
        public Vector3 targetOffset;
        public float distance = 5.0f;
        public float maxDistance = 20;
        public float minDistance = .6f;
        public float xSpeed = 200.0f;
        public float ySpeed = 200.0f;
        public int yMinLimit = -80;
        public int yMaxLimit = 80;
        public int zoomRate = 40;
        public float zoomDampening = 5.0f;

        private float _xDeg = 0.0f;
        private float _yDeg = 0.0f;
        private float _currentDistance;
        private float _desiredDistance;
        private Quaternion _currentRotation;
        private Quaternion _desiredRotation;
        private Quaternion _rotation;
        private Vector3 _position;

        private void Start()
        {
            Init();
        }

        private void OnEnable()
        {
            Init();
        }

        private void Init()
        {
            // Если цель не задана, создать временную цель на дистанции от текущей точки обзора камеры
            if (!target)
            {
                var go = new GameObject("Cam Target");
                go.transform.position = transform.position + transform.forward * distance;
                target = go.transform;
            }

            distance = Vector3.Distance(transform.position, target.transform.position);
            _currentDistance = distance;
            _desiredDistance = distance;

            // Получение текущих позиций и вращений как начальных
            _position = transform.position;
            _rotation = transform.rotation;
            _currentRotation = transform.rotation;
            _desiredRotation = transform.rotation;

            _xDeg = Vector3.Angle(Vector3.right, transform.right);
            _yDeg = Vector3.Angle(Vector3.up, transform.up);
        }

        // Логика камеры в данном методе, чтобы обновлять камеру только после обновления логики окружения
        private void LateUpdate()
        {
            // По ПКМ
            if (Input.GetMouseButton(1))
            {
                //// Изменение угла
                _xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                _yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                // Зажим значения угла вертикальной оси
                _yDeg = ClampAngle(_yDeg, yMinLimit, yMaxLimit);
                // Установить вращение камеры
                _desiredRotation = Quaternion.Euler(_yDeg, _xDeg, 0);
                _currentRotation = transform.rotation;

                _rotation = Quaternion.Lerp(_currentRotation, _desiredRotation, Time.deltaTime * zoomDampening);
                transform.rotation = _rotation;
            }

            //// Изменение позиции
            // Изменение дистанции зума от вразения колёсика мыши
            _desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate *
                                Mathf.Abs(_desiredDistance);
            // Зажим значения требуемого расстояния
            _desiredDistance = Mathf.Clamp(_desiredDistance, minDistance, maxDistance);
            // Для сглаживания зумирования использование Lerp 
            _currentDistance = Mathf.Lerp(_currentDistance, _desiredDistance, Time.deltaTime * zoomDampening);

            // Подсчитвть позицию на основе нового значения currentDistance
            _position = target.position - (_rotation * Vector3.forward * _currentDistance + targetOffset);
            transform.position = _position;
        }

        private static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
            return Mathf.Clamp(angle, min, max);
        }
    }
}