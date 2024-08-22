using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemyManager : NetworkBehaviour
{
    public static EnemyManager Instance;

    public List<Transform> players = new List<Transform>();
    [SerializeField] private NetworkObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        EnemyManager.Instance.SpawnEnemies_ServerRpc();
    }

    [ServerRpc]
    public void GetPlayerTransform_ServerRpc(ulong clientNumber) 
    {
        players.Add(NetworkManager.Singleton.ConnectedClients[clientNumber].PlayerObject.transform);
        Debug.Log(NetworkManager.Singleton.ConnectedClients[clientNumber].PlayerObject.transform.position);
    }

    [ServerRpc]
    public void SpawnEnemies_ServerRpc() 
    {
        Instantiate(enemyPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)]).GetComponent<NetworkObject>().Spawn();
    }
}
