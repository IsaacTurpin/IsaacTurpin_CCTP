using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] int ammoAmount = 5;
    [SerializeField] AmmoType ammoType;
    [SerializeField] ParticleSystem particleSystem;

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
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            FindObjectOfType<Ammo>().IncreaseCurrentAmmo(ammoType, ammoAmount);
            if(particlesEnabled)
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
        if(oscillatorToggle.activeInHierarchy)
        {
            oscillator.enabled = true;
        }
        else
        {
            oscillator.enabled = false;
        }

        //point light toggle
        if(pointLightToggle.activeInHierarchy) 
        {
            pointLight.SetActive(true);
        }
        else
        {
            pointLight.SetActive(false);
        }

        //Particle toggle
        if(particleToggle.activeInHierarchy)
        {
            particlesEnabled = true;
        }
        if(!particleToggle.activeInHierarchy)
        {
            particlesEnabled = false;
        }

        //Mesh toggle
        if(meshToggle.activeInHierarchy)
        {
            realMesh.SetActive(true);
            fakeMesh.SetActive(false);
        }
        if(!meshToggle.activeInHierarchy)
        {
            realMesh.SetActive(false);
            fakeMesh.SetActive(true);
        }
    }
}
