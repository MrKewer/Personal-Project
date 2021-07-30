using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

//public delegate void DamageDealtHandler(float amount);
public class EnemyMain : MonoBehaviour, IDamageable<float, string, Vector3>
{
    public float maxHealth;
    public float health;
    public float speed = 10f;
    public float forwardSpeed;
    public float backwardSpeed;
    private float runSpeed = -10f;
    public float collisionDamage;
    private GameObject player;
    private PlayerController playerController;
    public GameObject healthBar;
    private Vector3 healthBarSize;
    private SpawnManager spawnManager;
    public float spawnPos = -14f;
    public float spawnPosRange = 3f;
    public float zRandom;

    private void Awake()
    {
        healthBarSize = healthBar.transform.localScale;
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }
    private void OnEnable()
    {
        health = maxHealth;
        runSpeed = forwardSpeed;        
        zRandom = UnityEngine.Random.Range(-playerController.VerticalStep / 20, playerController.VerticalStep / 20);
        healthBar.transform.localScale = new Vector3(healthBarSize.x * (health / maxHealth), healthBarSize.y, healthBarSize.z);
    }

    void Update()
    {
        Vector3 FollowDirection = (player.transform.position - transform.position).normalized;
        FollowDirection = new Vector3((FollowDirection.x * runSpeed) / 100, 0, (FollowDirection.z + zRandom));
        transform.Translate(FollowDirection * speed * Time.deltaTime);
        if(gameObject.transform.position.x <= spawnPos)
        {            
            runSpeed = forwardSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Vector3 pos = contact.point;
        if (collision.gameObject.CompareTag("Player"))
        {
            IDamageable<float, string, Vector3> hit = (IDamageable<float, string, Vector3>)collision.gameObject.GetComponent(typeof(IDamageable<float, string, Vector3>));
            if (hit != null)
            {
                hit.Damage(collisionDamage, "Byte", pos);
                spawnManager.SpawnPartical("RedSmall", pos);
                runSpeed = backwardSpeed;                
            }
        }
    }
    public void Death()
    {
        spawnManager.SpawnPartical("PurpleLarge", gameObject.transform.position);
        GameManager.Instance.EnemiesDead += 1;
        if (GameManager.Instance.EnemiesDead >= 2)
        {
            GameManager.Instance.BossFight();
        }
        gameObject.SetActive(false);
    }

    public void Damage(float damageTaken, string damageType, Vector3 damageLocation)
    {
        health -= damageTaken;
        healthBar.transform.localScale = new Vector3(healthBarSize.x * (health / maxHealth), healthBarSize.y, healthBarSize.z);
        if (health <= 0)
        {
            Death();
        }
        if (damageType == "Collision")
        {
            spawnManager.SpawnPartical("PurpleSmall", damageLocation);
            runSpeed = backwardSpeed;
        }
    }
}
