using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EffectsMenu : MonoBehaviour
{
    [SerializeField] InputAction pause;
    [SerializeField] GameObject effectsMenuUI;
    [SerializeField] FirstPersonController fpsController;
    public static bool GameIsPaused = false;
    [SerializeField] GameObject ammoCanvas;
    [SerializeField] GameObject startMenu;
    private bool startMenuActive;
    [SerializeField] GameObject tutorialMenu;
    private bool tutorialMenuActive;
    private void OnEnable()
    {
        pause.Enable();
    }

    private void OnDisable()
    {
        pause.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        startMenuActive = startMenu.activeInHierarchy;
        if(startMenuActive)
        {
            return;
        }
        tutorialMenuActive = tutorialMenu.activeInHierarchy;
        if (tutorialMenuActive)
        {
            return;
        }
        var wasPressed = pause.triggered && pause.ReadValue<float>() > 0;

        if(wasPressed)
        {
            if (GameIsPaused) 
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        effectsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        fpsController.enabled = true;
        FindObjectOfType<WeaponSwitcher>().enabled = true;
        FindObjectOfType<WeaponZoom>().enabled = true;
        GameIsPaused = false;
    }

    void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        effectsMenuUI.SetActive(true);
        Time.timeScale = 0f;
        fpsController.enabled = false;
        FindObjectOfType<WeaponSwitcher>().enabled = false;
        FindObjectOfType<WeaponZoom>().enabled = false;
        GameIsPaused = true;
    }
}
