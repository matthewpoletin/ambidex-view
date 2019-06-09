using System.Collections.Generic;
using Client.Scripts.Ui.DesignBrowser.Model;
using UnityEngine;

namespace Client.Scripts.Ui.DesignBrowser.View
{
    public class DesignBrowserUiController : MonoBehaviour
    {
        public GameObject designPrefab;

        public Transform designHolder;

        public void Refill(IEnumerable<DesignData> data)
        {
            Deactivate();

            foreach (var dataItem in data)
            {
                var design = Instantiate(designPrefab, designHolder);
                design.GetComponent<DesignUiController>().Initialize(dataItem);
            }
        }

        public void Deactivate()
        {
            foreach (Transform child in designHolder)
                Destroy(child.gameObject);
        }
    }
}