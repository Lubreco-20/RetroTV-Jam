using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    private int actualHealth;

    public Image screenBlood;

    CamShake shake;
    GameState state;
    Animator anim;

    [SerializeField] SoundEffect sound;

    public void InitializeHealth(GameState state, Animator anim)
    {
        actualHealth = maxHealth;
        shake = CamShake.Instance;
        this.state = state;
        this.anim = anim;
        SetAlphaBlood();
    }

    public void GetDamage(int dmg)
    {
        actualHealth -= dmg;
        actualHealth = Mathf.Max(0, actualHealth);
        SetAlphaBlood();

        if (actualHealth <= 0)
        {
            sound.SetOGPitch();
            anim.Play("Player_Death");
            anim.updateMode = AnimatorUpdateMode.UnscaledTime;
            state.EndOfTheGame(true); // True = Game Over
            return;
        }

        sound.SetRandomPitch();
        sound.PlaySoundEffect("Player_Hurt");
        shake.ShakeCamera(1f, 0.5f);
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
