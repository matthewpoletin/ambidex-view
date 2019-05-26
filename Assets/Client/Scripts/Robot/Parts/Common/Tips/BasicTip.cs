using UnityEngine;

namespace Client.Scripts.Robot.Parts.Common.Tips
{
    public class BasicTip : MonoBehaviour, ISelectablePart
    {
        public void Select()
        {
            foreach (Transform child in transform.Find("Mesh"))
                child.GetComponent<MeshRenderer>().material.SetFloat("_IsEnabled", 1f);
        }

        public void Deselect()
        {
            foreach (Transform child in transform.Find("Mesh"))
                child.GetComponent<MeshRenderer>().material.SetFloat("_IsEnabled", 0f);
        }
    }
}