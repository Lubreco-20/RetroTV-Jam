using UnityEngine;

public class AmmoItem : PickUpItem
{
    public string weaponName;
    public int ammoAmount;

    public override void PickUp(PlayerController player)
    {
        WeaponHandler weaponHandler = player.weapons;
        if (weaponHandler != null)
        {
            weaponHandler.AddReserveAmmo(weaponName, ammoAmount);
        }
    }
}
