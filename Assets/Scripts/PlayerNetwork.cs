using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;



public class PlayerNetwork : NetworkBehaviour
{
    private PlayerInput input;
    private Rigidbody rigidBody;

    /// <summary>
    /// network objects or game objects? Does it matter?
    /// </summary>
    public NetworkObject projectile;
    public Transform projectileSpawnPoint;
    public NetworkTransform projectileSpanPointNT;
    private Vector2 moveInput;
    private Vector2 mousePos;
    public bool emoteEnabled;
    public GameObject exclamationMark;
    private float emoteLifeTime = 3f;
    private float sinceEmoteActive;

    /// <summary>
    /// network variables go here. They are replicated and constant between the server/host and clients
    /// </summary>

    [SerializeField] private static int maxHealthValue = 10;
    private static NetworkVariable<int> maxHealth = new NetworkVariable<int>(maxHealthValue, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField]private NetworkVariable<int> currentHealth = new NetworkVariable<int>(maxHealth.Value, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private void Awake()
    {
        emoteEnabled = false;
        exclamationMark.SetActive(false);
        rigidBody = GetComponent<Rigidbody>();
        if (input == null)
        {
            input = new PlayerInput();
        }

        sinceEmoteActive = float.PositiveInfinity;

    }
    private void Start()
    {
        if (IsLocalPlayer)
        {
            input.Action.Shoot.performed += OnFireEvent;
            input.Action.Emote.performed += EmoteToggleEvent;
        }
        EnemyManager.Instance.GetPlayerTransform_ServerRpc(OwnerClientId);
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Action.Shoot.performed -= OnFireEvent;
        input.Action.Emote.performed -= EmoteToggleEvent;
        input.Disable();
    }

    

    private void Update()
    {
        MoveInput();
        MouseInput();

        sinceEmoteActive += Time.deltaTime;

        if (currentHealth.Value <= 0)
        {
            this.GetComponent<NetworkObject>().Despawn();
            Destroy(this);
        }
        if (sinceEmoteActive >= emoteLifeTime)
        {
            Chat.Instance.SendMessageToServerDisable_ServerRpc(this);
        }
        
    }

    private void EmoteToggleEvent(InputAction.CallbackContext context)
    {
        if (sinceEmoteActive >= emoteLifeTime)
        {
            emoteEnabled = true;
            sinceEmoteActive = 0;
            if (emoteEnabled)
            {
                Chat.Instance.SendMessageToServerEnable_ClientRpc(this);
            }
        }
    }

    private void FixedUpdate()
    {
        OnMove();
        LookAt();
    }


    private void OnMove()
    {
        float moveSpeed = 3.0f;
        Vector3 movement = (moveInput.x * transform.right) + (moveInput.y * transform.forward);
        movement.Normalize();
        rigidBody.velocity = movement * moveSpeed;
    }


    private void LookAt()
    {
        if (IsLocalPlayer)
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                Vector3 lookAtPoint = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                transform.LookAt(lookAtPoint);
            }
        }
    }


    [ClientRpc]
    public void OnTakeDamageEvent_ClientRpc() 
    {
        currentHealth.Value--;

        // TODO: handle death and destruction
    }

    public void OnFireEvent(InputAction.CallbackContext context)
    {
        FireEvent_ServerRpc();
    }

    [ServerRpc]
    public void FireEvent_ServerRpc()
    {
        NetworkObject newProjectile = Instantiate(projectile, projectileSpawnPoint);
        newProjectile.Spawn();
        Debug.Log("Pewpew" + " " + OwnerClientId);
    }

    public void MoveInput()
    {
        moveInput = input.PlayerMovement.Movement.ReadValue<Vector2>();
    }


    public void MouseInput() 
    {
        mousePos = Input.mousePosition;
    }

    [ServerRpc]
    public void Disable_ServerRpc() 
    {
        base.OnNetworkDespawn();
    }

    [ClientRpc]
    public void EnableExclamationMark_ClientRpc() 
    {
        exclamationMark.SetActive(true);
        Debug.Log("Exclamation mark enabled");
    }
    [ClientRpc]
    public void DisableExclamationMark_ClientRpc() 
    {
        exclamationMark.SetActive(false);
        emoteEnabled = false;
    }
}
