using UnityEngine;
using UnityEngine.InputSystem;

public class DuckCuack : MonoBehaviour
{
    public Animator duckAnim;
    private bool isCuacking;

    PlayerInputs inputActions;

    public WeaponHandler handler;
    public GameObject weapons;

    private void Awake()
    {
        inputActions = new PlayerInputs();

        inputActions.PlayerMap.Cuack.started += CallForTheDuck;
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    private void CallForTheDuck(InputAction.CallbackContext context)
    {
        if (isCuacking) return;
        isCuacking = true;
        handler.SetCanAttack(false);
        weapons.SetActive(false);
        duckAnim.SetTrigger("Cuack");
    }

    public void StopTheCuacking()
    {
        handler.SetCanAttack(true);
        weapons.SetActive(true);
        isCuacking = false;
    }
}
