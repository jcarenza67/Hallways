using UnityEngine;
using UHFPS.Runtime;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UHFPS.Tools
{
    public static class HandlesDrawing
    {
        public static void DrawLimits(Vector3 position, MinMax limits, Vector3 forward, Vector3 upward, bool bothSides = true, bool flip = false, float radius = 1f)
        {
#if UNITY_EDITOR
            Vector3 from = Quaternion.AngleAxis(bothSides || !flip ? limits.min : limits.min - limits.min, upward) * forward;

            float angle = bothSides
                ? limits.max - limits.min % 360f
                : !flip ? -limits.min % 360 : limits.max % 360;

            Handles.color = Color.white;
            Handles.DrawWireArc(position, upward, from, angle, radius);

            Handles.color = Color.white.Alpha(0.1f);
            Handles.DrawSolidArc(position, upward, from, angle, radius);
#endif
        }
    }
}