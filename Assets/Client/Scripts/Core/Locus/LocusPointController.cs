using Client.Scripts.Service.Model;
using TMPro;
using UnityEngine;

namespace Client.Scripts.Core.Locus
{
    [RequireComponent(typeof(SphereCollider))]
    public class LocusPointController : MonoBehaviour
    {
        public LocusPointData data;

        private Material _material;

        public Canvas canvas;
        public TextMeshProUGUI text;

        private void Start()
        {
            _material = GetComponent<Renderer>().material;
            Deselect();
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

            var res = $"{data.Position.X:0.0} {data.Position.Y:0.0} {data.Position.Z:0.0}";

            foreach (var partData in data.Configuration)
            {
                switch (partData.Type)
                {
                    case "RotaryJoint":
                        res += $"\nRJ: {partData.CurrentAngle}";
                        break;
                }
            }

            text.text = res;
        }

        public void Select()
        {
            canvas.gameObject.SetActive(true);
            _material.SetFloat("_IsEnabled", 1f);
        }

        public void Deselect()
        {
            canvas.gameObject.SetActive(false);
            _material.SetFloat("_IsEnabled", 0f);
        }

        private void Update()
        {
            canvas.transform.LookAt(Camera.main.transform);
            canvas.transform.Rotate(0f, 180f, 0f);
        }
    }
}