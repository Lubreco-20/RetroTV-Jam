using UnityEngine;

public class FirstAidItem : PickUpItem
{
    public int healthAmount;

    public override void PickUp(PlayerController player)
    {
        player.health.GetHealth(healthAmount);
    }
}
