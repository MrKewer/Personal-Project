using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMain : EnemyMain
{
    public event DamageDealtHandler DamageDealt; //This event will be used to update the InGameUI
    [SerializeField] protected Enums.Particles collisionParticle;
    public string BossName; //The name that will be displayed on the InGameUI

    public override void Damage(float damageTaken, Enums.DamageType damageType, Vector3 damageLocation)
    {
        base.Damage(damageTaken, damageType, damageLocation); //Will calculate new health and will call death function when health below 0

        if (DamageDealt != null)
        {
            DamageDealt(damageTaken); //Fire up the event
        }
    }
}
