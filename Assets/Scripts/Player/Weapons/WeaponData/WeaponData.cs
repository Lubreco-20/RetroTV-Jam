using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName;

    public int dmg;

    public float fireRate;
    public float timeBtwShooting;

    public int magazineSize;
    public int reserveAmmo;
    public int bulletsPerShot;

    public float reloadTime;
    public float spread;
    public float range;

    public bool allowBtnHold;
    public bool useBullets;
}