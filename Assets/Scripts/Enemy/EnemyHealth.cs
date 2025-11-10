using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth;
    private int actualHealth;

    public event Action<EnemyHealth> OnEnemyDeath;

    private Animator anim;

    public void SetVal(Animator anim)
    {
        this.anim = anim;
    }

    public void RestoreHealth()
    {
        actualHealth = maxHealth;
    }

    public void GetDamage(int dmg)
    {
        actualHealth -= dmg;
        
        if (actualHealth <= 0)
        {
            anim.SetTrigger("Death");
            OnEnemyDeath?.Invoke(this);
            gameObject.SetActive(false);
        }
        else
        {
            anim.SetTrigger("Hurt");
        }
    }
}
