using UnityEngine;

namespace Client.Scripts.Robot.Parts.Common
{
    public class Beam : MonoBehaviour, ISelectablePart
    {
        public void Select()
        {
            transform.Find("Mesh").GetComponent<MeshRenderer>().material.SetFloat("_IsEnabled", 1f);
        }

        public void Deselect()
        {
            transform.Find("Mesh").GetComponent<MeshRenderer>().material.SetFloat("_IsEnabled", 0f);
        }
    }
}