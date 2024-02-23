using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] ParticleSystem particleSystem;
    PlayerHealth playerHealth;

    //Toggles
    [SerializeField] GameObject oscillatorToggle;
    [SerializeField] GameObject pointLightToggle;
    [SerializeField] GameObject particleToggle;
    private bool particlesEnabled;
    [SerializeField] GameObject meshToggle;

    Oscillator oscillator;
    [SerializeField] GameObject pointLight;
    [SerializeField] GameObject realMesh;
    [SerializeField] GameObject fakeMesh;

    private void Start()
    {
        oscillator = GetComponent<Oscillator>();
        playerHealth = FindObjectOfType<PlayerHealth>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(playerHealth.IsAtMaxHealth()) { return; }

            playerHealth.IncreaseHealth();
            if (particlesEnabled)
            {
                particleSystem.gameObject.transform.SetParent(null);
                particleSystem.Play();
            }
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        CheckToggles();
    }

    private void CheckToggles()
    {
        //oscillator toggle
        if (oscillatorToggle.activeInHierarchy)
        {
            oscillator.enabled = true;
        }
        else
        {
            oscillator.enabled = false;
        }

        //point light toggle
        if (pointLightToggle.activeInHierarchy)
        {
            pointLight.SetActive(true);
        }
        else
        {
            pointLight.SetActive(false);
        }

        //Particle toggle
        if (particleToggle.activeInHierarchy)
        {
            particlesEnabled = true;
        }
        if (!particleToggle.activeInHierarchy)
        {
            particlesEnabled = false;
        }

        //Mesh toggle
        if (meshToggle.activeInHierarchy)
        {
            realMesh.SetActive(true);
            fakeMesh.SetActive(false);
        }
        if (!meshToggle.activeInHierarchy)
        {
            realMesh.SetActive(false);
            fakeMesh.SetActive(true);
        }
    }
}
