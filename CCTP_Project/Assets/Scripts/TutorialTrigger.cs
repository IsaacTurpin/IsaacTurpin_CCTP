using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] GameObject TutorialMenu;
    [SerializeField] GameObject MenuCanvas;
    TutorialUI tutorialUI;
    private bool hasEntered;

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
        }
    }
}
