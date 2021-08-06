using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyBasic : EnemyMain
{
    public GameObject healthBar; //The displayed health bar each enemy has attached
    private Vector3 healthBarSize; //The initial size of the health bar, to reset when enemy is pooled
    [SerializeField] protected TextMeshPro healthDisplayText; //This shows the amount of health with the max health

    protected override void Awake()
    {
        base.Awake();
        healthBarSize = healthBar.transform.localScale; //Gets the initial size
        healthDisplayText.text = health + "/" + maxHealth; //Sets the text to current health
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameManager.Instance.basicEnemiesCount++; //Count the amount of enemies are active
        healthBar.transform.localScale = new Vector3(healthBarSize.x * (health / maxHealth), healthBarSize.y, healthBarSize.z); //Sets the healthbar image to the size of the current health
        healthDisplayText.text = health + "/" + maxHealth; //Sets the text to current health
        zRandom = UnityEngine.Random.Range(-playerController.VerticalStep / 20, playerController.VerticalStep / 20); //Sets a z position for each enemy, so they will have more lanes to run in
    }

    public override void Damage(float damageTaken, Enums.DamageType damageType, Vector3 damageLocation)
    {
        base.Damage(damageTaken, damageType, damageLocation); //Will calculate new health and will call death function when health below 0
        healthDisplayText.text = health + "/" + maxHealth; //Update the text to current health
        healthBar.transform.localScale = new Vector3(healthBarSize.x * (health / maxHealth), healthBarSize.y, healthBarSize.z); //Sets the healthbar image to the size of the current health
        if (damageType == Enums.DamageType.Collision) //When the enemy collide with obstacle, a particle will spawn and the enemy will run backwards
        {
            spawnManager.SpawnParticle(Enums.Particals.PurpleSmall, damageLocation);
            runSpeed = backwardSpeed;
        }
    }
    protected override void Death()
    {
        spawnManager.SpawnParticle(Enums.Particals.PurpleLarge, gameObject.transform.position); //Spawn particle on death
        GameManager.Instance.enemiesDead += 1; //Calculate how many you have killed
        if (GameManager.Instance.enemiesDead >= GameManager.Instance.enemiesDeadBeforeBossFight) //When the boss will come out
        {
            GameManager.Instance.BossFight();
        }

        base.Death(); //Will disable the enemy
    }
    protected void OnDisable()
    {
        GameManager.Instance.basicEnemiesCount--; //Count the amount of enemies are active
    }
}
