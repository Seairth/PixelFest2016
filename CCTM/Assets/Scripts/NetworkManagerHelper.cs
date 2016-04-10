using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManagerHelper : NetworkManager {

    public int MaxPlayers = 4;
    private GameObject[] Players;
    const int PlayerNumOffset = 1;

    // Use this for initialization
    void Start()
    {
        Players = new GameObject[MaxPlayers];

		var input = FindObjectOfType<InputField>();
		input.text = networkAddress;
		input.onValueChange.AddListener(SetHostAddress);
    }

    public void StartHost()
    {
		if (string.IsNullOrEmpty(networkAddress))
			return;

        base.StartHost();
    }

    public void StartClient()
    {
		if (string.IsNullOrEmpty(networkAddress))
			return;

		base.StartClient();
    }

	public void SetHostAddress(string address)
	{
		networkAddress = address;
	}

    public int SetPlayerNum(GameObject newPlayer)
    {
        int retval = -1;

        for (int i = 0; i < MaxPlayers; i++)
        {
            if (Players[i] == null)
            {
                Players[i] = newPlayer;
                retval = i + PlayerNumOffset;
                break;
            }
        }

        return retval;
    }

    public bool IsPlayerConnected(int playerNum)
    {
        return Players[playerNum - PlayerNumOffset] != null;
    }

    public void RemovePlayerNum(int num)
    {
        Players[num - PlayerNumOffset] = null;
    }
}
