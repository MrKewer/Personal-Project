using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

//public delegate void DamageDealtHandler(float amount);
public class EnemyMain : MonoBehaviour, IDamageable<float, Enums.DamageType, Vector3>
{
    protected Animator runAnimation; //Gets the run animation to control the speed of the animation if the background speed increase
    public float AnimationSpeed = 1; //Sets the speed of the animation
    public float maxHealth; //Used to reset health to be full
    public float health; //Used to show current health
    public float speed = 10f; //The speed used basically when changing lanes
    public float forwardSpeed; //The speed when the enemy runs towards player
    public float backwardSpeed; //The speed the enemy will run when going back(should be negative)
    [SerializeField] protected float runSpeed = -10f; //This speed is used to calculate current speed. ForwardSpeed or BackwardSpeed
    public float collisionDamage; //The damage the enemy will dealt when it hits the player
    protected GameObject player; //Reference to player
    protected PlayerController playerController; //Reference to playerController

    protected SpawnManager spawnManager; //Reference to Spawn Manager
    public float spawnPos = -14f; //The spawn Position
    public float spawnPosRange = 3f; //Random spawn position offset
    public float zRandom = 0; //Sets a z position for each enemy, so they will have more lanes to run in

    protected virtual void Awake()
    {
        //healthBarSize = healthBar.transform.localScale;
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        runAnimation = gameObject.GetComponent<Animator>();
        gameObject.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate; //This helps to make the enemies run through each other
    }
    protected virtual void OnEnable()
    {
        health = maxHealth; //Resets health when enemy will be pooled again
        runSpeed = forwardSpeed; //Resets runspeed when enemy will be pooled again
    }

    protected virtual void FixedUpdate()
    {
        Vector3 FollowDirection = (player.transform.position - transform.position).normalized; //Get the direction of this character and the player
        FollowDirection = new Vector3((FollowDirection.x * runSpeed) / 100, 0, (FollowDirection.z + zRandom)); //Setup the vector to only use the x direction
        transform.Translate(FollowDirection * speed * Time.deltaTime); //Translate the character to the location
        runAnimation.SetFloat("Speed_f", GameManager.Instance.gameSpeed * AnimationSpeed / 10); //Set the run animation to sinc in with game speed
        if (gameObject.transform.position.x <= spawnPos || FollowDirection.x < -1) //If running back and gets to spawn position then run forward, when FollowDirection.x is negative it must prevent from running past player
        {            
            runSpeed = forwardSpeed;
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0]; 
        Vector3 pos = contact.point; //Get the position's point of the collision contact
        if (collision.gameObject.CompareTag("Player"))
        {
            IDamageable<float, Enums.DamageType, Vector3> hit = (IDamageable<float, Enums.DamageType, Vector3>)collision.gameObject.GetComponent(typeof(IDamageable<float, Enums.DamageType, Vector3>));
            if (hit != null)
            {
                hit.Damage(collisionDamage, Enums.DamageType.Bite, pos); //Do damage to player
                spawnManager.SpawnParticle(Enums.Particles.RedSmall, pos); //Spawn Particle
                runSpeed = backwardSpeed; //Run backwards
            }
        }
    }

    protected virtual void Death()
    {
        playerController.UpdateScore((int)maxHealth);
        gameObject.SetActive(false); //Set enemy Deactive
    }

    public virtual void Damage(float damageTaken, Enums.DamageType damageType, Vector3 damageLocation)
    {
        health -= damageTaken; //Calculate new health
        if (health <= 0) //Death if health below 0
        {
            Death();
        }
    }
}
