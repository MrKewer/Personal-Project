using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDamage : MonoBehaviour
{
    private SpawnManager spawnManager;
    [SerializeField] private float damage;
    void Awake()
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        //ContactPoint contact = other.contactOffset();
        //Vector3 pos = contact.point;
        if (other.CompareTag("Obstacle"))
        {
            other.gameObject.transform.parent.gameObject.SetActive(false);
            spawnManager.SpawnPartical("GreenSmall", other.gameObject.transform.position);

        }
        if (other.CompareTag("Player"))
        {
            IDamageable<float, string, Vector3> hit = (IDamageable<float, string, Vector3>)other.gameObject.GetComponent(typeof(IDamageable<float, string, Vector3>));
            if (hit != null)
            {
                hit.Damage(damage, "Byte", other.gameObject.transform.position);
                spawnManager.SpawnPartical("GreenSmall", other.gameObject.transform.position);
            }
            Debug.Log("Area Damage");

        }

    }
}
