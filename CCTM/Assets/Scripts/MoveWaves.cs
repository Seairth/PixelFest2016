using UnityEngine;
using System.Collections;

public class MoveWaves : MonoBehaviour {
    public int maxMove = 5;
    public float movementPerFrame = 0.05f;
    public bool movingLeft = true;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	    if (movingLeft)
        {
            transform.position = new Vector3(transform.position.x - movementPerFrame, transform.position.y, transform.position.z);

            if (transform.position.x <= -maxMove)
            {
                movingLeft = false;
            }
        }
        else
        {
            transform.position = new Vector3(transform.position.x + movementPerFrame, transform.position.y, transform.position.z);

            if (transform.position.x >= maxMove)
            {
                movingLeft = true;
            }
        }
	}
}
