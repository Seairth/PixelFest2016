using UnityEngine;
using System.Collections;

public class OctoMove : MonoBehaviour {

    private Rigidbody2D body;

    public float force = 0;
    public float turnRate = 0;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetKey(KeyCode.UpArrow))
            body.AddForce(transform.up * force);

        if (Input.GetKey(KeyCode.LeftArrow))
            transform.Rotate(new Vector3(0, 0, turnRate));

        if (Input.GetKey(KeyCode.RightArrow))
            transform.Rotate(new Vector3(0, 0, -turnRate));
    }
}
