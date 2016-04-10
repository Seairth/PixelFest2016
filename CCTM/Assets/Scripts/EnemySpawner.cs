using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour {

    [SyncVar]
    public bool isSpawning = false;

    GameObject[] obj;

    public float spawnMin = 2.0f;
    public float spawnMax = 4.0f;

	// Use this for initialization
	void Start () {
        Invoke("Spawn", Random.Range(spawnMin, spawnMax));
	}
	
	// Update is called once per frame
	void Spawn () {
        if (isSpawning)
        {
            GameObject o = (GameObject)Instantiate(obj[Random.Range(0, obj.Length)], transform.position, Quaternion.identity);

            
        }
        Invoke("Spawn", Random.Range(spawnMin, spawnMax));
    }
}
