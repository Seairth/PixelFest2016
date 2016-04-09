using System;
using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class OctoSound : MonoBehaviour {
    public AudioMixerSnapshot ambient;
    public AudioMixerSnapshot combat;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Combat"))
        {
            combat.TransitionTo(0);
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Combat"))
        {
            ambient.TransitionTo(0);
        }        
    }
}
