using System.Collections;
using UnityEngine;
using TMPro;

namespace UHFPS.Runtime
{
    public class SubObjectivePanel : MonoBehaviour
    {
        public Animator Animator;
        public ObjectiveUICheckmark Checkmark;
        public TMP_Text ObjectiveText;

        [Header("Colors")]
        public Color NormalColor = Color.white;
        public Color CompletedColor = Color.green;

        [Header("Animations")]
        public string CompleteAnimation = "Complete";
        public string HideAnimation = "Hide";

        public IEnumerator CompleteObjective()
        {
            Checkmark.Checked = true;
            Checkmark.Checkmark.color = CompletedColor;
            ObjectiveText.color = CompletedColor;

            Animator.SetTrigger(CompleteAnimation);
            yield return new WaitForAnimatorClip(Animator, CompleteAnimation);
        }

        public IEnumerator HideObjective()
        {
            Animator.SetTrigger(HideAnimation);
            yield return new WaitForAnimatorClip(Animator, HideAnimation);
        }
    }
}