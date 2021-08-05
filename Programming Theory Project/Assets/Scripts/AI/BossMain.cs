using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMain : EnemyMain
{
    public event DamageDealtHandler DamageDealt;
    [SerializeField] protected Enums.Particals collisionParticle;

    // Start is called before the first frame update
    public override void Damage(float damageTaken, Enums.DamageType damageType, Vector3 damageLocation)
    {
        base.Damage(damageTaken, damageType, damageLocation);

        if (DamageDealt != null)
        {
            DamageDealt(damageTaken);
        }
    }
}
