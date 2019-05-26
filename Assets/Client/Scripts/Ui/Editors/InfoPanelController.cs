using Client.Scripts.Robot.Parts.Kinematics;
using UnityEngine;

namespace Client.Scripts.Ui.Editors
{
    public class InfoPanelController : MonoBehaviour
    {
        #region Singleton

        public static InfoPanelController Instance { get; private set; } = null;

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

        private void HideAll()
        {
            foreach (Transform child in contentHolder)
            {
                child.gameObject.SetActive(false);
            }
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
    }
}