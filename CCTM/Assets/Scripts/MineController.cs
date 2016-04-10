using UnityEngine;
using System.Collections;

public class MineController : MonoBehaviour {


	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			var mover = GetComponent<EnemyMover>();

			mover.HorizontalSpeed = 0;
			mover.Speed = 0;

			var anim = GetComponent<Animator>();
			anim.SetTrigger("Explode");

			Destroy(gameObject, 2);
		}
	}
}
