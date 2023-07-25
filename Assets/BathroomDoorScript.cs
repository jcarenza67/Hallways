using System;
using UnityEngine;

public class BathroomDoorScript : MonoBehaviour
{
    public static Action OnBathroomDoorOpened;
    private Animator myAnimator;
    private bool doorOpen;

    void Start()
    {
        myAnimator = GetComponent<Animator>();
        if (myAnimator == null)
        {
            myAnimator = GetComponentInParent<Animator>();
        }
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
            }
        }
    }
}

