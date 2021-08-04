using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : EnemyMain
{
    public GameObject healthBar;
    private Vector3 healthBarSize;


    protected override void Awake()
    {
        base.Awake();
        healthBarSize = healthBar.transform.localScale;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        healthBar.transform.localScale = new Vector3(healthBarSize.x * (health / maxHealth), healthBarSize.y, healthBarSize.z);
        zRandom = UnityEngine.Random.Range(-playerController.VerticalStep / 20, playerController.VerticalStep / 20);
    }

    public override void Damage(float damageTaken, string damageType, Vector3 damageLocation)
    {
        base.Damage(damageTaken, damageType, damageLocation);
        healthBar.transform.localScale = new Vector3(healthBarSize.x * (health / maxHealth), healthBarSize.y, healthBarSize.z);
        if (damageType == "Collision")
        {
            spawnManager.SpawnPartical("PurpleSmall", damageLocation);
            runSpeed = backwardSpeed;
        }
    }
    protected override void Death()
    {
        spawnManager.SpawnPartical("PurpleLarge", gameObject.transform.position);
        GameManager.Instance.EnemiesDead += 1;
        if (GameManager.Instance.EnemiesDead >= 20)
        {
            GameManager.Instance.BossFight();
        }

        base.Death();
    }

}
