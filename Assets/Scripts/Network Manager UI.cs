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
    [SerializeField] private TMP_InputField InputText;

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

        });
    }

    [ServerRpc(RequireOwnership=false)]
    public void InputText_ServerRpc()
    {
        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            Debug.Log("Messsage Input");
            SendMessage_ServerRpc(InputText.text);
        }
    }

    [ServerRpc]
    public void RecieveChatMessage_ServerRpc(string aMessage)
    {
        //display aMessage on screen
        ChatText.text = aMessage;
    }

    [ServerRpc]
    public void SendMessage_ServerRpc(string messageToSend) 
    {
        messageToSend = InputText.text;
        RecieveChatMessage_ServerRpc(messageToSend);
    }

    [ClientRpc]
    public void ConfirmChatMessage_ClientRpc()
    {
        string message = InputText.text;
        if (string.IsNullOrEmpty(message))
            return;

        SendMessage_ServerRpc(message);
    }
}
