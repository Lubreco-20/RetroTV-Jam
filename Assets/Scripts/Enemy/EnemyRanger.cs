using UnityEngine;

public class EnemyRanger : MonoBehaviour
{
    private int maxProjectiles = 5;
    public GameObject projectile;

    public void Awake()
    {
        InstantiateProjectiles();
    }

    private void InstantiateProjectiles()
    {
        for (int i = 0; i < maxProjectiles; i++)
        {
            GameObject proj = Instantiate(projectile, Vector3.zero, Quaternion.identity, transform);
            proj.SetActive(false);
        }
    }

    public void RequestProjectile()
    {

    }

    public void ReturnProjectile()
    {

    }
}
