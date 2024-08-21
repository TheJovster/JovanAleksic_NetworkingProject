using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class NetworkManagerUI : NetworkBehaviour
{
    [SerializeField] private Button servetButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    private void Awake()
    {
        servetButton.onClick.AddListener(() => 
        {
            NetworkManager.Singleton.StartServer();
        });

        hostButton.onClick.AddListener(() => 
        {
            NetworkManager.Singleton.StartHost();
        });
        clientButton.onClick.AddListener(() => 
        {
            NetworkManager.Singleton.StartClient();
        });
    }
}
