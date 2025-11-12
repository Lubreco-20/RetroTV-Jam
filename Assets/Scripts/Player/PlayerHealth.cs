using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    private int actualHealth;

    public Image screenBlood;

    private void Awake()
    {
        actualHealth = maxHealth;
        SetAlphaBlood();
    }

    public void GetDamage(int dmg)
    {
        actualHealth -= dmg;
        actualHealth = Mathf.Max(0, actualHealth);
        SetAlphaBlood();

        if (actualHealth < 0)
        {
            Debug.Log("GameOver");
            return;
        }


    }

    private void SetAlphaBlood()
    {
        float perc = (float)actualHealth / maxHealth;
        float newAlpha = Mathf.Lerp(1f, 0f, perc);

        Color color = screenBlood.color;
        color.a = newAlpha;
        screenBlood.color = color;
    }

    public void GetHealth(int health)
    {
        actualHealth = Mathf.Min(maxHealth, health + actualHealth);
        SetAlphaBlood();
    }
}
