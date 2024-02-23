using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartUI : MonoBehaviour
{
    [SerializeField] GameObject tutorialMenu;
    [SerializeField] FirstPersonController fpsController;
    public static bool GameIsPaused = false;

    [SerializeField] GameObject startUIFirst;

    // Start is called before the first frame update
    void Start()
    {
        Pause();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Resume()
    {
        EventSystem.current.SetSelectedGameObject(null);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        tutorialMenu.SetActive(false);
        Time.timeScale = 1f;
        fpsController.enabled = true;
        FindObjectOfType<WeaponSwitcher>().enabled = true;
        FindObjectOfType<WeaponZoom>().enabled = true;
        GameIsPaused = false;
    }

    void Pause()
    {
        EventSystem.current.SetSelectedGameObject(startUIFirst);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        tutorialMenu.SetActive(true);
        Time.timeScale = 0f;
        fpsController.enabled = false;
        FindObjectOfType<WeaponSwitcher>().enabled = false;
        FindObjectOfType<WeaponZoom>().enabled = false;
        GameIsPaused = true;
    }
}
