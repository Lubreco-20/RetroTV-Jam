using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth;
    private int actualHealth;

    private void OnEnable()
    {
        actualHealth = maxHealth;
    }

    public void GetDamage(int dmg)
    {
        actualHealth -= dmg;

        if(actualHealth < 0)
            gameObject.SetActive(false);
    }
}
