using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("General")]
    public Rigidbody rb;
    PlayerInputs inputActions;

    [Header("Movement")]
    public float playerSpeed = 5f;
    Vector2 dir;

    [Header("Camera")]
    [Range(0f, 1f)]
    public float sensX = .3f,sensY = .3f;

    public Transform cam;
    float xRot;

    Vector2 mousePos;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        inputActions = new PlayerInputs();

        inputActions.PlayerMap.Move.performed += ctx => dir = ctx.ReadValue<Vector2>();
        inputActions.PlayerMap.Move.canceled += ctx => dir = Vector2.zero;

        inputActions.PlayerMap.Look.performed += ctx => mousePos = ctx.ReadValue<Vector2>();
        inputActions.PlayerMap.Look.canceled += ctx => mousePos = Vector2.zero;

        inputActions.Enable();
    }

    private void Update()
    {
        float mouseX = mousePos.x * sensX;
        float mouseY = mousePos.y * sensY;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90, 90);

        // Rotacion del cuerpo
        transform.Rotate(Vector3.up * mouseX);

        // Rotacion de la camara
        cam.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }

    void FixedUpdate()
    {
        moveCharacter();
    }

    void moveCharacter()
    {
        Vector3 moveDir = transform.right * dir.x + transform.forward * dir.y;
        rb.linearVelocity = moveDir * playerSpeed;
    }
}
