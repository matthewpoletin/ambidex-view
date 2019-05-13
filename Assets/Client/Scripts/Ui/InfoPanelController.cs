using Client.Scripts.Robot.Kinematics;
using UnityEngine;

namespace Client.Scripts
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

        public GameObject rotaryJointEditorGo;

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

            rotaryJointEditorGo.SetActive(true);
            rotaryJointEditorGo.GetComponent<RotaryJointEditorController>().Activate(rotaryJoint);
        }

        public void HideRotaryJointEditor()
        {
            rotaryJointEditorGo.GetComponent<RotaryJointEditorController>().Deactivate();
            HideAll();
        }
    }
}