using System.Collections;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    public LayerMask hitMask;

    protected WeaponData data;

    protected float timeBtwShots;
    protected float lastFireTime;

    protected bool isAttacking;
    protected bool isReloading;

    protected int currentAmmo;
    protected int currentMagazineSize;
    protected int currentReserve;

    public virtual void Initialize(WeaponData data)
    {
        this.data = data;
        timeBtwShots = data.fireRate;
        currentAmmo = data.magazineSize;
        currentMagazineSize = data.magazineSize;
        currentReserve = data.reserveAmmo;
    }

    public virtual void StartAttacking()
    {
        if (isReloading) return;
        isAttacking = true;
    }

    public virtual void StopAttacking()
    {
        isAttacking = false;
    }

    public virtual void StartReloading()
    {
        if (isReloading) return;
        if(currentAmmo >= data.magazineSize) return;
        if (currentReserve <= 0) return;

        StopAttacking();
        isReloading = true;

        StartCoroutine(ReloadTime());
    }

    //TEMP
    IEnumerator ReloadTime()
    {
        yield return new WaitForSeconds(data.reloadTime);

        int bulletsNeeded = currentMagazineSize - currentAmmo;
        int bulletsTaken = Mathf.Min(bulletsNeeded, currentReserve);

        currentAmmo += bulletsTaken;
        currentReserve -= bulletsTaken;

        isReloading = false;
    }

    /// <summary>
    /// Lo llamaré según el tipo de arma para su animación
    /// </summary>
    public virtual void Reload() { }

    public virtual bool CanConsumeAmmo()
    {
        if (currentAmmo == 0) return false;

        currentAmmo--;
        return true;
    }

    public virtual bool CanAttack()
    {
        if(isReloading) return false;
        if (Time.time - lastFireTime < timeBtwShots) return false;
        if (currentAmmo <= 0) return false;

        return true;
    }
}