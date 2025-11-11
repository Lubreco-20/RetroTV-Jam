using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float duration;
    [SerializeField] private int damage;

    [SerializeField] private Rigidbody rb;
    private BulletPool pool;

    public void SetPool(BulletPool bulletPool) => pool = bulletPool;

    public void ShootBullet()
    {
        rb.AddForce(gameObject.transform.forward * speed, ForceMode.Impulse);

        Invoke("ReturnBullet", duration);
    }

    private void ReturnBullet()
    {
        rb.linearVelocity = Vector3.zero;
        pool.ReturnBullet(this.gameObject);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            CancelInvoke("ReturnBullet");

            col.gameObject.GetComponent<PlayerHealth>().GetDamage(damage);

            ReturnBullet();
        }
    }
}