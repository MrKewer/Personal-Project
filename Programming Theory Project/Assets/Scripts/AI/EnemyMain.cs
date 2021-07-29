using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//public delegate void DamageDealtHandler(float amount);
public class EnemyMain : MonoBehaviour, IDamageable<float>
{
    public float maxHealth = 100f;
    public float health = 100f;
    public float speed = 10f;
    public float forwardSpeed = 0f;
    private GameObject player;
    public GameObject healthBar;
    Vector3 healthBarSize;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        healthBarSize = healthBar.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 FollowDirection = (player.transform.position - transform.position).normalized;
        FollowDirection = new Vector3((FollowDirection.x * forwardSpeed) / 100, 0, FollowDirection.z);
        transform.Translate(FollowDirection * speed * Time.deltaTime);
    }

    void IDamageable<float>.Damage(float damageTaken)
    {
        health -= damageTaken;
        healthBar.transform.localScale = new Vector3(healthBarSize.x * (health / maxHealth), healthBarSize.y, healthBarSize.z);
        if (health <= 0)
        {
            Death();
        }
    }
    public void Death()
    {
        gameObject.SetActive(false);
    }
}
