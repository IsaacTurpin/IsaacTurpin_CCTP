using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Cinemachine;

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
        muzzleFlash.Play();
    }

    private void ProcessRaycast()
    {
        Vector3 direction = GetDirection();
        RaycastHit hit;
        if(Physics.Raycast(FPCamera.transform.position /*BulletSpawnPoint.position*/ ,  direction, out hit, range, Mask))
        {
            
            // change all references to 'FPCamera.transform.position', to BulletSpawnPoint.Position. (Private Transform BulletSpawnPoint)
            TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hit.point));

            //CreateHitImpact(hit);
            EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
            if (target == null)
            {
                CreateHitImpact(hit);
                return;
            }
            else
            {
                CreateEnemyHitImpact(hit);
                target.TakeDamage(damage);
            }
            
        }
        else
        {
            return;
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

    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint)
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
        //ChooseHitImpact(Hit);

        Destroy(Trail.gameObject, Trail.time);
    }

    private void CreateHitImpact(RaycastHit hit)
    {
        GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impact, 1);
    }
    private void CreateEnemyHitImpact(RaycastHit hit)
    {
        GameObject impact = Instantiate(hitEnemyEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impact, 1);
    }
}
