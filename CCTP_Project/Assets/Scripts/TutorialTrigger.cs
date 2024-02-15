using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] GameObject TutorialMenu;
    [SerializeField] GameObject MenuCanvas;
    TutorialUI tutorialUI;
    private bool hasEntered;
    [SerializeField] GameObject nextEffectsElements;
    [SerializeField] GameObject nextEffectsElements2;
    [SerializeField] GameObject nextEffectsElements3;
    [SerializeField] GameObject oldEffectsElements;

    private void Start()
    {
        tutorialUI = MenuCanvas.GetComponent<TutorialUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasEntered) { return; }
        if(other != null)
        {
            TutorialMenu.SetActive(true);
            tutorialUI.Pause();
            hasEntered = true;
            TurnOnEffects();
            TurnOffEffects();
        }
    }

    void TurnOnEffects()
    {
        if(nextEffectsElements != null)
        {
            nextEffectsElements.SetActive(true);
        }

        if (nextEffectsElements2 != null)
        {
            nextEffectsElements2.SetActive(true);
        }

        if (nextEffectsElements3 != null)
        {
            nextEffectsElements3.SetActive(true);
        }

    }
    
    void TurnOffEffects()
    {
        if(oldEffectsElements != null)
        {
            oldEffectsElements.SetActive(false);
        }
        
    }
}
