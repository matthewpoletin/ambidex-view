using System;
using System.IO;
using System.Linq;
using Client.Scripts.Robot.Parts.Kinematics;
using Client.Scripts.Ui.DesignBrowser.Model;
using Client.Scripts.Ui.DesignBrowser.View;
using UnityEngine;

namespace Client.Scripts.Ui.Editors
{
    public class InfoPanelController : MonoBehaviour
    {
        #region Singleton

        public static InfoPanelController Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }

        #endregion

        public Transform contentHolder;

        private void Start()
        {
            HideAll();
        }

        public RotaryJointEditor rotaryJointEditor;
        public RevoluteJointEditor revoluteJointEditor;
        public DesignBrowserUiController designBrowser;

        public void HideAll()
        {
            foreach (Transform child in contentHolder)
                child.gameObject.SetActive(false);
        }

        public void ShowRotaryJointEditor(RotaryJoint rotaryJoint)
        {
            HideAll();

            rotaryJointEditor.gameObject.SetActive(true);
            rotaryJointEditor.Activate(rotaryJoint);
        }

        public void HideRotaryJointEditor()
        {
            rotaryJointEditor.Deactivate();
            HideAll();
        }

        public void ShowRevoluteJointEditor(RevoluteJoint revoluteJoint)
        {
            HideAll();

            revoluteJointEditor.gameObject.SetActive(true);
            revoluteJointEditor.Activate(revoluteJoint);
        }

        public void HideRevoluteJointEditor()
        {
            revoluteJointEditor.Deactivate();
            HideAll();
        }

        public void ToggleDesignBrowser()
        {
            if (!designBrowser.gameObject.activeSelf)
                ShowDesignBrowser();
            else
                HideDesignBrowser();
        }

        public void ShowDesignBrowser()
        {
            HideAll();

            designBrowser.gameObject.SetActive(true);

            var data = Directory
                .EnumerateFiles(Path.Combine(Application.dataPath, "Client", "Configurations"), "*.json")
                .Select(fileName => new DesignData
                {
                    CreateDate = DateTime.Now,
                    Name = Path.GetFileName(fileName),
                    FileName = fileName,
//                    PreviewImage = 
                });
            designBrowser.Refill(data);
        }

        public void HideDesignBrowser()
        {
            designBrowser.Deactivate();
            designBrowser.gameObject.SetActive(false);
        }
    }
}