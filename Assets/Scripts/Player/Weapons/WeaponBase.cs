using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [Header("General")]
    public WeaponData data;
    public RaycastHit rayHit;
    public LayerMask enemyMask;
    CamShake shake;

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
    [HideInInspector] public int currentMagazineSize;
    [HideInInspector] public int currentReserve;

    // Animator
    private Animator anim;

    public Action<int, int, int> OnBulletsChange;

    public void Initialize(Camera cam)
    {
        fpsCam = cam;
        timeBtwShots = data.timeBtwShooting;
        fireRate = data.fireRate;
        currentAmmo = data.magazineSize;
        currentMagazineSize = data.magazineSize;
        currentReserve = data.reserveAmmo;

        shake = CamShake.Instance;

        anim = GetComponent<Animator>();
    }

    public void SetWeapon()
    {
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
        Attack();
    }

    public void StopAttacking()
    {
        isAttacking = false;
        //isReadyToAttack = true;
    }

    private void Attack()
    {
        isReadyToAttack = false;

        anim.SetTrigger("Attack");

        if (data.useBullets)
        {
            HandleRangeAttack();
        }
        else
        {
            HandleMeleeAttack();
        }

        //Invoke("ResetAttack", fireRate);

        if(currentAmmo > 0 && isAttacking && data.allowBtnHold)
            Invoke("Attack", timeBtwShots);
    }

    private void HandleRangeAttack()
    {
        for (int i = 0; i < data.bulletsPerShot; i++)
        {
            // Spread
            float x = UnityEngine.Random.Range(-data.spread, data.spread);
            float y = UnityEngine.Random.Range(-data.spread, data.spread);

            // Calculate direction spread
            Vector3 dirSpread = fpsCam.transform.forward + new Vector3(x, y, 0);

            // Debug
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

        OnBulletsChange?.Invoke(currentAmmo, currentMagazineSize, currentReserve);
    }

    private void HandleMeleeAttack()
    {
        Vector3 boxCenter = fpsCam.transform.position + fpsCam.transform.forward * ((data.range / 2f) + 1);
        Vector3 halfExtents = new Vector3(data.spread, data.spread, data.range / 2f);

        Collider[] hits = Physics.OverlapBox(boxCenter, halfExtents, fpsCam.transform.rotation, enemyMask);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                hit.GetComponent<EnemyHealth>()?.GetDamage(data.dmg);
            }
        }
    }

    // Called from animator
    public void ResetAttack() => isReadyToAttack = true;

    public void StartReloading()
    {
        if (isReloading ||
            data.useBullets && currentAmmo >= data.magazineSize ||
            currentReserve <= 0) return;

        StopAttacking();
        isReloading = true;

        anim.SetTrigger("Reload");
    }

    public void StopReloading()
    {
        isReloading = false;
    }

    // Called from animator
    public void Reload()
    {
        int bulletsNeeded = currentMagazineSize - currentAmmo;
        int bulletsTaken = Mathf.Min(bulletsNeeded, currentReserve);

        currentAmmo += bulletsTaken;
        currentReserve -= bulletsTaken;

        OnBulletsChange?.Invoke(currentAmmo, currentMagazineSize, currentReserve);

        isReloading = false;
    }

    public void CallForShake(int intensity)
    {
        shake.ShakeCamera(intensity, 0.5f);
    }

    private List<(Vector3 origin, Vector3 dir)> debugRays = new();
    [Header("Debug")]
    [SerializeField] private bool showGizmos;
    
    private void OnDrawGizmos()
    {
        if (!showGizmos || data == null || fpsCam == null) return;

        Gizmos.color = Color.red;

        if (data.useBullets)
        {
            if (debugRays == null || debugRays.Count == 0) return;

            foreach (var ray in debugRays)
            {
                Gizmos.DrawRay(ray.origin, ray.dir * data.range);
            }
        }
        else
        {
            Vector3 boxCenter = fpsCam.transform.position + fpsCam.transform.forward * ((data.range / 2f) + 1);
            Vector3 halfExtents = new Vector3(data.spread, data.spread, data.range / 2f);
            Gizmos.matrix = Matrix4x4.TRS(boxCenter, fpsCam.transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, halfExtents * 2f);
        }
    }
}