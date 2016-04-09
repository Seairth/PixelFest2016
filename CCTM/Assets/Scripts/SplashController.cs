using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SplashController : MonoBehaviour {

    private NetworkManager manager;
    
    public Button startHostButton;
    public Button startClientButton;
    
    void Awake()
    {
        manager = FindObjectOfType<NetworkManager>();
    }
    
    public void StartHost()
    {
        manager.StartHost();
    }
    
    public void StartClient()
    {
        manager.StartClient();
    }
}
