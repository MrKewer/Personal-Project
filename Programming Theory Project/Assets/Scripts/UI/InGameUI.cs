using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private GameObject playerNameText;
    [SerializeField] private GameObject playerHealthBar;
    [SerializeField] private GameObject scoreText;
    [SerializeField] private GameObject timeText;
    [SerializeField] private GameObject powerupIndicator;
    [SerializeField] private List <GameObject> powerupIndicatorTime;

    [SerializeField] private GameObject bossNameText;
    [SerializeField] private GameObject bossHealthBar;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
