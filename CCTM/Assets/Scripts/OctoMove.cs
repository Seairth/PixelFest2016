using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;

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

    [SyncVar(hook = "TreasuresCollectedChanged")]
    public int treasuresCollected = 0;

    public float GravityPerTreasureMultiplier = 1;

    public Text myScoreText = null;

	//public GameObject bulletPrefab;
	//public Transform bulletSpawn;

	[SyncVar]
	bool playing = false;

	private Animator anim;
    private GameObject spawnLoc = null;

    DateTimeOffset lastPlayingCheck = DateTimeOffset.MinValue;
    DateTimeOffset lastEnemyCollision = DateTimeOffset.MinValue;

    public float hitCooldownSeconds = 0.75f;

    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();

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

        if (PlayerNum > 0)
        {
            SetPlayerColor(PlayerNum);
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (isLocalPlayer && playing)
        {
            Control();
        }

        if (!playing)
        {
            MoveLocalPlayerToSpawn();
        }
    }

    public void MoveLocalPlayerToSpawn()
    {
        if (isLocalPlayer)
        {
            if (spawnLoc != null)
            {
                if ((body.position.x != spawnLoc.transform.position.x) ||
                    (body.position.y != spawnLoc.transform.position.y))
                {
                    Debug.Log("Setting player inital position");
                    body.position = spawnLoc.transform.position;
                }
            }
            else
            {
                Debug.Log("Player spawn position is null");
            }
        }
    }

	void OnGUI()
	{
        if (isLocalPlayer)
        {
            if (!playing)
            {
                var x = Screen.width / 2;
                var y = Screen.height / 2;

                if (isServer)
                {
                    x -= 160 / 2;
                    y -= 30 / 2;

                    if (GUI.Button(new Rect(x, y, 160, 30), "START"))
                    {
                        playing = true;
                        RpcPlaying(true);
                    }
                }
                else
                {
                    /*
                    if (DateTimeOffset.UtcNow.Subtract(lastPlayingCheck).TotalSeconds > 1)
                    {
                        bool run = false;

                        foreach (var v in FindObjectsOfType<OctoMove>())
                        {
                            if (v.IsPlaying())
                            {
                                run = true;
                                break;
                            }
                        }

                        if (run)
                        {
                            playing = true;
                        }

                        lastPlayingCheck = DateTimeOffset.UtcNow;
                    }
                    */

                    if (!playing)
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
        }
	}


    void Control()
    {
		anim.SetBool("PickUp", false);
		anim.SetBool("PutDown", false);

		var v = Input.GetAxis("Vertical");

		if (v > 0)
			body.AddForce(transform.up * forwardForce * v);
		else if (v < 0)
			body.AddForce(transform.up * backwardForce * v);

		anim.SetFloat("Speed", GetComponent<Rigidbody2D>().velocity.magnitude);

		var h = Input.GetAxis("Horizontal");

		if (Input.GetKey(KeyCode.LeftArrow))
			body.AddTorque(turnRate);
		else if (Input.GetKey(KeyCode.RightArrow))
			body.AddTorque(-turnRate);
		else if (Mathf.Abs(h) > 0.1)
			body.AddTorque(turnRate * -h);

		if (Input.GetButton("PickUp"))
			anim.SetBool("PickUp", true);
		else if (Input.GetButton("PutDown"))
			anim.SetBool("PutDown", true);

        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("M KEY ADD TREASURE");
            AddSingleTreasure();
        }
	}

	public void AddSingleTreasure()
    {
        if (numTreasuresHolding < MaxTreasuresCanHold)
            numTreasuresHolding++;

        CmdSetHoldingTreasure(numTreasuresHolding);
    }

    public void DropSingleTreasure()
    {
        if (numTreasuresHolding > 0)
            numTreasuresHolding--;

        CmdSetHoldingTreasure(numTreasuresHolding);
    }

    public void DropOffAllTreasure()
    {
        if (numTreasuresHolding > 0)
        {
            treasuresCollected += numTreasuresHolding;
            numTreasuresHolding = 0;
        }
    }

    public void TreasuresHoldingChanged(int treasures)
    {
        Debug.Log("Player " + PlayerNum + " is holding " + treasures + " treasures.");
        GetComponent<Rigidbody2D>().gravityScale = treasures * GravityPerTreasureMultiplier;
        UpdateText(treasuresCollected, treasures);
    }

    public void TreasuresCollectedChanged(int treasures)
    {
        Debug.Log("Player " + PlayerNum + " has now collected " + treasures + " treasure.");
        UpdateText(treasures, 0);
    }

    void UpdateText(int treasures, int holding)
    {
        if (myScoreText != null)
        {
            myScoreText.text = treasures.ToString() + " (" + holding.ToString() + ")";
        }
    }

    [Command]
    void CmdSetHoldingTreasure(int num)
    {
        numTreasuresHolding = num;
        UpdateText(treasuresCollected, num);
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

    [Command]
    void CmdSetPlaying(bool newPlaying)
    {
        playing = newPlaying;
        Debug.Log("CmdSetPlaying: " + newPlaying + " for player " + PlayerNum);

        foreach (var o in GameObject.FindObjectsOfType<OctoMove>())
        {
            Debug.Log("CmdSetPlaying: Setting playing to " + newPlaying + " for player " + o.PlayerNum);
            o.playing = newPlaying;
        }
    }

    [ClientRpc]
    void RpcPlaying(bool newPlaying)
    {
        playing = newPlaying;
        Debug.Log("CmdSetPlaying: " + newPlaying + " for player " + PlayerNum);

        foreach (var o in GameObject.FindObjectsOfType<OctoMove>())
        {
            Debug.Log("CmdSetPlaying: Setting playing to " + newPlaying + " for player " + o.PlayerNum);
            o.playing = newPlaying;
        };
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null)
            return;

        Debug.Log("OnTrigger Player");
        if (other.gameObject.CompareTag("Treasure"))
        {
            if (numTreasuresHolding < MaxTreasuresCanHold)
            {
                Debug.Log("Attempting to add treasure");
                AddSingleTreasure();

                var cont = other.gameObject.GetComponent<ChestController>();

                if (cont != null)
                {
                    Debug.Log("Attempting decrement chest");
                    cont.Decrease();
                }
                else
                {
                    Debug.Log("ERROR: Chest controller null");
                }

                //transform.Rotate(Vector3.forward * -90);
                //anim.SetBool("PickUp", true);
            }

            //Debug.Log("Attempting to destroy object");
            //Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            if (DateTimeOffset.UtcNow.Subtract(lastEnemyCollision).TotalMilliseconds > hitCooldownSeconds * 1000)
            {
                if (numTreasuresHolding >= 0)
                {
                    Debug.Log("Dropping treasure!");
                    DropSingleTreasure();
                }

                lastEnemyCollision = DateTimeOffset.UtcNow;
            }
        }
        else if (other.gameObject.CompareTag("Dropoff"))
        {
            if (numTreasuresHolding >= 0)
            {
                DropOffAllTreasure();
            }
        }
    }

    void PlayerNumChanged(int newPlayerNum)
    {
        if (isLocalPlayer)
        {
            Debug.Log("Local player num is now " + newPlayerNum);

            spawnLoc = GameObject.Find("Player" + newPlayerNum + "Spawn");
        }

        SetPlayerColor(newPlayerNum);
    }

    void SetPlayerColor(int plNum)
    {
        var render = GetComponent<SpriteRenderer>();

        if (render != null)
        {
            switch (plNum)
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

        Debug.Log("Set color for player " + plNum);

        myScoreText = null;

        foreach (var tags in GameObject.FindGameObjectsWithTag("PlayerScoreText"))
        {
            Text playerText = tags.GetComponent<Text>();
            if (playerText != null)
            {
                Debug.Log("Checking text " + playerText.name);
                if (playerText.name == "Player" + plNum + "Score")
                {
                    Debug.LogError("FOUND MY NAME TEXT: " + playerText.name);
                    myScoreText = playerText;
                    tags.SetActive(true);
                    UpdateText(treasuresCollected, numTreasuresHolding);
                    break;
                }
                else
                {
                    //Debug.LogError("Name: " + t.name);
                }
            }
            else
            {
                Debug.LogError("playerText is null");
            }
        }

        if (myScoreText == null)
        {
            Debug.LogError("Error: did not find player score text for player " + plNum);
        }
    }

    public bool IsPlaying()
    {
        return playing;
    }
}
