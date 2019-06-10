using System;
using Client.Scripts.Service.Model;
using UnityEngine;

namespace Client.Scripts.Robot.Parts.Common.Tips
{
    public class BasicTip : MonoBehaviour, IPart
    {
        private float _rotationY;

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

        public void Deserialize(PartData data)
        {
            throw new NotImplementedException();
        }

        public PartData Serialize()
        {
            throw new NotImplementedException();
        }
    }
}