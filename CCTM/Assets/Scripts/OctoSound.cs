using System;
using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class OctoSound : MonoBehaviour {
	public AudioClip collisionSound;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
			GetComponent<AudioSource>().PlayOneShot(collisionSound);
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
    }
}
