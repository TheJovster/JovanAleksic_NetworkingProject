using Unity.Netcode;
using UnityEngine;


public class EnemyNetwork : NetworkBehaviour
{

    [SerializeField] public NetworkVariable<float> moveSpeed;
    private static int maxHealthValue;
    [SerializeField] private static NetworkVariable<int> maxHealth = new NetworkVariable<int>(maxHealthValue, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Server);
    [SerializeField] private NetworkVariable<int> currentHealth = new NetworkVariable<int>(maxHealth.Value, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Server);


    private PlayerNetwork currentTarget;

    private Rigidbody rigidBody;

    private float distanceToPlayerOne;
    private float distanceToPlayerTwo;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        currentHealth.Value = 1;
    }

    private void Start()
    {

    }

    public override void OnNetworkSpawn()
    {
        PopulateList_ServerRpc();
        base.OnNetworkSpawn();
    }

    [ServerRpc(RequireOwnership =false)]
    private void PopulateList_ServerRpc()
    {
        EnemyManager.Instance.enemiesList.Add(this);
    }

    private void Update()
    {
        if(currentHealth.Value <= 0) 
        {
            EnemyManager.Instance.enemiesList.Remove(this);
            this.GetComponent<NetworkObject>().Despawn();
            Destroy(this.gameObject);
            if(EnemyManager.Instance.enemiesList.Count <= 0) 
            {
                EnemyManager.Instance.StartNewWave_ServerRpc();
            }
        }
        if (EnemyManager.Instance != null && EnemyManager.Instance.players != null) 
        {
            //transform.LookAt(new Vector3(EnemyManager.Instance.players[0].position.x, transform.position.y, EnemyManager.Instance.players[0].position.z));
        }
        if (EnemyManager.Instance != null)
        {
            //Vector3 targetPos = EnemyManager.Instance.players[0].position - transform.position;
            //targetPos.Normalize();
            //rigidBody.velocity = new Vector3(targetPos.x, transform.position.y, targetPos.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player") 
        {
            collision.gameObject.GetComponent<PlayerNetwork>().OnTakeDamageEvent_ClientRpc();
            EnemyManager.Instance.enemiesList.Remove(this);
            this.GetComponent<NetworkObject>().Despawn();
            Destroy(this.gameObject);
        }
    }


    [ServerRpc]
    private void GoToNearestPlayer_ServerRpc() 
    {
       //finish tomorrow
    }

    [ServerRpc]
    public void OnTakeDamageEvent_ServerRpc() 
    {

        currentHealth.Value--;
    }

    [ServerRpc]
    public void Disable_ServerRpc() 
    {
        base.OnNetworkDespawn();
    }


}
