using UnityEngine;

namespace Client.Scripts.Core
{
    public class TimeManager : MonoBehaviour
    {
        #region Singleton

        public static TimeManager Instance { get; private set; } = null;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }

        #endregion

        private bool _isPaused;

        public bool IsPaused
        {
            get => _isPaused;
            set
            {
                _isPaused = value;
                if (_isPaused)
                    Pause();
                else
                    Unpause();
            }
        }

        public void Unpause()
        {
            Time.timeScale = 1f;
            _isPaused = false;
        }

        public void Pause()
        {
            Time.timeScale = 0f;
            _isPaused = true;
        }

        public void TogglePause()
        {
            IsPaused = !IsPaused;
        }
    }
}