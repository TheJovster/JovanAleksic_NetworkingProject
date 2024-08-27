using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemyManager : NetworkBehaviour
{
    public static EnemyManager Instance;
    [HideInInspector] public NetworkVariable<int> waveNumber;

    public List<Transform> players = new List<Transform>();
    [SerializeField] private NetworkObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] public List<EnemyNetwork> enemiesList = new List<EnemyNetwork>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        EnemieSpawnQueue_ServerRpc();
        if (IsServer)
        {
            waveNumber.Value++;
        }
    }

    [ServerRpc(RequireOwnership =false)]
    private void EnemieSpawnQueue_ServerRpc()
    {
        for (int i = 0; i < 5; i++)
            SpawnEnemies_ServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void GetPlayerTransform_ServerRpc(ulong clientNumber)
    {
        players.Add(NetworkManager.Singleton.ConnectedClients[clientNumber].PlayerObject.transform);
        Debug.Log(NetworkManager.Singleton.ConnectedClients[clientNumber].PlayerObject.transform.position);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnEnemies_ServerRpc()
    {
        Instantiate(enemyPrefab, spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)]).GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    public void StartNewWave_ServerRpc()
    {
        waveNumber.Value++;
        for(int i = 0; i < 5 + waveNumber.Value; i++) 
        {
            SpawnEnemies_ServerRpc();
        }
    }
}
