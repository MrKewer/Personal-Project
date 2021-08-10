using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDamage : MonoBehaviour
{
    private SpawnManager spawnManager; //Reference to Spawn Manager
    [SerializeField] private float damagePerSecond; //The amount of damage to deal
    private bool bDoDamage = false; // When damage will be aplied each second, this keeps the loop running in the coroutine
    [SerializeField] private Enums.Particles particleToSpawnOnDamage; //The Type of particle to spawn when damage is dealt

    void Awake()
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>(); //Gets reference to Spawn Manager
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Obstacle"))
        {
            other.gameObject.transform.parent.gameObject.SetActive(false); //Deactivate the obstacle 
            spawnManager.SpawnParticle(particleToSpawnOnDamage, other.gameObject.transform.position); //Show particle 

        }
        if (other.CompareTag("Player")) //Do damage per second to player
        {
            IDamageable<float, Enums.DamageType, Vector3> hit = (IDamageable<float, Enums.DamageType, Vector3>)other.gameObject.GetComponent(typeof(IDamageable<float, Enums.DamageType, Vector3>));
            if (hit != null)
            {
                bDoDamage = true; //When the player has enter the colider
                StartCoroutine(DoDamagePerSecond(other, hit));
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bDoDamage = false; //When the player has exit the colider
        }

    }

    IEnumerator DoDamagePerSecond(Collider other, IDamageable<float, Enums.DamageType, Vector3> hit)
    {
        WaitForSeconds waitTime = new WaitForSeconds(1);
        while (bDoDamage)
        {
            yield return waitTime;
            hit.Damage(damagePerSecond, Enums.DamageType.Poison, other.gameObject.transform.position); //Do damage to player
            spawnManager.SpawnParticle(particleToSpawnOnDamage, other.gameObject.transform.position); //Spawn Particle
        }
    }
}
