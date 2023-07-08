using UnityEngine;
using UnityEngine.UI;

namespace UHFPS.Runtime
{
    public class FloatingIcon : MonoBehaviour
    {
        private float fadeTime;
        private Image iconImage;

        private float targetFade = -1f;
        private float fadeVelocity;

        private void Awake()
        {
            iconImage = GetComponent<Image>();
            targetFade = -1f;
        }

        public void FadeIn(float fadeTime)
        {
            if (!iconImage) return;

            this.fadeTime = fadeTime;
            targetFade = 1f;

            Color color = iconImage.color;
            color.a = 0f;
            iconImage.color = color;
        }

        public void FadeOut(float fadeTime)
        {
            this.fadeTime = fadeTime;
            targetFade = 0f;
        }

        private void Update()
        {
            if (targetFade >= 0f && iconImage)
            {
                Color color = iconImage.color;
                color.a = Mathf.SmoothDamp(color.a, targetFade, ref fadeVelocity, fadeTime);
                iconImage.color = color;

                if (color.a < 0.01f && targetFade == 0f)
                    Destroy(gameObject);
            }
        }
    }
}