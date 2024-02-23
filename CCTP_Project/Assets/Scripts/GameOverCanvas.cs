using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameOverCanvas : MonoBehaviour
{
    [SerializeField] GameObject gameOverUIFirst;
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(gameOverUIFirst);
    }
    private void OnDisable()
    {
        //EventSystem.current.SetSelectedGameObject(null);
    }
}
