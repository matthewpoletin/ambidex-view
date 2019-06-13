using Client.Scripts.Service.Model;
using UnityEngine;

namespace Client.Scripts.Core.Locus
{
    [RequireComponent(typeof(SphereCollider))]
    public class LocusPointController : MonoBehaviour
    {
        public LocusPointData data;

        private Material _material;

        private void Start()
        {
            _material = GetComponent<Renderer>().material;
        }

        private void OnMouseDown()
        {
            if (ModelManager.Instance.Mode != ApplicationMode.Locus)
                return;

            LocusManager.Instance.SelectLocusPoint(this);
        }

        public void Initialize(LocusPointData pointData)
        {
            data = pointData;
        }

        public void Select()
        {
            _material.SetFloat("_IsEnabled", 1f);
        }

        public void Deselect()
        {
            _material.SetFloat("_IsEnabled", 0f);
        }
    }
}