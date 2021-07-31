using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

//public delegate void DamageDealtHandler(float amount);
public class EnemyMain : MonoBehaviour, IDamageable<float, string, Vector3>
{
    protected Animator runAnimation;
    public float AnimationSpeed = 10;
    public float maxHealth;
    public float health;
    public float speed = 10f;
    public float forwardSpeed;
    public float backwardSpeed;
    protected float runSpeed = -10f;
    public float collisionDamage;
    protected GameObject player;
    protected PlayerController playerController;

    protected SpawnManager spawnManager;
    public float spawnPos = -14f;
    public float spawnPosRange = 3f;
    public float zRandom = 0;

    protected virtual void Awake()
    {
        //healthBarSize = healthBar.transform.localScale;
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        runAnimation = gameObject.GetComponent<Animator>();
    }
    protected virtual void OnEnable()
    {
        health = maxHealth;
        runSpeed = forwardSpeed;        

        //healthBar.transform.localScale = new Vector3(healthBarSize.x * (health / maxHealth), healthBarSize.y, healthBarSize.z);
    }

    protected virtual void Update()
    {
        Vector3 FollowDirection = (player.transform.position - transform.position).normalized;
        FollowDirection = new Vector3((FollowDirection.x * runSpeed) / 100, 0, (FollowDirection.z + zRandom));
        transform.Translate(FollowDirection * speed * Time.deltaTime);
         runAnimation.SetFloat("Speed_f", GameManager.Instance.gameSpeed * AnimationSpeed / 10);
        if (gameObject.transform.position.x <= spawnPos)
        {            
            runSpeed = forwardSpeed;
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
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
    protected virtual void Death()
    {
        gameObject.SetActive(false);
    }

    public virtual void Damage(float damageTaken, string damageType, Vector3 damageLocation)
    {
        health -= damageTaken;
        //healthBar.transform.localScale = new Vector3(healthBarSize.x * (health / maxHealth), healthBarSize.y, healthBarSize.z);
        if (health <= 0)
        {
            Death();
        }
        //if (damageType == "Collision")
        //{
        //    spawnManager.SpawnPartical("PurpleSmall", damageLocation);
        //    runSpeed = backwardSpeed;
        //}
    }
}
