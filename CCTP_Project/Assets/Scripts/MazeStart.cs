using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeStart : MonoBehaviour
{
    [SerializeField] GameObject timerManager;
    Timer timer;

    private void Start()
    {
        timer = timerManager.GetComponent<Timer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other != null) 
        {
            timer.resetTimer = true;
            timer.timerEnabled = true;
        }
    }
}
