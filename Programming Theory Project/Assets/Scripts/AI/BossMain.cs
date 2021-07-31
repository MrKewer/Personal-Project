using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMain : EnemyMain
{
    public event DamageDealtHandler DamageDealt;
    // Start is called before the first frame update
    public override void Damage(float damageTaken, string damageType, Vector3 damageLocation)
    {
        base.Damage(damageTaken, damageType, damageLocation);

        if (DamageDealt != null)
        {
            DamageDealt(damageTaken);
        }
    }
}
