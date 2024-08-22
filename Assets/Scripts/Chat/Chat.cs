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
    public void SendChatMessageServerRpc(string aMessagem, ulong senderID)
    {
        // do the thing

        //RecieveChatMessage_ClientRpc(aMessage);
    }


    //inside of the ui, not the chat script


}
