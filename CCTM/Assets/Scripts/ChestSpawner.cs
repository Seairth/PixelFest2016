using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ChestSpawner : NetworkBehaviour {

    public bool isSpawning = false;
    private bool lastIsSpawning = false;
    public GameObject objectToSpawn = null;
    GameObject spawnedObject = null;

    public float spawnMin = 10.0f;
    public float spawnMax = 20.0f;
    public float despawnMin = 15.0f;
    public float despawnMax = 20.0f;

    // Use this for initialization
    void Start () {
        //this.gameObject.AddComponent<ChestController>();
        //obj = gameObject.GetComponent<>();

        if (objectToSpawn == null)
        {
            Debug.Log("ERROR: coulnd't init chest object");
        }
        SpawnAfterRandomDelay();
	}
	
	// Update is called once per frame
	void Update () {
        /*
        if (!lastIsSpawning && isSpawning)
        {
            Start();
        }

        if (lastIsSpawning && !isSpawning)
        {
            lastIsSpawning = isSpawning;
        }
        */
	}

    public override float GetNetworkSendInterval()
    {
        return 0;
    }

    void SpawnAfterRandomDelay()
    {
        float delay = Random.Range(spawnMin, spawnMax);
        Debug.Log("Spawning chest after " + delay + " seconds.");
        Invoke("SpawnNow", delay);
    }

    void SpawnNow()
    {
        if (isSpawning)// && isServer)
        {
            if (objectToSpawn != null)
            {
                RpcSpawnChest();
            }
            else
            {
                Debug.Log("ERROR: Not spawning chest: obj is null");
            }
        }
    }

    [ClientRpc]
    void RpcSpawnChest()
    {
        if (objectToSpawn != null)
        {
            Debug.Log("Spawning chest");
            spawnedObject = (GameObject)Instantiate(objectToSpawn, transform.position, Quaternion.identity);

            if (spawnedObject == null)
            {
                Debug.Log("ERROR: Spawned chest was null");
            }

            float des = Random.Range(despawnMin, despawnMax);
            Debug.Log("Despawning chest after " + des + " seconds.");
            Invoke("RpcDespawnChest", des);
        }
        else
        {
            Debug.Log("Not spawning chest: obj is null");
        }
    }

    [ClientRpc]
    void RpcDespawnChest()
    {
        if (spawnedObject != null)
        {
            Destroy(spawnedObject);
            spawnedObject = null;
        }

        SpawnAfterRandomDelay();
    }
}
