using Unity.Netcode;
using UnityEngine;

public class NetworkProjectile : NetworkBehaviour
{

    Rigidbody rigidBody;
    [SerializeField] float projectileForce = 50.0f;
    [SerializeField] private float lifetime = 3f;
    float currentLifetime = 0;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody.AddForce(transform.forward * projectileForce, ForceMode.Impulse);
    }

    private void Update()
    {
        currentLifetime += Time.deltaTime;
        if(currentLifetime >= lifetime && IsHost)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject != this.gameObject && collision.gameObject.tag == "Enemy") 
        {
            Debug.Log("Collided with" + collision.gameObject.tag);
            collision.gameObject.GetComponent<EnemyNetwork>().OnTakeDamageEvent_ClientRpc();
            Destroy(this.gameObject);
        }
    }

}
