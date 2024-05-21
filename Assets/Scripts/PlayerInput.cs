using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerInput : MonoBehaviour
{
    private InputAction move;
    private InputAction stopMove;
    private PlayerInputAction playerControls;

    private Vector2 moveDirection;
    private float stop;

    public float verticalDirection => moveDirection.y;
    public float horizontalDirection => moveDirection.x;
    public float brakeTorque => stop;
    private void Awake()
    {
        playerControls = new PlayerInputAction();
    }
    private void OnEnable()
    {
        move = playerControls.Player.Move;
        stopMove = playerControls.Player.BrakeTorque;
        move.Enable();
        stopMove.Enable();
    }
    private void OnDisable()
    {
        move.Disable();
        stopMove.Disable();
    }
    private void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
        stop = stopMove.ReadValue<float>();
    }

}
