using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UHFPS.Runtime;

public class SimpleOpenClose : MonoBehaviour, IOpenable
{
    private Animator myAnimator;
    private Animator additionalAnimator;
    public bool objectOpen;
    public bool objectOpenAdditional;
    public GameObject animateAdditional;
    private bool hasAdditional = false;
    float myNormalizedTime;

    // Open or close animator state in start depending on selection.
    // Additional object with animator. For example another door when double doors. 
    void Start()
    {
        // If there is no animator in the gameobject itself, get the parent animator.
        myAnimator = GetComponent<Animator>();
        if (myAnimator == null)
        {
            myAnimator = GetComponentInParent<Animator>();
        }

        if (objectOpen == true)
        {
            StartOpening();
        }
        if (animateAdditional != null)
            if (animateAdditional.GetComponent<SimpleOpenClose>())
            {
                additionalAnimator = animateAdditional.GetComponent<Animator>();
                hasAdditional = true;
                objectOpenAdditional = animateAdditional.GetComponent<SimpleOpenClose>().objectOpen;
            }
        else
            {
                hasAdditional = false;
            }
    }

    // Player clicks object. Method called from SimplePlayerUse script.
    public void OpenOrClose()
    {
        ObjectClicked();
    }

    public void ObjectClicked()
    {
        myNormalizedTime = myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (hasAdditional == false)
        {
            if (myNormalizedTime >= 1.0)
            {
                if (objectOpen == true)
                {
                    StopOpening();
                }
                else
                {
                    StartOpening();
                }
            }
        }

        if (hasAdditional == true && myNormalizedTime >= 1.0)
        {
            if (objectOpen == true)
            {
                StopOpening();
                animateAdditional.GetComponent<SimpleOpenClose>().StopOpening();

                if (objectOpenAdditional == true)
                {
                    additionalAnimator.Play("Close", 0, 0.0f);
                    objectOpenAdditional = false;
                    animateAdditional.GetComponent<SimpleOpenClose>().StopOpening();
                }
            }
            else
            {
                StartOpening();
                animateAdditional.GetComponent<SimpleOpenClose>().StartOpening();

                if (objectOpenAdditional == false)
                {
                    additionalAnimator.Play("Open", 0, 0.0f);
                    objectOpenAdditional = true;
                    animateAdditional.GetComponent<SimpleOpenClose>().StartOpening();
                }
            }
        }
    }

    public void StartOpening()
    {
        myAnimator.Play("Open", 0, 0.0f);
        objectOpen = true;
    }

    public void UpdateOpening(float intensity)
    {
        // Update opening logic here
    }

    public void StopOpening()
    {
        myAnimator.Play("Close", 0, 0.0f);
        objectOpen = false;
    }
}
