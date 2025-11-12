using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("General")]
    public CharacterController characterController;
    PlayerInputs inputActions;

    [Header("Movement")]
    public float playerSpeed = 5f;
    float gravity = -20f;
    Vector3 vel;
    Vector2 dir;

    [Header("Camera")]
    [Range(0f, 1f)]
    public float sensX = .3f,sensY = .3f;

    public Transform cam;
    float xRot;

    Vector2 mousePos;

    [Header("WeaponController")]
    public WeaponHandler weaponHandler;

    [Header("Classes")]
    public PlayerHealth health;
    public WeaponHandler weapons;
    public GameState state;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        inputActions = new PlayerInputs();

        inputActions.PlayerMap.Move.performed += ctx => dir = ctx.ReadValue<Vector2>();
        inputActions.PlayerMap.Move.canceled += ctx => dir = Vector2.zero;

        inputActions.PlayerMap.Look.performed += ctx => mousePos = ctx.ReadValue<Vector2>();
        inputActions.PlayerMap.Look.canceled += ctx => mousePos = Vector2.zero;

        inputActions.PlayerMap.Attack.started += weaponHandler.OnAttackStart;
        inputActions.PlayerMap.Attack.canceled += weaponHandler.OnAttackStop;

        inputActions.PlayerMap.Reload.started += weaponHandler.OnReload;

        inputActions.PlayerMap.Scroll.performed += weaponHandler.HandleScrollWeapon;
        inputActions.PlayerMap.Scroll.canceled += weaponHandler.HandleScrollWeapon;

        inputActions.PlayerMap.Pause.started += state.PauseResume;
    }

    private void Update()
    {
        if (state.CheckIfGameIsPause())
        {
            DisableInput();
            return;
        }

        EnableInput();

        float mouseX = mousePos.x * sensX;
        float mouseY = mousePos.y * sensY;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90, 90);

        // Rotacion del cuerpo
        transform.Rotate(Vector3.up * mouseX);

        // Rotacion de la camara
        cam.localRotation = Quaternion.Euler(xRot, 0f, 0f);

        moveCharacter();
    }

    void moveCharacter()
    {
        Vector3 moveDir = transform.right * dir.x + transform.forward * dir.y;

        if (characterController.isGrounded && vel.y < 0)
            vel.y = -2;
        else
            vel.y += gravity * Time.deltaTime;

        Vector3 finalMove = moveDir * playerSpeed + vel;
        characterController.Move(finalMove * Time.deltaTime);
    }

    public void DisableInput()
        => inputActions.Disable();

    public void EnableInput()
        => inputActions.Enable();

    void OnEnable()
    {
        EnableInput();
    }

    void OnDisable()
    {
        DisableInput();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Item"))
        {
            PickUpItem item = col.GetComponent<PickUpItem>();

            if (item != null)
            {
                item.anim.SetTrigger("PickUp");
                item.PickUp(this);
            }
        }
    }
}
