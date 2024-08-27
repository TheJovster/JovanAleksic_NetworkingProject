using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

public class NetworkManagerUI : NetworkBehaviour
{
    public static NetworkManagerUI Instance;

    [SerializeField] private Button servetButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private NetworkObject enemyManager;
    [SerializeField] private NetworkObject Chat;
    [SerializeField] private TextMeshProUGUI ChatText;
    [SerializeField] private TMP_InputField InputText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != null)
            Destroy(this);

        servetButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
            Instantiate(enemyManager).GetComponent<NetworkObject>().Spawn();
            Instantiate(Chat).GetComponent<NetworkObject>().Spawn();
        });

        hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            Instantiate(enemyManager).GetComponent<NetworkObject>().Spawn();
            Instantiate(Chat).GetComponent<NetworkObject>().Spawn();
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
    public void ConfirmChatMessage_ClientRpc(string aMessage)
    {
        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            InputText.text = aMessage;
            if (string.IsNullOrEmpty(aMessage))
                return;

            SendMessage_ServerRpc(aMessage);
            InputText.text = null;
            Debug.Log("Input text " + aMessage);
        }
    }
}
