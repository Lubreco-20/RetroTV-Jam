using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private int maxHealth;
    private int actualHealth;

    public event Action<EnemyHealth> OnEnemyDeath;

    private Animator anim;
    private SoundEffect sfx;

    public void InitailizeHealthValues(Animator anim, SoundEffect sfx, int health)
    {
        maxHealth = health;
        this.anim = anim;
        this.sfx = sfx;
    }

    public void RestoreHealth()
    {
        actualHealth = maxHealth;
    }

    public void GetDamage(int dmg)
    {
        actualHealth -= dmg;

        sfx.SetRandomPitch();
        sfx.GetRandomSoundEffect(4, 7); // Hits/Deaths

        if (actualHealth <= 0)
        {
            anim.Play("Zombie_Die");
        }
        else
        {
            anim.Play("Zombie_Hurt");
        }
    }

    public void OnDeath()
    {
        OnEnemyDeath?.Invoke(this);
        gameObject.SetActive(false);
    }
}
