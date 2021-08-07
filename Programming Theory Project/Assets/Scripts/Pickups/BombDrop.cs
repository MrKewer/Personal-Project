using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDrop : MonoBehaviour
{
    [SerializeField] private GameObject EnemyEnterZone;
    private SpawnManager spawnManager;

    private void Start()
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Boss")) //Does damage only if its the enemy or boss, Lets the player jump and run on the obstacles
        {
            spawnManager.SpawnExplosion(transform.position);
            gameObject.SetActive(false);
        }
    }

}
