using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleMazeCompletion : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject respawnPoint;
    FirstPersonController fpsController;

    private void Start()
    {
        fpsController = player.GetComponent<FirstPersonController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other != null)
        {
            Debug.Log("wtf");
            RespawnPlayer();
        }
    }

    void RespawnPlayer()
    {
        fpsController.enabled = false;
        player.transform.position = respawnPoint.transform.position;
        player.transform.eulerAngles = respawnPoint.transform.eulerAngles;
        Invoke(("EnableController"), 0.1f);
    }
    void EnableController()
    {
        fpsController.enabled = true;
    }
}
