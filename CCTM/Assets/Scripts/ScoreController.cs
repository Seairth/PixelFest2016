using UnityEngine;
using System.Collections;

public class ScoreController : MonoBehaviour {
	

	void OnClientConnected(NetworkPlayer player)
	{
		Debug.Log("Client Connected");
	}
}
