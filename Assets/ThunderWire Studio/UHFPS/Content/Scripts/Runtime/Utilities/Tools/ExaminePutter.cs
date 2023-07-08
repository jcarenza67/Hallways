using UnityEngine;
using UHFPS.Tools;

namespace UHFPS.Runtime
{
    public class ExaminePutter : MonoBehaviour
    {
        public sealed class PutCurve
        {
            private readonly AnimationCurve curve;
            public float evalMultiply = 1f;
            public float curveTime = 0.1f;

            public PutCurve(AnimationCurve curve)
            {
                this.curve = curve;
            }

            public float Eval(float time) => curve.Evaluate(time) * evalMultiply;
        }

        public struct PutSettings
        {
            public Vector3 putPosition;
            public Quaternion putRotation;
            public Vector3 putControl;
            public PutCurve putPositionCurve;
            public PutCurve putRotationCurve;
            public bool isLocalSpace;

            public PutSettings(Transform tr, Vector3 controlOffset, PutCurve posCurve, PutCurve rotCurve, bool isLocalSpace)
            {
                putPosition = isLocalSpace ? tr.localPosition : tr.position;
                putRotation = isLocalSpace ? tr.localRotation : tr.rotation;
                putControl = isLocalSpace ? tr.localPosition + controlOffset : tr.position + controlOffset;
                putPositionCurve = posCurve;
                putRotationCurve = rotCurve;
                this.isLocalSpace = isLocalSpace;
            }
        }

        private PutSettings putSettings;
        private Vector3 putStartPos;
        private Quaternion putStartRot;
        private bool putStarted;

        private float putPosT;
        private float putPosVelocity;

        private float putRotT;
        private float putRotVelocity;

        public void Put(PutSettings putSettings)
        {
            this.putSettings = putSettings;
            putStartPos = putSettings.isLocalSpace ? transform.localPosition : transform.position;
            putStartRot = putSettings.isLocalSpace ? transform.localRotation : transform.rotation;
            putStarted = true;
        }

        private void Update()
        {
            if (!putStarted) return;

            float putPosCurve = putSettings.putPositionCurve.Eval(putPosT);
            putPosT = Mathf.SmoothDamp(putPosT, 1f, ref putPosVelocity, putSettings.putPositionCurve.curveTime + putPosCurve);

            if(!putSettings.isLocalSpace) transform.position = VectorE.QuadraticBezier(putStartPos, putSettings.putPosition, putSettings.putControl, putPosT);
            else transform.localPosition = VectorE.QuadraticBezier(putStartPos, putSettings.putPosition, putSettings.putControl, putPosT);

            float putRotCurve = putSettings.putRotationCurve.Eval(putRotT);
            putRotT = Mathf.SmoothDamp(putRotT, 1f, ref putRotVelocity, putSettings.putRotationCurve.curveTime + putRotCurve);

            if (!putSettings.isLocalSpace) transform.rotation = Quaternion.Slerp(putStartRot, putSettings.putRotation, putRotT);
            else transform.localRotation = Quaternion.Slerp(putStartRot, putSettings.putRotation, putRotT);

            if ((putPosT * putRotT) >= 0.99f)
            {
                if (!putSettings.isLocalSpace)
                {
                    transform.SetPositionAndRotation(putSettings.putPosition, putSettings.putRotation);
                }
                else
                {
                    transform.localPosition = putSettings.putPosition;
                    transform.localRotation = putSettings.putRotation;
                }

                Destroy(this);
            }
        }
    }
}