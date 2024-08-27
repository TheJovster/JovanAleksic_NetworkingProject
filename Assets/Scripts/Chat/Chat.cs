using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Chat : MonoBehaviour
{
    public static Chat Instance;

    private const float minimumIntervalBetweenMessages = 1.0f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);
    }

    [ServerRpc]
    public void SendMessageToServerEnable_ClientRpc(PlayerNetwork sender) 
    {
        sender.EnableExclamationMark_ClientRpc();
    }

    [ServerRpc]
    public void SendMessageToServerDisable_ServerRpc(PlayerNetwork sender) 
    {
        sender.DisableExclamationMark_ClientRpc();
    }
    
    //inside of the ui, not the chat script


}
