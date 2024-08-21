using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, PlayerInput.IPlayerMovementActions, PlayerInput.IActionActions
{
    private PlayerInput input;

    public event UnityAction<Vector2> MoveEvent = delegate { };
    public event UnityAction ShootEvent = delegate { };
    public event UnityAction EmoteEvent = delegate { };
    public event UnityAction SendEvent = delegate { };

    public void OnEmote(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed) 
        {
            EmoteEvent.Invoke();
        }
    }


    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed) 
        {
            ShootEvent.Invoke();
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        MoveEvent.Invoke(context.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        if(input == null) 
        {
            input = new PlayerInput();
            input.PlayerMovement.SetCallbacks(this);
            input.Action.SetCallbacks(this);
            input.PlayerMovement.Enable();
            input.Action.Enable();
        }
    }
}
