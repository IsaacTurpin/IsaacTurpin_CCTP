using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Cinemachine;
using UnityEngine.InputSystem.HID;
using JetBrains.Annotations;

public class Weapon : MonoBehaviour
{
    [SerializeField] InputAction fire;
    [SerializeField] Camera FPCamera;
    [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] float range = 100f;
    [SerializeField] float damage = 30f;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject hitEffect;
    [SerializeField] GameObject hitEnemyEffect;
    [SerializeField] GameObject hitWoodEffect;
    [SerializeField] GameObject hitStoneEffect;
    [SerializeField] GameObject hitSandEffect;

    [SerializeField] Ammo ammoSlot;
    [SerializeField] AmmoType ammoType;
    [SerializeField] float timeBetweenShots = 0.5f;
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] private LayerMask Mask;
    [SerializeField] private Transform BulletSpawnPoint;
    [SerializeField] private bool AddBulletSpread = true;
    [SerializeField] private Vector3 BulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private TrailRenderer BulletTrail;
    [SerializeField] private float bulletSpeed = 100f;
    EnemyHealth target;
    bool hitWood;
    bool hitStone;
    bool hitSand;


    //Objects for toggle effects menu
    [SerializeField] GameObject TrailsObject;
    private bool TrailActive = true;
    [SerializeField] GameObject MuzzleflashObject;
    private bool MuzzleflashActive = true;
    [SerializeField] GameObject HitImpactsObject;
    private bool HitImpactsActive = true;

    bool canShoot = true;

    private void OnEnable()
    {
        fire.Enable();
        canShoot = true;
    }

    private void OnDisable()
    {
        fire.Disable();
    }

    void Update()
    {
        DisplayAmmo();
        CheckTogglesActive();

        if (fire.ReadValue<float>() > 0.5 && canShoot == true)
        {
            StartCoroutine(Shoot());
        }

        if (ammoType.ToString() == "Shells")
        {
            cinemachineVirtualCamera.m_Lens.FieldOfView = 80;
        }
    }

    private void DisplayAmmo()
    {
        int currentAmmo = ammoSlot.GetCurrentAmmo(ammoType);
        ammoText.text = currentAmmo.ToString();
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        if (ammoSlot.GetCurrentAmmo(ammoType) > 0)
        {
            PlayMuzzleFlash();
            ProcessRaycast();
            ammoSlot.ReduceCurrentAmmo(ammoType);

        }
        yield return new WaitForSeconds(timeBetweenShots);
        canShoot = true;
    }

    private void PlayMuzzleFlash()
    {
        if(MuzzleflashActive)
        {
            muzzleFlash.Play();
        }
        
    }

    private void ProcessRaycast()
    {
        Vector3 direction = GetDirection();
        RaycastHit hit;
        
        TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);
        if (Physics.Raycast(FPCamera.transform.position /*BulletSpawnPoint.position*/ ,  direction, out hit, range, Mask))
        {
            target = hit.transform.GetComponent<EnemyHealth>();
            if(hit.collider.gameObject.tag == "Wood")
            {
                hitWood = true;
            }
            if (hit.collider.gameObject.tag == "Stone")
            {
                hitStone = true;
            }
            if (hit.collider.gameObject.tag == "Sand")
            {
                hitSand = true;
            }


            //EnemyHealth HitEnemy = hit.transform.GetComponent<EnemyHealth>();
            StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));
        }
        else
        {
            target = null;
            //EnemyHealth HitEnemy = hit.transform.GetComponent<EnemyHealth>();
            StartCoroutine(SpawnTrail(trail, BulletSpawnPoint.position + direction * 100, hit.normal, false));
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = FPCamera.transform.forward;

        if(AddBulletSpread)
        {
            direction += new Vector3(Random.Range(-BulletSpreadVariance.x, BulletSpreadVariance.x),
               Random.Range(-BulletSpreadVariance.y, BulletSpreadVariance.y),
               Random.Range(-BulletSpreadVariance.z, BulletSpreadVariance.z));

            direction.Normalize();
        }

        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint, Vector3 HitNormal, bool madeImpact)
    {
        //float time = 0f;
        Vector3 startPosition = Trail.transform.position;
        float distance = Vector3.Distance(Trail.transform.position, HitPoint);
        float startingDistance = distance;


        while(distance > 0)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (distance / startingDistance));
            distance -= Time.deltaTime * bulletSpeed;

            yield return null;
        }

        Trail.transform.position = HitPoint;

        if(madeImpact) 
        {
            //CreateHitImpact(HitPoint, HitNormal);
            ChooseImpact(HitPoint, HitNormal);
        }
        else
        {
            yield return null;//StartCoroutine(SpawnTrail(Trail, HitPoint, HitNormal, false));
        }
        //ChooseHitImpact(Hit);

        Destroy(Trail.gameObject, Trail.time);
    }

    private void ChooseImpact(Vector3 HitPoint, Vector3 HitNormal) 
    {
        //CreateHitImpact(hit);
        //EnemyHealth target =  //hit.transform.GetComponent<EnemyHealth>();
        if (target == null)
        {
            if(hitWood)
            {
                CreateWoodHitImpact(HitPoint, HitNormal);
                hitWood = false;
            }
            if(hitStone)
            {
                CreateStoneHitImpact(HitPoint, HitNormal);
                hitStone = false;
            }
            if(hitSand)
            {
                CreateSandHitImpact(HitPoint, HitNormal);
                hitSand = false;
            }
            else
            {
                CreateHitImpact(HitPoint, HitNormal);
            }
            
            return;
        }
        else
        {
            CreateEnemyHitImpact(HitPoint, HitNormal);
            target.TakeDamage(damage);
        }
    }

    //Creating Hit Impacts
    private void CreateHitImpact(Vector3 HitPoint, Vector3 HitNormal)
    {
        if (HitImpactsActive)
        {
            GameObject impact = Instantiate(hitEffect, HitPoint, Quaternion.LookRotation(HitNormal));
            Destroy(impact, 1);
        }
    }
    private void CreateEnemyHitImpact(Vector3 HitPoint, Vector3 HitNormal)
    {
        if (HitImpactsActive)
        {
            GameObject impact = Instantiate(hitEnemyEffect, HitPoint, Quaternion.LookRotation(HitNormal));
            Destroy(impact, 1);
        }
    }
    private void CreateWoodHitImpact(Vector3 HitPoint, Vector3 HitNormal)
    {
        if (HitImpactsActive)
        {
            GameObject impact = Instantiate(hitWoodEffect, HitPoint, Quaternion.LookRotation(HitNormal));
            Destroy(impact, 1);
        }
    }
    private void CreateStoneHitImpact(Vector3 HitPoint, Vector3 HitNormal)
    {
        if (HitImpactsActive)
        {
            GameObject impact = Instantiate(hitStoneEffect, HitPoint, Quaternion.LookRotation(HitNormal));
            Destroy(impact, 1);
        }
    }
    private void CreateSandHitImpact(Vector3 HitPoint, Vector3 HitNormal)
    {
        if (HitImpactsActive)
        {
            GameObject impact = Instantiate(hitSandEffect, HitPoint, Quaternion.LookRotation(HitNormal));
            Destroy(impact, 1);
        }
    }

    //Checking toggle effects
    void CheckTogglesActive()
    {
        //Trails
        TrailActive = TrailsObject.activeInHierarchy;
        if(!TrailActive)
        {
            BulletTrail.enabled = false;
        }
        else
        {
            BulletTrail.enabled = true;
        }

        //Muzzleflash
        MuzzleflashActive = MuzzleflashObject.activeInHierarchy;

        //BulletImpacts
        HitImpactsActive = HitImpactsObject.activeInHierarchy;
    }
}
