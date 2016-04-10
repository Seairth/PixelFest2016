using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KelpColorVariation : MonoBehaviour {

    public float RedVariation = 0.2f;
    public float BlueVariation = 0.2f;
    public float GreenVariation = 0.2f;

	// Use this for initialization
	void Start () {
        GameObject[] kelps = GameObject.FindGameObjectsWithTag("Kelp");
        int count = 0;

        foreach (var kelp in kelps)
        {
            var spr = kelp.GetComponent<SpriteRenderer>();

            float r = spr.color.r - Random.Range(0f, 1f) * RedVariation;
            float g = spr.color.g - Random.Range(0f, 1f) * BlueVariation;
            float b = spr.color.b - Random.Range(0f, 1f) * GreenVariation;

            spr.color = new Color(r, g, b, spr.color.a);

            count++;
        }

        Debug.Log("Applied color variation to " + count + " kelps.");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
