using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class OctoMove : NetworkBehaviour {

    private Rigidbody2D body;
    //[SyncVar(hook = "UpdateRot")]
    //public float SpriteRot;

    public float force = 0;
    public float turnRate = 0;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    private float lastRotation = 0;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();

        if (isLocalPlayer)
        {
            foreach (var src in GetComponents<AudioSource>())
            {
                src.Play();
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (isLocalPlayer)
        {
            Control();   
        }
    }

    void Control()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            body.AddForce(transform.up * force);

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            body.AddTorque(turnRate);
            //body.rotation = lastRotation + turnRate;
            //lastRotation = body.rotation;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            body.AddTorque(-turnRate);
            //body.rotation = lastRotation - turnRate;
            //lastRotation = body.rotation;
        }
        else
        {
            //if (body.rotation != lastRotation)
            //    body.rotation = lastRotation;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
        }
    }

    [Command]
    void CmdFire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // Add velocity to the bullet
        var b = bullet.GetComponent<Rigidbody2D>();
        b.velocity = bullet.transform.up * 6;

        NetworkServer.Spawn(bullet);

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }

    //[Command]
    void CmdSetSpriteRot(float rot)
    {
        //SpriteRot = rot;
    }

    void UpdateRot(float rot)
    {
        //transform.Rotate(SpriteRot);
        //body.rotation = rot;
        //Debug.Log(string.Format("newpos: x: {0} y: {1} z: {2}", newpos.x, newpos.y, newpos.z));
    }
}
