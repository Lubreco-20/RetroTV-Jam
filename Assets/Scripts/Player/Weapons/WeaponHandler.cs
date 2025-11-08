using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHandler : MonoBehaviour
{
    public List<WeaponBase> weaponHolder = new List<WeaponBase>();
    private int currentIdx = -1;
    public WeaponBase startingWeapon;

    private WeaponBase currentWeapon;
    private Camera cam;

    [Header("Texts")]
    public TextMeshProUGUI currentAmmoText;
    public TextMeshProUGUI magazineSizeText;
    public TextMeshProUGUI ReserveText;

    void Start()
    {
        cam = Camera.main;
        InitializeAllWeapons();
        EquipWeapon(startingWeapon.data.weaponName);
    }

    public void InitializeAllWeapons()
    {
        foreach(var weapon in weaponHolder)
        {
            weapon.Initialize(cam);
        }
    }

    public void EquipWeapon(string weaponName)
    {
        int foundIndex = weaponHolder.FindIndex(w => w.data.weaponName == weaponName);
        if (foundIndex >= 0 && currentIdx != foundIndex)
        {
            currentIdx = foundIndex;

            if (currentWeapon != null)
                currentWeapon.OnBulletsChange -= UpdateAmmoUI;

            foreach (var weapon in weaponHolder)
            {
                if (weapon.data.weaponName == weaponName)
                {
                    weapon.gameObject.SetActive(true);
                    currentWeapon = weapon;
                    currentWeapon.OnBulletsChange += UpdateAmmoUI;
                    currentWeapon.SetWeapon();
                }
                else
                {
                    weapon.gameObject.SetActive(false);
                }
            }
        }
    }

    private void UpdateAmmoUI(int current, int magSize, int reserve)
    {
        if (currentWeapon == null) return;

        currentAmmoText.text = current.ToString();
        magazineSizeText.text = magSize.ToString();
        ReserveText.text = reserve.ToString();
    }

    public void OnAttackStart(InputAction.CallbackContext context) => currentWeapon?.StartAttacking();

    public void OnAttackStop(InputAction.CallbackContext context) => currentWeapon?.StopAttacking();

    public void OnReload(InputAction.CallbackContext context) => currentWeapon?.StartReloading();
}
