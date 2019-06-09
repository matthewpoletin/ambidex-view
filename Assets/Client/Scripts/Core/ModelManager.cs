using System.IO;
using Client.Scripts.Robot;
using Client.Scripts.Robot.Parts.Kinematics;
using Client.Scripts.Ui.Editors;
using Client.Scripts.Ui.Logging.View;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.Core
{
    /// <summary>
    /// Current state of application
    /// </summary>
    public enum ApplicationMode
    {
        // No model is loaded
        Unloaded,

        // Design edit mode
        Construction,

        // Edit waypoint path mode
        Waypoints,

        // Application is simulating design
        Simulation,
    }

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

        private ApplicationMode _mode;

        public ApplicationMode Mode
        {
            get => _mode;
            set
            {
                // Previous mode
                switch (_mode)
                {
                    case ApplicationMode.Construction:
                        PartSelection.Instance.UnselectObject();
                        break;
                    case ApplicationMode.Simulation:
                        SimulationEnabled = false;
                        break;
                }

                // New modes
                editModeButtons.SetActive(value == ApplicationMode.Construction);
                waypointsModeButtons.SetActive(value == ApplicationMode.Waypoints);
                simulateModeButtons.SetActive(value == ApplicationMode.Simulation);
                closeButton.gameObject.SetActive(value != ApplicationMode.Unloaded);
                editButton.interactable = value != ApplicationMode.Construction;
                waypointsButton.interactable = value != ApplicationMode.Waypoints;
                simulateButton.interactable = value != ApplicationMode.Simulation;

                _mode = value;
            }
        }

        private float _movementSpeed = 40f;

        private bool _simulationEnabled = false;

        // Simulation is running
        public bool SimulationEnabled
        {
            get
            {
                if (Mode != ApplicationMode.Simulation)
                    return false;

                return _simulationEnabled;
            }
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
            designsButton.onClick.AddListener(OnDesignsButtonClick);
            editButton.onClick.AddListener(OnEditButtonClick);
            waypointsButton.onClick.AddListener(OnWaypointsButtonClick);
            simulateButton.onClick.AddListener(OnSimulateButtonClick);
            addButton.onClick.AddListener(OnAddButtonClick);
            saveButton.onClick.AddListener(OnSaveButtonClick);
            playPauseButton.onClick.AddListener(OnPlayPauseButtonClick);
            restartButton.onClick.AddListener(OnRestartButtonClick);
            closeButton.onClick.AddListener(OnCloseButtonClick);

            SimulationComplete = false;

            UpdatePlayPauseImage();

            UnloadDesign();
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
        public Button designsButton;

        public Button editButton;
        public Button waypointsButton;
        public Button simulateButton;

        public Button addButton;
        public Button saveButton;

        public Button restartButton;
        public Button playPauseButton;

        public Button closeButton;

        public GameObject editModeButtons;
        public GameObject waypointsModeButtons;
        public GameObject simulateModeButtons;

        private void OnDesignsButtonClick()
        {
            InfoPanelController.Instance.ToggleDesignBrowser();
        }

        private void OnEditButtonClick()
        {
            Mode = ApplicationMode.Construction;
        }

        private void OnWaypointsButtonClick()
        {
            Mode = ApplicationMode.Waypoints;
        }

        private void OnSimulateButtonClick()
        {
            Mode = ApplicationMode.Simulation;
        }

        private void OnAddButtonClick()
        {
        }

        private void OnSaveButtonClick()
        {
        }

        private void OnPlayPauseButtonClick()
        {
            // Switch state
            SimulationEnabled = !SimulationEnabled;
            // Change button image
            UpdatePlayPauseImage();
        }

        private void OnRestartButtonClick()
        {
            SimulationEnabled = false;
            RobotController.Instance.Rebuild();
        }

        private void OnCloseButtonClick()
        {
            InfoPanelController.Instance.HideAll();
            UnloadDesign();
        }

        #endregion

        public Text titleText;

        public GameObject modeHolder;
        public Text modeText;

        public void UpdatePlayPauseImage()
        {
            if (SimulationEnabled)
                playPauseButtonImage.sprite = pauseSprite;
            else
                playPauseButtonImage.sprite = playSprite;
        }

        public void LoadDesign(string fileName)
        {
            RobotController.Instance.BuildFromFile(fileName);
            titleText.gameObject.SetActive(true);
            titleText.text = Path.GetFileName(fileName);
            Mode = ApplicationMode.Construction;
        }

        public void UnloadDesign()
        {
            titleText.text = "";
            titleText.gameObject.SetActive(false);
            RobotController.Instance.Unload();
            WaypointManager.Instance.Unload();
            Mode = ApplicationMode.Unloaded;
        }
    }
}