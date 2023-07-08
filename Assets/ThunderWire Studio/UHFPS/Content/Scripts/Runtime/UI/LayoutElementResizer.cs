using UnityEngine;
using UnityEngine.UI;

namespace UHFPS.Runtime
{
    public class LayoutElementResizer : LayoutElement
    {
        public bool AutoResizeWidth;
        public bool AutoResizeHeight;

        public float WidthPadding;
        public float HeightPadding;

        public override float preferredWidth 
        { 
            get
            {
                if (AutoResizeWidth) 
                {
                    RectTransform rectTransform = transform.GetChild(0) as RectTransform;
                    return LayoutUtility.GetPreferredWidth(rectTransform) + WidthPadding;
                }

                return base.preferredWidth;
            }
            set => base.preferredWidth = value;
        }

        public override float preferredHeight 
        {
            get
            {
                if (AutoResizeHeight)
                {
                    RectTransform rectTransform = transform.GetChild(0) as RectTransform;
                    return LayoutUtility.GetPreferredHeight(rectTransform) + HeightPadding;
                }

                return base.preferredHeight;
            }
            set => base.preferredHeight = value;
        }
    }
}