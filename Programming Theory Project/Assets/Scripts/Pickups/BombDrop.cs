using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDrop : MonoBehaviour
{
    [SerializeField] private GameObject EnemyEnterZone; //Extra sphere around the bomb, to have a bigger change to hit something
    private SpawnManager spawnManager; 

    private void Start()
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Boss"))
        {
            spawnManager.SpawnExplosion(transform.position);
            gameObject.SetActive(false);
        }
    }

}
