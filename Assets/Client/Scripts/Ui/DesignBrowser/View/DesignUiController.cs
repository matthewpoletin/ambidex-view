using Client.Scripts.Core;
using Client.Scripts.Ui.DesignBrowser.Model;
using Client.Scripts.Ui.Editors;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.Ui.DesignBrowser.View
{
    public class DesignUiController : MonoBehaviour
    {
        public Image previewImage;
        public Text nameText;
        public Text createDateText;
        public Button button;

        public void Initialize(DesignData data)
        {
            // TODO: Load preview image
//            previewImage.sprite = Sprite.Create(new Texture2D(), );

            nameText.text = data.Name;
            createDateText.text = data.CreateDate.ToString("dd/MM/yy");

            button.onClick.AddListener(() =>
            {
                ModelManager.Instance.LoadDesign(data.FileName);
                InfoPanelController.Instance.HideDesignBrowser();
            });

            Destroy(this);
        }
    }
}