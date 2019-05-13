using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts
{
    public class AngleSelectorController : MonoBehaviour
    {
        public Canvas canvas;
        public Image angleBarImage;
        public RectTransform angleBarTransform;
        public RectTransform minButton;
        public RectTransform maxButton;

        private float _minAngle = 0f;

        public float MinAngle
        {
            get => _minAngle;
            set
            {
                _minAngle = Mathf.Clamp(value, 0f, MaxAngle);
                var amount = _minAngle / 360f;
                // TODO: Rotate angleBarTransform
                minButton.localEulerAngles = new Vector3(0, 0, amount * 360f);
            }
        }

        private float _maxAngle = 360f;

        public float MaxAngle
        {
            get => _maxAngle;
            set
            {
                _maxAngle = Mathf.Clamp(value, MinAngle, 360f);
                var amount = _maxAngle / 360f;
                angleBarImage.fillAmount = amount;
                maxButton.localEulerAngles = new Vector3(0, 0, amount * 360f);
            }
        }
    }
}