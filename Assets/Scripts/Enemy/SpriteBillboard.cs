using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (cam == null) return;

        transform.LookAt(cam.transform.position, Vector3.up);
        //transform.forward = cam.transform.forward;
    }
}
