using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameObject characterSelected;
    [SerializeField] private GameObject CharacterListPrefab;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 characterTransform = new Vector3(0, 0, 0);
        Quaternion characterRotation = Quaternion.Euler(0, 90, 0);
        characterSelected = CharacterListPrefab.GetComponent<CharacterList>().characterList[1];
        characterSelected = Instantiate(characterSelected,characterTransform, characterRotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
