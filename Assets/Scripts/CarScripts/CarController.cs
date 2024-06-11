using UnityEngine;
using UnityEngine.InputSystem;


public class CarController : Car
{
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    private void Start()
    {
        SetUpWheels();
        carRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {

        ApplyTorque(playerInput.verticalDirection, _motorTorque);

        BrakeTorque(playerInput.brakeTorque);

        UpdateRotation(playerInput.horizontalDirection, _steerAngle);

    }

    

}
