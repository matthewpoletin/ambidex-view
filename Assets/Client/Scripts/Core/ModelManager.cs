﻿using System;
using System.Collections.Generic;
using System.IO;
using Client.Scripts.Core.Locus;
using Client.Scripts.Robot;
using Client.Scripts.Robot.Parts.Kinematics;
using Client.Scripts.Service;
using Client.Scripts.Service.Model;
using Client.Scripts.Ui.Editors;
using Client.Scripts.Ui.Logging.View;
using Client.Scripts.Ui.Status.View;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.Core
{
    public class DesignInfo
    {
        public Guid Id;
        public string Name;
        public string Description;
        public string Author;
        public DateTime CreateDate;
    }

    /// <summary>
    /// Current state of application
    /// </summary>
    public enum ApplicationMode
    {
        // No model is loaded
        Unloaded,

        // Design edit mode
        Construction,

        // Preview of locus points
        Locus,

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
                    case ApplicationMode.Locus:
                        LocusManager.Instance.Hide();
                        IsLocusShown = false;
                        break;
                    case ApplicationMode.Waypoints:
                        WaypointManager.Instance.UnselectWaypoint();
                        break;
                    case ApplicationMode.Simulation:
                        SimulationEnabled = false;
                        break;
                }

                InfoPanelController.Instance.HideAll();

                // New modes
                editModeButtons.SetActive(value == ApplicationMode.Construction);
                locusModeButtons.SetActive(value == ApplicationMode.Locus);
                waypointsModeButtons.SetActive(value == ApplicationMode.Waypoints);
                simulateModeButtons.SetActive(value == ApplicationMode.Simulation);
                closeButton.gameObject.SetActive(value != ApplicationMode.Unloaded);
                editButton.interactable = value != ApplicationMode.Construction;
                locusButton.interactable = value != ApplicationMode.Locus;
                waypointsButton.interactable = value != ApplicationMode.Waypoints;
                simulateButton.interactable = value != ApplicationMode.Simulation;

                WaypointManager.Instance.WaypointsShown =
                    value == ApplicationMode.Waypoints || value == ApplicationMode.Simulation;

                switch (value)
                {
                    case ApplicationMode.Unloaded:
                        modeHolder.SetActive(false);
                        modeText.text = "";
                        modeBackground.color = Color.white;
                        break;
                    case ApplicationMode.Construction:
                        modeHolder.SetActive(true);
                        modeText.text = "Construction";
                        modeBackground.color = constructionModeColor;
                        break;
                    case ApplicationMode.Locus:
                        modeHolder.SetActive(true);
                        modeText.text = "Locus";
                        modeBackground.color = locusModeColor;
                        break;
                    case ApplicationMode.Waypoints:
                        modeHolder.SetActive(true);
                        modeText.text = "Waypoints";
                        modeBackground.color = waypointsModeColor;
                        break;
                    case ApplicationMode.Simulation:
                        modeHolder.SetActive(true);
                        modeText.text = "Simulation";
                        modeBackground.color = simulationModeColor;
                        break;
                }

                _mode = value;
            }
        }

        private string _designFileName = null;

        private DesignInfo _designData = null;

        [HideInInspector]
        public Queue<SimulationStep> simulationProcess = null;

        // Simulation is loaded
        private bool SimulationLoaded => simulationProcess != null;

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

        // Simulation is over (timeout or all steps done)
        private bool _simulationComplete = false;

        public bool SimulationComplete
        {
            get
            {
                if (simulationProcess.Count == 0)
                    _simulationComplete = true;

                return _simulationComplete;
            }
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

        // TODO: Move into LocusManager
        private bool _isLocusShown = false;

        public bool IsLocusShown
        {
            get => _isLocusShown;
            set
            {
                _isLocusShown = value;
                if (_isLocusShown)
                {
                    var design = SerializeDesign();
                    if (design == null)
                        return;
                    CoreService.UploadDesignAndLoadLocus(design);
                }
                else
                {
                    LocusManager.Instance.Hide();
                }
            }
        }

        private void Start()
        {
            designsButton.onClick.AddListener(OnDesignsButtonClick);
            editButton.onClick.AddListener(OnEditButtonClick);
            locusButton.onClick.AddListener(OnLocusButtonClick);
            waypointsButton.onClick.AddListener(OnWaypointsButtonClick);
            simulateButton.onClick.AddListener(OnSimulateButtonClick);

            addPartButton.onClick.AddListener(OnAddPartButtonClick);
            saveDesignButton.onClick.AddListener(OnSaveDesignButtonClick);
            syncLocusButton.onClick.AddListener(OnSyncLocusButtonClick);
            addWaypointButton.onClick.AddListener(OnAddWaypointButtonClick);
            saveWaypointsButton.onClick.AddListener(OnSaveWaypointsButtonClick);
            undoWaypointsButton.onClick.AddListener(OnUndoWaypointsButtonClick);
            uploadButton.onClick.AddListener(OnUploadButtonClick);
            playPauseButton.onClick.AddListener(OnPlayPauseButtonClick);
            restartButton.onClick.AddListener(OnRestartButtonClick);

            closeButton.onClick.AddListener(OnCloseButtonClick);

            SimulationComplete = false;

            IsLocusShown = false;

            UpdatePlayPauseImage();

            UnloadDesign();
        }

        private void Update()
        {
            // We run simulation, itf is loaded and not over yet
            if (SimulationEnabled && SimulationLoaded && !SimulationComplete)
            {
                var step = simulationProcess.Dequeue();
                foreach (var item in step.Configuration)
                {
                    var itemData = RobotController.Instance.GetItemById(item.Id);
                    if (itemData == null)
                    {
                        Debug.LogWarning($"Item {item.Id} not found");
                        continue;
                    }

                    switch (itemData.Type)
                    {
                        case ItemType.RotaryJoint:
                            itemData.GameObject.GetComponent<RotaryJoint>().SetAngle(item.CurrentAngle);
                            break;
                    }
                }
            }
        }

        #region Interface

        [Space]
        [SerializeField]
        private Image playPauseButtonImage = null;

        public Sprite playSprite;
        public Sprite pauseSprite;

        [Header("Buttons")]
        public Button designsButton;

        public Button editButton;
        public Button locusButton;
        public Button waypointsButton;
        public Button simulateButton;

        public Button addPartButton;
        public Button saveDesignButton;

        public Button syncLocusButton;

        public Button addWaypointButton;
        public Button saveWaypointsButton;
        public Button undoWaypointsButton;

        public Button uploadButton;
        public Button playPauseButton;
        public Button restartButton;

        public Button closeButton;

        public GameObject editModeButtons;
        public GameObject locusModeButtons;
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

        private void OnLocusButtonClick()
        {
            Mode = ApplicationMode.Locus;
        }

        private void OnWaypointsButtonClick()
        {
            Mode = ApplicationMode.Waypoints;
        }

        private void OnSimulateButtonClick()
        {
            Mode = ApplicationMode.Simulation;
        }

        private void OnAddPartButtonClick()
        {
        }

        private void OnSaveDesignButtonClick()
        {
            SaveDesign();
        }

        private void OnSyncLocusButtonClick()
        {
            // TODO: Implement
            IsLocusShown = true;
        }

        private void OnAddWaypointButtonClick()
        {
            var newWaypoint = WaypointManager.Instance.CreateWaypoint();
            InfoPanelController.Instance.ShowWaypointEditor(newWaypoint);
        }

        private void OnSaveWaypointsButtonClick()
        {
            CoreService.SyncWaypointPath();
        }

        private void OnUndoWaypointsButtonClick()
        {
            // TODO: Undo waypoints changes
        }

        private void OnUploadButtonClick()
        {
            // TODO: Create and make simulation request
        }

        private void OnPlayPauseButtonClick()
        {
            // Switch state
            SimulationEnabled = !SimulationEnabled;
            // Change button image
            UpdatePlayPauseImage();

            // We need to run simulation and it's not loaded
            if (SimulationEnabled && !SimulationLoaded)
                CoreService.Simulate();
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

        public Text titleText;

        public GameObject modeHolder;
        public Image modeBackground;
        public Text modeText;
        public Color constructionModeColor = Color.white;
        public Color locusModeColor = Color.white;
        public Color waypointsModeColor = Color.white;
        public Color simulationModeColor = Color.white;

        #endregion

        public void UpdatePlayPauseImage()
        {
            playPauseButtonImage.sprite = SimulationEnabled ? pauseSprite : playSprite;
        }

        #region Serialization

        /// <summary>
        /// Load design from file
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns>Loading result</returns>
        public bool LoadDesign(string fileName)
        {
            // Store file name for later saving
            _designFileName = fileName;

            // Load file
            if (!File.Exists(fileName))
                return false;

            var data = JsonConvert.DeserializeObject<Design>(File.ReadAllText(fileName));
            if (data == null)
                return false;

            // Store design data
            _designData = new DesignInfo
            {
                Id = data.Id,
                Name = data.Name,
                Description = data.Description,
                Author = data.Author,
                CreateDate = data.CreateDate,
            };

            // Initialize robot data
            if (!RobotController.Instance.BuildFromFile(fileName))
                return false;

            // Initialize waypoint data
            if (!WaypointManager.Instance.Load(data.WaypointsConfiguration.Waypoints))
                return false;

            // Update UI
            titleText.gameObject.SetActive(true);
            titleText.text = Path.GetFileName(fileName);
            Mode = ApplicationMode.Construction;
            SimulationComplete = false;
            SimulationEnabled = false;

            return true;
        }

        /// <summary>
        /// Save design
        /// </summary>
        /// <returns>Save result</returns>
        private bool SaveDesign()
        {
            StatusUiController.Instance.Percentage = 0f;
            StatusUiController.Instance.Show();

            // Check if fail name stored
            if (_designFileName == null)
                return false;

            // Serialize design
            var design = SerializeDesign();
            if (design == null)
                return false;

            // Serialize JSON to a string and then write string to a file
            File.WriteAllText(_designFileName, JsonConvert.SerializeObject(design, Formatting.Indented));

            StatusUiController.Instance.Percentage = 1f;

            return true;
        }

        private Design SerializeDesign()
        {
            // Check if fail name stored
            if (_designFileName == null)
                return null;

            // Serialize design
            if (_designData == null)
                return null;

            var design = new Design
            {
                Id = _designData.Id,
                Name = _designData.Name,
                Description = _designData.Description,
                Author = _designData.Author,
                CreateDate = _designData.CreateDate,
                RobotConfiguration = RobotController.Instance.Serialize(),
                WaypointsConfiguration = WaypointManager.Instance.Serialize(),
            };

            return design;
        }

        #endregion

        public void UnloadDesign()
        {
            // Unload design
            RobotController.Instance.Unload();
            WaypointManager.Instance.Unload();
            _designFileName = null;
            _designData = null;
            // Update UI
            titleText.text = "";
            titleText.gameObject.SetActive(false);
            Mode = ApplicationMode.Unloaded;
        }
    }
}