using System;
using Client.Scripts.Ui.Logging.Model;
using UnityEngine;

namespace Client.Scripts.Ui.Logging.View
{
    public class LogController : MonoBehaviour
    {
        #region Singleton

        public static LogController Instance { get; private set; } = null;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }

        #endregion

        // Объект для помещения логов
        [SerializeField]
        private Transform logHolder = null;

        // Префаб записи лога
        [SerializeField]
        private GameObject logEntryPrefab = null;

        private Animator _animator;
        private static readonly int IsShown = Animator.StringToHash("IsShown");

        private void Start()
        {
            _animator = GetComponent<Animator>();

            if (!logHolder)
                Debug.LogWarning("");
        }

        private void Update()
        {
            _animator.SetBool(IsShown, Screen.width * 0.9 < Input.mousePosition.x);
        }

        private void WriteLog(LogEntryType type, string text)
        {
            var data = new LogEntryData
            {
                Type = type,
                Text = text,
                CreateTime = DateTime.Now,
            };
            var logEntryGo = Instantiate(logEntryPrefab, logHolder);
            logEntryGo.GetComponent<LogEntryController>().Initialize(data);
        }

        public void WriteInfoLog(string text)
        {
            WriteLog(LogEntryType.Info, text);
        }

        public void WriteWarningLog(string text)
        {
            WriteLog(LogEntryType.Warning, text);
        }

        public void WriteErrorLog(string text)
        {
            WriteLog(LogEntryType.Error, text);
        }
    }
}