using System;
using Client.Scripts.Ui.Logging.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.Ui.Logging.View
{
    public class LogEntryController : MonoBehaviour
    {
        [SerializeField]
        private Text infoText = null;

        private void Start()
        {
            if (!infoText)
                Debug.LogWarning("Info text object is not setup");
        }

        public void Initialize(LogEntryData data)
        {
            // Fill with information
            infoText.text = data.Text;

            switch (data.Type)
            {
                case LogEntryType.Info:
                    infoText.color = Color.blue;
                    break;
                case LogEntryType.Warning:
                    infoText.color = Color.yellow;
                    break;
                case LogEntryType.Error:
                    infoText.color = Color.red;
                    break;
                case LogEntryType.Undefined:
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Set on top of log list
            transform.SetSiblingIndex(0);

            // Delete script
            Destroy(this);
        }
    }
}