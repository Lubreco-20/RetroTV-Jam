using System;
using UnityEngine;

public abstract class PickUpItem : MonoBehaviour
{
    public abstract void PickUp(PlayerController player);
}