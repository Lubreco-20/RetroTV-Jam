using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth;
    private int actualHealth;

    public event Action<EnemyHealth> OnEnemyDeath;

    private void OnEnable()
    {
        actualHealth = maxHealth;
    }

    public void GetDamage(int dmg)
    {
        actualHealth -= dmg;
        Debug.Log("Ma dao");

        if (actualHealth <= 0)
        {
            OnEnemyDeath?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}
