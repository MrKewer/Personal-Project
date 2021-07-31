using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPugDog : BossMain
{
    [SerializeField] private bool bInPos = false;
    [SerializeField] private GameObject stinkBreathPrefab;
    [SerializeField] private GameObject stinkBreathColliderPrefab;
    private float healthAttackInterval = -50;
    private float nextAttackHealth = 0;
    protected override void Awake()
    {
        base.Awake();
        nextAttackHealth = health + healthAttackInterval;
        stinkBreathPrefab.SetActive(false);
        stinkBreathColliderPrefab.SetActive(false);
    }
    protected override void Update()
    {
        if (health > nextAttackHealth)
        {
            base.Update();
        }
        else
        {
            if (bInPos)
            {
                stinkBreathPrefab.SetActive(true);
                stinkBreathColliderPrefab.SetActive(true);
                StartCoroutine(CoAbilityAttack());
            }
            else
            {
                runSpeed = -40;
                Vector3 FollowDirection = (player.transform.position - transform.position).normalized;
                FollowDirection = new Vector3((FollowDirection.x * runSpeed) / 100, 0, 0);
                transform.Translate(FollowDirection * speed * Time.deltaTime);
                runAnimation.SetFloat("Speed_f", GameManager.Instance.gameSpeed * AnimationSpeed / 10);
                if (gameObject.transform.position.x <= -10)
                {
                    runSpeed = 1;
                    bInPos = true;
                }
            }
        }        
    }
    IEnumerator CoAbilityAttack()
    {
        yield return new WaitForSeconds(4);
        bInPos = false;
        nextAttackHealth = health + healthAttackInterval;
        runSpeed = forwardSpeed;
        stinkBreathPrefab.SetActive(false);
        stinkBreathColliderPrefab.SetActive(false);
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        ContactPoint contact = collision.contacts[0];
        Vector3 pos = contact.point;
        if (collision.gameObject.CompareTag("Enemy"))
        {
            IDamageable<float, string, Vector3> hit = (IDamageable<float, string, Vector3>)collision.gameObject.GetComponent(typeof(IDamageable<float, string, Vector3>));
            if (hit != null)
            {
                hit.Damage(200, "Collision", pos);
            }
        }
    }
    public override void Damage(float damageTaken, string damageType, Vector3 damageLocation)
    {
        base.Damage(damageTaken, damageType, damageLocation);

        if (damageType == "Collision")
        {
            spawnManager.SpawnPartical("BlueSmall", damageLocation);
            runSpeed = backwardSpeed;
        }
    }
    protected override void Death()
    {
        spawnManager.SpawnPartical("BlueLarge", gameObject.transform.position);
        GameManager.Instance.EndGame();
        spawnManager.BossDeath(gameObject.transform.position);
        base.Death();
    }
}
