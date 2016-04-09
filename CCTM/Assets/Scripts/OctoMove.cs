using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class OctoMove : NetworkBehaviour {

    private Rigidbody2D body;
    //[SyncVar(hook = "UpdateRot")]
    //public float SpriteRot;
    [SyncVar(hook = "PlayerNumChanged")]
    public int PlayerNum = 0;

    public float forwardForce = 0;
    public float backwardForce = 0;
    
    public float turnRate = 0;

    public float camDampTime = 0.15f;
    private Vector3 camVelocity = Vector3.zero;

    [SyncVar(hook = "TreasuresHoldingChanged")]
    public int numTreasuresHolding = 0;
    public int MaxTreasuresCanHold = 3;

	//public GameObject bulletPrefab;
	//public Transform bulletSpawn;

	[SyncVar]
	bool playing = false;


    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody2D>();

        if (isLocalPlayer)
        {
            var sc = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SmoothCamera2D>();

            if (sc)
            {
                sc.target = body.transform;
            }
            //foreach (var src in GetComponents<AudioSource>())
            //{
            //    src.Play();
            //}
        }

        if (isServer)
        {
            var nmh = GameObject.FindObjectOfType<NetworkManagerHelper>();

            if (nmh != null)
            {
                PlayerNum = nmh.SetPlayerNum(this.gameObject);
                Debug.Log("Server: Player num is " + PlayerNum);
            }
            else
            {
                Debug.Log("Server: Error getting NHM");
            }
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (isLocalPlayer && playing)
        {
            Control();
        }
    }

	void OnGUI()
	{
		if (! playing)
		{
			var x = Screen.width / 2;
			var y = Screen.height / 2;

			if (isServer)
			{
				x -= 160 / 2;
				y -= 30 / 2;

				if (GUI.Button(new Rect(x, y, 160, 30), "START"))
					playing = true;
			}
			else
			{
				var style = new GUIStyle();
				style.fontSize = 24;
				style.fontStyle = FontStyle.Bold;
				style.alignment = TextAnchor.MiddleCenter;
				style.normal.textColor = Color.white;

				GUI.Label(new Rect(x, y, 0, 0), "WAITING FOR START...", style);
			}
		}
	}


    void Control()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            body.AddForce(transform.up * forwardForce);
        else if (Input.GetKey(KeyCode.DownArrow))
            body.AddForce(transform.up * -backwardForce);

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

    public void AddSingleTreasure()
    {
        numTreasuresHolding++;
    }

    public void DropSingleTreasure()
    {
        numTreasuresHolding--;
    }

    public void DropOffAllTreasure()
    {
        numTreasuresHolding = 0;
    }

    public void TreasuresHoldingChanged(int treasures)
    {

    }

    [Command]
    void CmdPickUpTreasure()
    {
        
    }

    [Command]
    void CmdFire()
    {
        /*
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
        */
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

    void OnCollisionEnter2D(Collision2D coll)
    {
        //Debug.Log("Player collide");
    }

    void PlayerNumChanged(int newPlayerNum)
    {
        if (isLocalPlayer)
        {
            Debug.Log("Local player num is now " + newPlayerNum);
        }

        var render = GetComponent<SpriteRenderer>();

        if (render != null)
        {
            switch (newPlayerNum)
            {
                case 1:
                    break;
                case 2:
                    render.color = Color.blue;
                    break;
                case 3:
                    render.color = Color.red;
                    break;
                case 4:
                    render.color = Color.green;
                    break;
            }
        }

    }
}
