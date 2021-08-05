using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRacoon : BossMain
{
    [SerializeField] private bool bInPos = false; //In Position to do ability attack
    [SerializeField] private GameObject blackBreathPrefab; //The Particle attached to the character
    [SerializeField] private GameObject blackBreathColliderPrefab; //The box collider attached
    private float healthAttackInterval = -50; //When the health gets this amount lower than before, it will do the ability 
    private float nextAttackHealth = 0; //Keep track of the new health to reach
    private float abilityTime = 5; //The amount of seconds the boss will do the ability
    private float xPositionForAbility = -10;

    protected override void Awake()
    {
        base.Awake(); //Keeps the same Awake as the parent
        nextAttackHealth = health + healthAttackInterval; //Calculate the health amount that will cause the boss to use its ability
        blackBreathPrefab.SetActive(false); //Set the effects inactive
        blackBreathColliderPrefab.SetActive(false); //set the collider inactive
    }
    protected override void Update()
    {
        if (health > nextAttackHealth) //Do the normal routine when health still good
        {
            base.Update();
        }
        else
        {
            if (bInPos) //If in position use ability
            {
                blackBreathPrefab.SetActive(true);
                blackBreathColliderPrefab.SetActive(true);
                StartCoroutine(CoAbilityAttack());
            }
            else //Get in position to use ability
            {
                runSpeed = -40; //Set to a large value to get in position quicker
                Vector3 FollowDirection = (player.transform.position - transform.position).normalized; //Get the direction of this character and the player
                FollowDirection = new Vector3((FollowDirection.x * runSpeed) / 100, 0, 0); //Setup the vector to only use the x direction
                transform.Translate(FollowDirection * speed * Time.deltaTime); //Translate the character to the location
                runAnimation.SetFloat("Speed_f", GameManager.Instance.gameSpeed * AnimationSpeed / 10); //set the run animation to sinc in with game speed
                if (gameObject.transform.position.x <= xPositionForAbility) //If in the position to start using ability
                {
                    runSpeed = 1;
                    bInPos = true;
                }
            }
        }
    }
    IEnumerator CoAbilityAttack()
    {
        yield return new WaitForSeconds(abilityTime); //Wait a few seconds before disabling the ability
        //Stop ability and reset all
        bInPos = false;
        nextAttackHealth = health + healthAttackInterval; //The next health amount before using ability again
        runSpeed = forwardSpeed;
        blackBreathPrefab.SetActive(false);
        blackBreathColliderPrefab.SetActive(false);
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision); //Use parrent collision method
        ContactPoint contact = collision.contacts[0];
        Vector3 pos = contact.point; //Get the position's point of the collision contact
        if (collision.gameObject.CompareTag("Enemy")) //If the boss runs over an enemy, just destroy the enemy instantly
        {
            IDamageable<float, Enums.DamageType, Vector3> hit = (IDamageable<float, Enums.DamageType, Vector3>)collision.gameObject.GetComponent(typeof(IDamageable<float, Enums.DamageType, Vector3>));
            if (hit != null)
            {
                hit.Damage(400, Enums.DamageType.Collision, pos);
            }
        }
    }
    public override void Damage(float damageTaken, Enums.DamageType damageType, Vector3 damageLocation)
    {
        base.Damage(damageTaken, damageType, damageLocation); //Use parents damage method

        if (damageType == Enums.DamageType.Collision) //When it collides with an obsticle run back and do particle effect
        {
            spawnManager.SpawnParticle(collisionParticle, damageLocation);
            runSpeed = backwardSpeed;
        }
    }
    protected override void Death()
    {
        spawnManager.SpawnParticle(Enums.Particals.BlueLarge, gameObject.transform.position); // Spawn Particle
        GameManager.Instance.EndGame(); //Tell the game manager the game has ended
        spawnManager.BossDeath(gameObject.transform.position); //Tell the spawn manager the boss is dead, will spawn extra particle effects
        base.Death(); //Parrent death will deactivate this game object
    }
}