using System;
using Client.Scripts.Service.Model;
using UnityEngine;

namespace Client.Scripts.Robot.Parts.Common
{
    public class Beam : MonoBehaviour, IPart
    {
        private Guid _id;

        private float _length;
        private float _diameter;

        public void Select()
        {
            transform.Find("Mesh").GetComponent<MeshRenderer>().material.SetFloat("_IsEnabled", 1f);
        }

        public void Deselect()
        {
            transform.Find("Mesh").GetComponent<MeshRenderer>().material.SetFloat("_IsEnabled", 0f);
        }

        public void Deserialize(PartData data)
        {
            _id = data.Id;
            _length = data.Length;
            _diameter = data.Diameter;
        }

        public PartData Serialize()
        {
            return new PartData
            {
                Id = _id,
                Type = "Beam",
                Length = 1f,
                Diameter = 1f,
            };
        }
    }
}