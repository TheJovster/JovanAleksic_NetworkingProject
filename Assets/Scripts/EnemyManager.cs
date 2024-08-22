using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemyManager : NetworkBehaviour
{
    public static EnemyManager Instance;

    public List<Transform> players = new List<Transform>();
    


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    [ServerRpc]
    public void GetPlayerTransform_ServerRpc(ulong clientNumber) 
    {
        players.Add(NetworkManager.Singleton.ConnectedClients[clientNumber].PlayerObject.transform);
        Debug.Log(NetworkManager.Singleton.ConnectedClients[clientNumber].PlayerObject.transform.position);
    }
}
