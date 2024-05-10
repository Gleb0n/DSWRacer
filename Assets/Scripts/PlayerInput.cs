using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerInput : MonoBehaviour
{
    private InputAction move;
    private InputAction stopMove;
    private PlayerInputAction playerControls;
    private Vector2 moveDirection;
    private float brakeTorque;

    public Vector2 MoveDirection => moveDirection;
    public float BrakeTorque => brakeTorque;
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
    private void Inputs()
    {
        moveDirection = move.ReadValue<Vector2>();

        brakeTorque = stopMove.ReadValue<float>();
    }
    private void Update()
    {
        Inputs();
    }
}
