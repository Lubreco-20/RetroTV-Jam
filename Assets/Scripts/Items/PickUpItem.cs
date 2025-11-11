using System;
using UnityEngine;

public abstract class PickUpItem : MonoBehaviour
{
    public Animator anim;

    private void OnEnable()
    {
        anim.Play("Item_Rotation");
    }

    public abstract void PickUp(PlayerController player);

    public void DeactivateItem()
        => gameObject.SetActive(false);
}