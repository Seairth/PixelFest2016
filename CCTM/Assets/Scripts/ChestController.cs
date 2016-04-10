using UnityEngine;
using System.Collections;

public class ChestController : MonoBehaviour {

	public int minBars = 1;
	public int maxBars = 5;

	private int bars;

	// Use this for initialization
	void Start () {

		bars = Random.Range(minBars, maxBars);
		SetSprite();
	}

	public bool Decrease()
	{
		Debug.Log("Bars: " + bars);

		if (bars == 0)
			return false;

		bars--;
		SetSprite();

		if (bars == 0)
			Destroy(gameObject, 1);

		return true;
	}

	private void SetSprite()
	{
		GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Treasure Chest/treasure chest " + bars);
	}
}
