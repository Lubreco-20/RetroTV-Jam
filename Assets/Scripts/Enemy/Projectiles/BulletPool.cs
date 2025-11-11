using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int amountBullets;
    [SerializeField] private Transform shootPos;
    [SerializeField] private Transform shootRot;
    private GameObject[] bullets;

    [SerializeField] private EnemyController controller;

    private void Awake()
    {
        CreateBullets();
    }

    private void CreateBullets()
    {
        bullets = new GameObject[amountBullets];

        for (int i = 0; i < amountBullets; i++)
        {
            GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity, transform);
            bullets[i] = newBullet;
            newBullet.GetComponent<Bullet>().SetPool(this);
            newBullet.SetActive(false);
        }
    }

    public void RequestBullet()
    {
        for(int i = 0; i < amountBullets; i++)
        {
            GameObject actualBullet = bullets[i];

            if (!actualBullet.activeSelf)
            {
                actualBullet.transform.position = shootPos.position;

                Quaternion rot = Quaternion.LookRotation(shootRot.forward);
                Vector3 euler = rot.eulerAngles;
                euler.x = 0f;
                rot = Quaternion.Euler(euler);
                actualBullet.transform.rotation = rot;

                actualBullet.transform.parent = null;
                actualBullet.SetActive(true);
                actualBullet.GetComponent<Bullet>().ShootBullet();

                break;
            }
        }
    }

    public void ReturnBullet(GameObject actualBullet)
    {
        actualBullet.transform.position = transform.position;
        actualBullet.transform.rotation = transform.rotation;
        actualBullet.transform.parent = transform;
        actualBullet.SetActive(false);
    }

    private void OnEnable()
    {
        controller.OnRangeAttack += RequestBullet;
    }

    private void OnDisable()
    {
        controller.OnRangeAttack -= RequestBullet;
    }
}
