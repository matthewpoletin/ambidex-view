using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.Ui.Status.View
{
    public class StatusUiController : MonoBehaviour
    {
        #region Singleton

        public static StatusUiController Instance { get; private set; } = null;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }

        #endregion

        public Text infoText;
        public Image percentageFillImage;
        public Text percentageText;

        public GameObject statusHolder;

        public float Percentage
        {
            set
            {
                var result = Mathf.Clamp(value, 0f, 1f);
                percentageFillImage.fillAmount = result;
                percentageText.text = $"{(result * 100f).ToString(CultureInfo.InvariantCulture)}%";
            }
        }

        private void Start()
        {
            Percentage = 1f;
            Hide();
        }

        public void Show()
        {
            statusHolder.SetActive(true);
        }

        public void Hide()
        {
            statusHolder.SetActive(false);
        }
    }
}