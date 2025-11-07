using UnityEngine;

public class GunWeapon : WeaponBase
{
    public override void StartAttacking()
    {
        base.StartAttacking();
    }

    private void Update()
    {
        if (!isAttacking) return;

        if (CanAttack())
            Attack();
    }

    public void Attack()
    {
        if (!CanConsumeAmmo()) return;

        lastFireTime = Time.time;

        Vector3 dir = Camera.main.transform.forward;

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, dir, out hit, Mathf.Infinity, hitMask))
        {
            var enemy = hit.collider.GetComponent<EnemyHealth>();
            if(enemy != null)
            {
                enemy.GetDamage(data.dmg);
            }

            //onHit?.Invoke
        }

        ApplyRecoil();
    }

    private void ApplyRecoil()
    {
        
    }
}
