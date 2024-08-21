using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;

public class EnemyNetwork : NetworkBehaviour
{
    [SerializeField] public NetworkVariable<float> currentHealth;
    [SerializeField] public NetworkVariable<float> moveSpeed;
    [SerializeField] public float maxHealth = 10.0f;
    private float distanceToPlayerOne;
    private float distanceToPlayerTwo;
    private PlayerNetwork currentTarget;

    private Rigidbody rigidBody;

    private void Awake()
    {
        currentHealth.Value = maxHealth;
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        
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

    public void Destroy(NetworkObject networkObject) 
    {

    }
}
