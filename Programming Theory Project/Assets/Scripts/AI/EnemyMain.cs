using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//public delegate void DamageDealtHandler(float amount);
public class EnemyMain : MonoBehaviour, IDamageable<float, string, Vector3>
{
    public float maxHealth = 100f;
    public float health = 100f;
    public float speed = 10f;
    public float forwardSpeed = 5f;
    public float backwardSpeed = -10f;
    private float runSpeed = -10f;
    public float collisionDamage = 30f;
    private GameObject player;
    public GameObject healthBar;
    Vector3 healthBarSize;
    private SpawnManager spawnManager;
    private float spawnPos = -14f;

    void Start()
    {
        runSpeed = forwardSpeed;
        player = GameObject.Find("Player");
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        healthBarSize = healthBar.transform.localScale;
    }

    void Update()
    {
        Vector3 FollowDirection = (player.transform.position - transform.position).normalized;
        FollowDirection = new Vector3((FollowDirection.x * runSpeed) / 100, 0, FollowDirection.z);
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
        }
    }
}
