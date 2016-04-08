using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D coll)
    {
        //Debug.Log("Bullet collide");
        Destroy(gameObject);
    }
}
