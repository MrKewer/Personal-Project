using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

//public delegate void DamageDealtHandler(float amount);
public class EnemyMain : MonoBehaviour, IDamageable<float, Enums.DamageType, Vector3>
{

    protected Animator runAnimation;
    public float AnimationSpeed = 1;
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
        Vector3 FollowDirection = (player.transform.position - transform.position).normalized; //Get the direction of this character and the player
        FollowDirection = new Vector3((FollowDirection.x * runSpeed) / 100, 0, (FollowDirection.z + zRandom)); //Setup the vector to only use the x direction
        transform.Translate(FollowDirection * speed * Time.deltaTime); //Translate the character to the location
        runAnimation.SetFloat("Speed_f", GameManager.Instance.gameSpeed * AnimationSpeed / 10); //Set the run animation to sinc in with game speed
        if (gameObject.transform.position.x <= spawnPos)
        {            
            runSpeed = forwardSpeed;
        }
        if(runSpeed < 0)
        {
            gameObject.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate; //When they run back they will run through the other enemies
        }
        else
        {
            gameObject.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Extrapolate;
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Vector3 pos = contact.point;
        if (collision.gameObject.CompareTag("Player"))
        {
            IDamageable<float, Enums.DamageType, Vector3> hit = (IDamageable<float, Enums.DamageType, Vector3>)collision.gameObject.GetComponent(typeof(IDamageable<float, Enums.DamageType, Vector3>));
            if (hit != null)
            {
                hit.Damage(collisionDamage, Enums.DamageType.Bite, pos);
                spawnManager.SpawnParticle(Enums.Particals.RedSmall, pos);
                runSpeed = backwardSpeed;                
            }
        }
    }

    protected virtual void Death()
    {
        gameObject.SetActive(false);
    }

    public virtual void Damage(float damageTaken, Enums.DamageType damageType, Vector3 damageLocation)
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
