using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHandler : MonoBehaviour
{
    public List<WeaponBase> weaponHolder = new List<WeaponBase>();
    private int currentIdx = 0;

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
        EquipWeapon(currentIdx);
    }

    public void InitializeAllWeapons()
    {
        foreach(var weapon in weaponHolder)
        {
            weapon.Initialize(cam);
        }
    }

    public void HandleScrollWeapon(InputAction.CallbackContext context)
    {
        float scrollY = context.ReadValue<Vector2>().y;

        if (scrollY < 0)
            NextWeapon();
        else if (scrollY > 0)
            PreviousWeapon();
    }

    private void NextWeapon()
    {
        currentIdx++;
        if (currentIdx >= weaponHolder.Count)
            currentIdx = 0;

        EquipWeapon(currentIdx);
    }

    private void PreviousWeapon()
    {
        currentIdx--;

        if (currentIdx < 0)
            currentIdx = weaponHolder.Count - 1;

        EquipWeapon(currentIdx);
    }

    private void EquipWeapon(int idx)
    {
        if (currentWeapon != null)
            currentWeapon.OnBulletsChange -= UpdateAmmoUI;

        for (int i = 0; i < weaponHolder.Count; i++)
        {
            if(currentIdx == i)
            {
                currentWeapon = weaponHolder[i];
                currentWeapon.gameObject.SetActive(true);
                currentWeapon.OnBulletsChange += UpdateAmmoUI;
                currentWeapon.SetWeapon();
            }
            else
            {
                weaponHolder[i].gameObject.SetActive(false);
            }
        }
    }

    public void EquipWeaponByName(string weaponName)
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

        currentAmmoText.text = (current == -1) ? "\u221E" : current.ToString();
        magazineSizeText.text = (magSize == -1) ? "\u221E" : magSize.ToString();
        ReserveText.text = (reserve == -1) ? "" : reserve.ToString();
    }

    public void AddReserveAmmo(string name, int addAmmo)
    {
        foreach (var weapon in weaponHolder)
        {
            if(weapon.data.weaponName == name)
            {
                weapon.currentReserve += addAmmo;

                if(currentWeapon == weapon)
                    UpdateAmmoUI(weapon.currentAmmo, weapon.currentMagazineSize, weapon.currentReserve);
            }
        }
    }

    bool canAttack = true;
    public void SetCanAttack(bool value)
    {
        canAttack = value;
        if (!value)
        {
            currentWeapon?.StopAttacking();
            currentWeapon?.ResetAttack();
            OnStopReloading();
        }
    }

    public void OnAttackStart(InputAction.CallbackContext context)
    {
        if (!canAttack) return;
        currentWeapon?.StartAttacking();
    }

    public void OnAttackStop(InputAction.CallbackContext context) => currentWeapon?.StopAttacking();

    public void OnReload(InputAction.CallbackContext context)
    {
        if (!canAttack) return;
        currentWeapon?.StartReloading();
    }

    public void OnStopReloading()
        => currentWeapon?.StopReloading();
}
