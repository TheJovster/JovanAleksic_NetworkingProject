using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;

public class EnemyNetwork : NetworkBehaviour
{

    [SerializeField] public NetworkVariable<float> moveSpeed;
    [field: SerializeField] private static int maxHealthValue = 5;
    [SerializeField] private static NetworkVariable<int> maxHealth = new NetworkVariable<int>(maxHealthValue, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Server);
    [SerializeField] private NetworkVariable<int> currentHealth = new NetworkVariable<int>(maxHealth.Value, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Server);
    private float distanceToPlayerOne;
    private float distanceToPlayerTwo;
    private PlayerNetwork currentTarget;

    private Rigidbody rigidBody;

    private void Awake()
    {
        currentHealth.Value = maxHealth.Value;
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(currentHealth.Value <= 0) 
        {
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate() 
    {

    }

    [ServerRpc]
    private void GoToNearestPlayer_ServerRpc() 
    {
       //finish tomorrow
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player") 
        {
            collision.gameObject.GetComponent<PlayerNetwork>().OnTakeDamageEvent_ClientRpc();
            this.GetComponent<NetworkObject>().Despawn();
            Destroy(this);
        }
    }

    [ClientRpc]
    public void OnTakeDamageEvent_ClientRpc() 
    {

        Debug.Log("HealthDecremented.");
        currentHealth.Value--;   
    }

    public void Destroy(NetworkObject networkObject) 
    {

    }
}
