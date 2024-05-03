using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [SerializeField] WheelHandler frontLeftWheel;
    [SerializeField] WheelHandler frontRightWheel;
    [SerializeField] WheelHandler backLeftWheel;
    [SerializeField] WheelHandler backRightWheel;

    //Player controlls
    private InputAction move;
    private InputAction stopMove;
    private PlayerInputAction playerControls;
    private Vector2 moveDirection = Vector2.zero;
    private float brakeTorque;



    //public float CarSpeed => carSpeed;
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
    private void FixedUpdate()
    {
        PlayerInput();
        frontLeftWheel.ApplyTorque(moveDirection, brakeTorque);
        frontRightWheel.ApplyTorque(moveDirection, brakeTorque);
        backLeftWheel.ApplyTorque(moveDirection, brakeTorque);
        backRightWheel.ApplyTorque(moveDirection, brakeTorque);

        frontLeftWheel.UpdateVisualRotation(true);
        frontRightWheel.UpdateVisualRotation(true);
        backLeftWheel.UpdateVisualRotation(false);
        backRightWheel.UpdateVisualRotation(false); ;
    }
    private void PlayerInput()
    {
        moveDirection = move.ReadValue<Vector2>();

        brakeTorque = stopMove.ReadValue<float>();
    }

   

}
