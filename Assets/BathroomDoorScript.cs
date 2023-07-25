using System;
using UnityEngine;

public class BathroomDoorScript : MonoBehaviour
{
    public static Action OnBathroomDoorOpened;
    private Animator myAnimator;
    private bool doorOpen;

    public GameObject monster; // Assign your monster game object in the Inspector

    void Start()
    {
        myAnimator = GetComponent<Animator>();
        if (myAnimator == null)
        {
            myAnimator = GetComponentInParent<Animator>();
        }

        // Initially make the monster invisible and stop the chase
        SkinnedMeshRenderer[] renderers = monster.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach(SkinnedMeshRenderer renderer in renderers)
        {
            renderer.enabled = false;
        }
        monster.GetComponent<MonsterChase>().StopChase();
    }

    public void ObjectClicked()
    {
        float normalizedTime = myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (normalizedTime >= 1.0)
        {
            if (doorOpen)
            {
                myAnimator.Play("Close", 0, 0.0f);
                doorOpen = false;
            }
            else
            {
                myAnimator.Play("Open", 0, 0.0f);
                doorOpen = true;

                OnBathroomDoorOpened?.Invoke();

                // Now make the monster visible and start the chase
                SkinnedMeshRenderer[] renderers = monster.GetComponentsInChildren<SkinnedMeshRenderer>();
                foreach(SkinnedMeshRenderer renderer in renderers)
                {
                    renderer.enabled = true;
                }
                monster.GetComponent<MonsterChase>().StartChase();
            }
        }
    }
}
