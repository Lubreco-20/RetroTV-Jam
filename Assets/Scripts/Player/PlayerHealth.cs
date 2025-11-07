using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    private int actualHealth;

    private void Awake()
    {
        actualHealth = maxHealth;
    }

    public void GetDamage(int dmg)
    {
        actualHealth -= dmg;

        if (actualHealth < 0)
        {
            gameObject.SetActive(false);
            Debug.Log("GameOver");
        }
    }

    public void GetHealth(int health)
    {
        actualHealth = Mathf.Min(maxHealth, health + actualHealth);
    }
}
