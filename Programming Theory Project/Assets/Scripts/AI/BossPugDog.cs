using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPugDog : EnemyMain
{
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        ContactPoint contact = collision.contacts[0];
        Vector3 pos = contact.point;
        if (collision.gameObject.CompareTag("Enemy"))
        {
            IDamageable<float, string, Vector3> hit = (IDamageable<float, string, Vector3>)collision.gameObject.GetComponent(typeof(IDamageable<float, string, Vector3>));
            if (hit != null)
            {
                hit.Damage(200, "Collision", pos);
            }
        }
    }
    public override void Damage(float damageTaken, string damageType, Vector3 damageLocation)
    {
        base.Damage(damageTaken, damageType, damageLocation);
        
        if (damageType == "Collision")
        {
            spawnManager.SpawnPartical("BlueSmall", damageLocation);
            runSpeed = backwardSpeed;
        }
    }
    protected override void Death()
    {
        spawnManager.SpawnPartical("BlueLarge", gameObject.transform.position);
        GameManager.Instance.EndGame();
        spawnManager.BossDeath(gameObject.transform.position);
        base.Death();
    }
}
