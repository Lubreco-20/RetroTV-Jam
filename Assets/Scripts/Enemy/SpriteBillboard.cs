using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform.position, Vector3.up);
        //transform.forward = Camera.main.transform.forward;
    }
}
