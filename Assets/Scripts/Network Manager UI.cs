using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

public class NetworkManagerUI : NetworkBehaviour
{
    [SerializeField] private Button servetButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private NetworkObject enemyManager;
    [SerializeField] private TextMeshProUGUI ChatText;

    private void Awake()
    {
        servetButton.onClick.AddListener(() => 
        {
            NetworkManager.Singleton.StartServer();
        });

        hostButton.onClick.AddListener(() => 
        {
            NetworkManager.Singleton.StartHost();
            Instantiate(enemyManager).GetComponent<NetworkObject>().Spawn();

        });
        clientButton.onClick.AddListener(() => 
        {
            NetworkManager.Singleton.StartClient();
            //EnemyManager.Instance.GetPlayerTransform_ServerRpc(OwnerClientId);
        });
    }

    [ClientRpc]
    public void RecieveChatMessage_ClientRpc(string aMessage)
    {
        //display aMessage on screen
        ChatText.text = aMessage;
    }
}
