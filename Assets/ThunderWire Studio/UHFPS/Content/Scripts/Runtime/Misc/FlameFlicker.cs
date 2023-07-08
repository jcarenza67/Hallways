using UnityEngine;
using ThunderWire.Attributes;

namespace UHFPS.Runtime
{
    [InspectorHeader("Flame Flicker")]
    public class FlameFlicker : MonoBehaviour
    {
        public Light FlameLight;
        public MinMax FlameFlickerLimits;
        public float FlameFlickerSpeed = 1f;

        private void Update()
        {
            if (!FlameLight.enabled)
                return;

            float flicker = Mathf.PerlinNoise(Time.time * FlameFlickerSpeed, 0);
            FlameLight.intensity = Mathf.Lerp(FlameFlickerLimits.RealMin, FlameFlickerLimits.RealMax, flicker);
        }
    }
}