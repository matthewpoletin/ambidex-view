﻿using Client.Scripts.Robot;
using Client.Scripts.Robot.Parts.Kinematics;
using Client.Scripts.Ui.Logging.View;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.Core
{
    public class ModelManager : MonoBehaviour
    {
        #region Singleton

        public static ModelManager Instance { get; private set; } = null;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }

        #endregion

        private float _movementSpeed = 40f;

        private bool _simulationEnabled = false;

        public bool SimulationEnabled
        {
            get => _simulationEnabled;
            set
            {
                _simulationEnabled = value;
                if (_simulationEnabled)
                {
                    PartSelection.Instance.UnselectObject();
                }

                UpdatePlayPauseImage();
            }
        }


        private bool _simulationComplete = true;

        public bool SimulationComplete
        {
            get => _simulationComplete;
            set
            {
                _simulationComplete = value;
                restartButton.interactable = _simulationComplete;
                if (_simulationComplete)
                {
                    const string result = "successfully";
                    LogController.Instance.WriteInfoLog($"Simulation Complete {result}");
                }
            }
        }

        private void Start()
        {
            restartButton.onClick.AddListener(OnRestartButtonClick);
            playlistButton.onClick.AddListener(OnPlaylistButtonClick);
            previousButton.onClick.AddListener(OnPreviousButtonClick);
            nextButton.onClick.AddListener(OnNextButtonClick);
            playPauseButton.onClick.AddListener(OnPlayPauseButtonClick);

            SimulationComplete = false;

            UpdatePlayPauseImage();
        }

        private void Update()
        {
            if (SimulationEnabled)
            {
                var rotaryJoints = FindObjectsOfType<RotaryJoint>();
                foreach (var joint in rotaryJoints)
                {
                    joint.ChangeAngle(_movementSpeed * Time.deltaTime);
                }
            }
        }

        #region Buttons

        [Space]
        [SerializeField]
        private Image playPauseButtonImage = null;

        public Sprite playSprite;
        public Sprite pauseSprite;

        [Header("Buttons")]
        public Button playlistButton;

        public Button restartButton;
        public Button previousButton;
        public Button nextButton;
        public Button playPauseButton;

        private void OnPlaylistButtonClick()
        {
            // if ()
        }

        public void OnRestartButtonClick()
        {
            SimulationEnabled = false;
            RobotController.Instance.Rebuild();
        }

        public void OnPreviousButtonClick()
        {
            Debug.Log("Previous");
        }

        public void OnNextButtonClick()
        {
            Debug.Log("Next");
        }

        public void OnPlayPauseButtonClick()
        {
            // Switch state
            SimulationEnabled = !SimulationEnabled;
            // Change button image
            UpdatePlayPauseImage();
        }

        #endregion

        public void UpdatePlayPauseImage()
        {
            if (SimulationEnabled)
                playPauseButtonImage.sprite = pauseSprite;
            else
                playPauseButtonImage.sprite = playSprite;
        }
    }
}