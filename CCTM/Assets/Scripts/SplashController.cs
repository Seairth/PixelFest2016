using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SplashController : NetworkBehaviour {

    private NetworkManager manager;
    
    public Button startHostButton;
    public Button startClientButton;
    
    public Button startGameButton;
    
    public Text waitingText;
    
    void Awake()
    {
        manager = FindObjectOfType<NetworkManager>();
    }
    
    void Start()
    {
        if (manager.IsClientConnected())
        {
            if (Network.isServer)
                _ShowStartButton();
            else
                _ShowWaiting();
        }
    }
    
    public void StartHost()
    {
        manager.StartHost();
        _ShowStartButton();
    }
    
    public void StartClient()
    {
        manager.StartClient();
        _ShowWaiting();
    }
    
    public void StartGame()
    {
        gameRunning = true;
    }
    
    private void _ShowStartButton()
    {
        startHostButton.gameObject.SetActive(false);
        startClientButton.gameObject.SetActive(false);
        startGameButton.gameObject.SetActive(true);
    }
    
    private void _ShowWaiting()
    {
        startHostButton.gameObject.SetActive(false);
        startClientButton.gameObject.SetActive(false);
        waitingText.gameObject.SetActive(true);
    }
    
    [SyncVar(hook="SetGameRunning")]
    public bool gameRunning;
      
    public void SetGameRunning(bool value)
    {
        if(value)
            SceneManager.LoadScene("GamePlay");
    }

	public override float GetNetworkSendInterval()
	{
		return 0;
	}
}
