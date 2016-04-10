using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour {

    //[SyncVar]
    public bool isSpawning = false;

    public GameObject[] ObjectsToSpawn;

    public float spawnMin = 2.0f;
    public float spawnMax = 4.0f;

    public float speed = 10;
    public float horizontalVariation = 1;

    public enum MovementDirectionEnum { Left, Right }
    public MovementDirectionEnum MovementDirection = MovementDirectionEnum.Left;

	// Use this for initialization
	void Start () {
        Invoke("Spawn", Random.Range(spawnMin, spawnMax));
	}
	
	// Update is called once per frame
	void Spawn () {
        if (isSpawning && isServer)
        {
            if (ObjectsToSpawn.Length > 0)
            {
                //RpcSpawnEnemy(ObjectsToSpawn[Random.Range(0, ObjectsToSpawn.Length)]);
                RpcSpawnEnemy(0);
            }
        }
        Invoke("Spawn", Random.Range(spawnMin, spawnMax));
    }

    public override float GetNetworkSendInterval()
    {
        return 0;
    }

    [ClientRpc]
    void RpcSpawnEnemy(int index)
    {
        Debug.Log("Spawning enemy");
        GameObject o = (GameObject)Instantiate(ObjectsToSpawn[index], transform.position, Quaternion.identity);

        if (o != null)
        {
            var mover = o.GetComponent<EnemyMover>();

            if (mover != null)
            {
                mover.SetSpeedAndHoriz(speed, horizontalVariation);

                switch (MovementDirection)
                {
                    case MovementDirectionEnum.Left:
                        mover.SetMovementDirection(EnemyMover.MovementDirectionEnum.Left);
                        break;
                    case MovementDirectionEnum.Right:
                        mover.SetMovementDirection(EnemyMover.MovementDirectionEnum.Right);
                        break;
                }
            }
            else
            {
                Debug.Log("Error: mover is null");
            }
        }
        else
        {
            Debug.Log("Error: o is null");
        }
    }
}
