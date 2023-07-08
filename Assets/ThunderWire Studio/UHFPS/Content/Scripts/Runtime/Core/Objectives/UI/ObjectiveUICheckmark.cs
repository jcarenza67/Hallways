using UnityEngine;
using UnityEngine.UI;

namespace UHFPS.Runtime
{
    [RequireComponent(typeof(Image))]
    public class ObjectiveUICheckmark : MonoBehaviour
    {
        public Sprite NormalSprite;
        public Sprite CheckedSprite;

        private Image image;
        private bool isChecked;

        public Image Checkmark
        {
            get
            {
                if (image == null) image = GetComponent<Image>();
                return image;
            }
        }

        public bool Checked
        {
            get => isChecked;
            set
            {
                Checkmark.sprite = value ? CheckedSprite : NormalSprite;
                isChecked = value;
            }
        }
    }
}