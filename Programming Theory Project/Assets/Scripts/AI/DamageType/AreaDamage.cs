using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDamage : MonoBehaviour
{
    private SpawnManager spawnManager;
    [SerializeField] private float damagePerSecond;
    private bool bDoDamage = false;
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
                bDoDamage = true;
                StartCoroutine(DoDamagePerSecond(other, hit));
            }
            Debug.Log("Area Damage OnTriggerEnter");
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bDoDamage = false;
            Debug.Log("Area Damage OnTriggerExit");
        }

    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        IDamageable<float, string, Vector3> hit = (IDamageable<float, string, Vector3>)other.gameObject.GetComponent(typeof(IDamageable<float, string, Vector3>));
    //        if (hit != null)
    //        {
    //            StartCoroutine(DoDamagePerSecond(other, hit));
    //        }
    //    }
    //}
    IEnumerator DoDamagePerSecond(Collider other, IDamageable<float, string, Vector3> hit)
    {
        WaitForSeconds waitTime = new WaitForSeconds(1);
        while (bDoDamage)
        {
            yield return waitTime;
            hit.Damage(damagePerSecond, "Poison", other.gameObject.transform.position);
            spawnManager.SpawnPartical("GreenSmall", other.gameObject.transform.position);
            Debug.Log("Coroutine");
        }


    }
}
