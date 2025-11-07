using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string nameWeapon;
    public int dmg;
    public float fireRate;
    public int magazineSize;
    public int reserveAmmo;
    public float reloadTime;
    public float spread;
    public float recoil;
}