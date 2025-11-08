using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [Header("General")]
    public WeaponData data;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask enemyMask;

    private Camera fpsCam;

    // Rates
    private float timeBtwShots;
    private float fireRate;

    // Checkers
    private bool isAttacking;
    private bool isReadyToAttack;
    private bool isReloading;

    // Bullets
    [HideInInspector] public int currentAmmo;
    private int bulletsShot;
    [HideInInspector] public int currentMagazineSize;
    [HideInInspector] public int currentReserve;

    public Action<int, int, int> OnBulletsChange;

    public void Initialize(Camera cam)
    {
        fpsCam = cam;
        timeBtwShots = data.timeBtwShooting;
        fireRate = data.fireRate;
        currentAmmo = data.magazineSize;
        currentMagazineSize = data.magazineSize;
        currentReserve = data.reserveAmmo;
    }

    public void SetWeapon()
    {
        currentAmmo = data.magazineSize;
        currentMagazineSize = data.magazineSize;
        currentReserve = data.reserveAmmo;

        OnBulletsChange?.Invoke(currentAmmo, currentMagazineSize, currentReserve);

        isReadyToAttack = true;
        isReloading = false;
    }

    public void StartAttacking()
    {
        if (isReloading ||
            data.useBullets && currentAmmo <= 0 ||
            isAttacking ||
            !isReadyToAttack)
            return;

        isAttacking = true;
        bulletsShot = data.bulletsPerShot;
        Attack();
    }

    public void StopAttacking()
    {
        isAttacking = false;
    }

    private void Attack()
    {
        isReadyToAttack = false;

        if (data.useBullets)
        {
            for (int i = 0; i < data.bulletsPerShot; i++)
            {
                // Spread
                float x = UnityEngine.Random.Range(-data.spread, data.spread);
                float y = UnityEngine.Random.Range(-data.spread, data.spread);

                // Calculate direction spread
                Vector3 dirSpread = fpsCam.transform.forward + new Vector3(x, y, 0);

                // Guardar para visualizar
                debugRays.Add((fpsCam.transform.position, dirSpread.normalized));

                if (Physics.Raycast(fpsCam.transform.position, dirSpread, out rayHit, data.range, enemyMask))
                {
                    if (rayHit.collider.CompareTag("Enemy"))
                    {
                        rayHit.collider.GetComponent<EnemyHealth>().GetDamage(data.dmg);
                    }
                }
            }

            currentAmmo--;
            //bulletsShot--;

            OnBulletsChange?.Invoke(currentAmmo, currentMagazineSize, currentReserve);
        }

        Invoke("ResetAttack", fireRate);

        if(currentAmmo > 0 && isAttacking && data.allowBtnHold)
            Invoke("Attack", timeBtwShots);
    }

    private void ResetAttack() => isReadyToAttack = true;

    public void StartReloading()
    {
        if (isReloading ||
            data.useBullets && currentAmmo >= data.magazineSize ||
            currentReserve <= 0) return;

        StopAttacking();
        isReloading = true;

        //Sustituir Invoke por final de animación
        Invoke("Reload", data.reloadTime);
    }

    // Lo llamaré desde la animación
    public void Reload()
    {
        /*int bulletsNeeded = currentMagazineSize - currentAmmo;
        int bulletsTaken = Mathf.Min(bulletsNeeded, currentReserve);

        currentAmmo += bulletsTaken;
        currentReserve -= bulletsTaken;*/
        
        currentAmmo = currentMagazineSize; // TEMP

        OnBulletsChange?.Invoke(currentAmmo, currentMagazineSize, currentReserve);

        isReloading = false;
    }

    private List<(Vector3 origin, Vector3 dir)> debugRays = new();
    [Header("Debug")]
    [SerializeField] private bool showGizmos;

    private void OnDrawGizmos()
    {
        if (!showGizmos || debugRays == null || debugRays.Count == 0)
            return;

        Gizmos.color = Color.red;

        foreach (var ray in debugRays)
        {
            Gizmos.DrawRay(ray.origin, ray.dir * data.range);
        }
    }
}