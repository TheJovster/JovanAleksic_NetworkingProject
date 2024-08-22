using Unity.Netcode;
using UnityEngine;

public class EnemyNetwork : NetworkBehaviour
{

    [SerializeField] public NetworkVariable<float> moveSpeed;
    [field: SerializeField] private static int maxHealthValue = 1;
    [SerializeField] private static NetworkVariable<int> maxHealth = new NetworkVariable<int>(maxHealthValue, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Server);
    [SerializeField] private NetworkVariable<int> currentHealth = new NetworkVariable<int>(maxHealth.Value, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Server);


    private PlayerNetwork currentTarget;


    private Rigidbody rigidBody;

    private float distanceToPlayerOne;
    private float distanceToPlayerTwo;

    private void Awake()
    {
        currentHealth.Value = maxHealth.Value;
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        if(currentHealth.Value <= 0) 
        {
            this.GetComponent<NetworkObject>().Despawn();
            Destroy(this.gameObject);
        }
        if (EnemyManager.Instance != null && EnemyManager.Instance.players != null) 
        {
            transform.LookAt(new Vector3(EnemyManager.Instance.players[0].position.x, transform.position.y, EnemyManager.Instance.players[0].position.z));
        }
    }

    


    private void FixedUpdate() 
    {
        //do the current target
        if(EnemyManager.Instance != null && EnemyManager.Instance.players[0] != null) 
        {
            Vector3 targetPos = EnemyManager.Instance.players[0].position - transform.position;
            targetPos.Normalize();
            rigidBody.velocity = targetPos * transform.forward.z * moveSpeed.Value;
        }



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
            Debug.Log("Colldiing with " + collision);
            collision.gameObject.GetComponent<PlayerNetwork>().OnTakeDamageEvent_ClientRpc();
            this.GetComponent<NetworkObject>().Despawn();
            Destroy(this);
        }
    }

    [ServerRpc]
    public void OnTakeDamageEvent_ServerRpc() 
    {

        currentHealth.Value--;
        Debug.Log("HealthDecremented.");
    }

    [ServerRpc]
    public void Disable_ServerRpc() 
    {
        base.OnNetworkDespawn();
    }


}
