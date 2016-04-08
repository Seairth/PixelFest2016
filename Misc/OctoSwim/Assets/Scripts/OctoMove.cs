using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class OctoMove : NetworkBehaviour {

    private Rigidbody2D body;
    [SyncVar(hook = "UpdateRot")]
    public float SpriteRot;

    public float force = 0;
    public float turnRate = 0;

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
	void FixedUpdate () {
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
            //SpriteRot = ;
            //CmdSetSpriteRot(new Vector3(0, 0, turnRate));
            body.rotation += turnRate;
            CmdSetSpriteRot(body.rotation);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            //SpriteRot = ;
            //CmdSetSpriteRot(new Vector3(0, 0, -turnRate));
            body.rotation -= turnRate;
            CmdSetSpriteRot(body.rotation);
        }
        else
        {
            //SpriteRot = new Vector3(0, 0, 0);
        }
    }

    [Command]
    void CmdSetSpriteRot(float rot)
    {
        SpriteRot = rot;
    }

    void UpdateRot(float rot)
    {
        //transform.Rotate(SpriteRot);
        body.rotation = rot;
        //Debug.Log(string.Format("newpos: x: {0} y: {1} z: {2}", newpos.x, newpos.y, newpos.z));
    }
}
