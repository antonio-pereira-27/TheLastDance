using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonSpwanSystem : MonoBehaviour
{
    // VARIABLES
    private int totalPersons = 0;
    private int personsSpawned = 0;
    
    // REFERENCES
    public GameObject[] personsPrefabs;
    public Transform[] spawnPoints;

    public Transform exit;
    
    
    // Start is called before the first frame update
    void Start()
    {
        totalPersons = spawnPoints.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (personsSpawned < totalPersons)
            InstantiatePersons();
    }

    private void InstantiatePersons()
    {
        for (int i = 0; i < totalPersons; i++)
        {
            Transform spawnPoint = spawnPoints[i];
            GameObject person = Instantiate(personsPrefabs[Random.Range(0, personsPrefabs.Length)], spawnPoint.position ,Quaternion.identity);
            Normal personNPC = person.GetComponent<Normal>();
            personNPC.exit = exit;

            personsSpawned++;
        }
    }
}
