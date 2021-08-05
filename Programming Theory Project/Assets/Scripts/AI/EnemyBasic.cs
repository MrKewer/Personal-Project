using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyBasic : EnemyMain
{
    public GameObject healthBar;
    private Vector3 healthBarSize;
    [SerializeField] protected TextMeshPro healthDisplayText;

    protected override void Awake()
    {
        base.Awake();
        healthBarSize = healthBar.transform.localScale;
        healthDisplayText.text = health + "/" + maxHealth;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameManager.Instance.basicEnemiesCount++;
        healthBar.transform.localScale = new Vector3(healthBarSize.x * (health / maxHealth), healthBarSize.y, healthBarSize.z);
        healthDisplayText.text = health + "/" + maxHealth;
        zRandom = UnityEngine.Random.Range(-playerController.VerticalStep / 20, playerController.VerticalStep / 20);
    }

    public override void Damage(float damageTaken, Enums.DamageType damageType, Vector3 damageLocation)
    {
        base.Damage(damageTaken, damageType, damageLocation);
        healthDisplayText.text = health + "/" + maxHealth;
        healthBar.transform.localScale = new Vector3(healthBarSize.x * (health / maxHealth), healthBarSize.y, healthBarSize.z);
        if (damageType == Enums.DamageType.Collision)
        {
            spawnManager.SpawnParticle(Enums.Particals.PurpleSmall, damageLocation);
            runSpeed = backwardSpeed;
        }
    }
    protected override void Death()
    {
        spawnManager.SpawnParticle(Enums.Particals.PurpleLarge, gameObject.transform.position);
        GameManager.Instance.enemiesDead += 1;
        if (GameManager.Instance.enemiesDead >= GameManager.Instance.enemiesDeadBeforeBossFight)
        {
            GameManager.Instance.BossFight();
        }

        base.Death();
    }
    protected void OnDisable()
    {
        GameManager.Instance.basicEnemiesCount--;
    }
}
