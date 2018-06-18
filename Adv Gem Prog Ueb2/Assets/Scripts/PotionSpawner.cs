using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionSpawner : MonoBehaviour {

    [Tooltip("assign the same Potion twice to make it more often")]
    public GameObject[] PotionPrefabs;

    private GameObject currentPotion;
    public float spawnTime = 10f;

	// Use this for initialization
	void Start () {
        StartCoroutine("Spawn");
	}

    

    IEnumerator Spawn()
    {
        while (true)
        {
            if (currentPotion != null) Destroy(currentPotion);
            currentPotion = Instantiate(PotionPrefabs[Random.Range(0, PotionPrefabs.Length)], transform.position, transform.rotation);
            yield return new WaitForSeconds(spawnTime);
        }
    }

}
