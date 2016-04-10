using UnityEngine;
using System.Collections;

public class EnemyDestroy : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("OnTrigger Destroy");
        //if (other.gameObject.CompareTag("Enemy"))
        {
            //Debug.Log("Attempting to destroy object");
            Destroy(other.gameObject);
        }
    }
}
