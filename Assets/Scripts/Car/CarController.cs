using UnityEngine;
using UnityEngine.InputSystem;


public class CarController : MonoBehaviour
{
    [SerializeField] WheelHandler frontLeftWheel;
    [SerializeField] WheelHandler frontRightWheel;
    [SerializeField] WheelHandler backLeftWheel;
    [SerializeField] WheelHandler backRightWheel;

    [SerializeField] float _motorTorque = 4000f, _brakeForce = 30f, _steerAngle = 30f;


    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    private void FixedUpdate()
    {

        frontLeftWheel.motorTorque = playerInput.verticalDirection * _motorTorque;
        frontRightWheel.motorTorque = playerInput.verticalDirection * _motorTorque;
        backLeftWheel.motorTorque = playerInput.verticalDirection;
        backRightWheel.motorTorque = playerInput.verticalDirection;


        if (playerInput.brakeTorque > 0)
        {
            frontLeftWheel.brakeForce = _brakeForce;
            frontRightWheel.brakeForce = _brakeForce;
            backLeftWheel.brakeForce = _brakeForce;
            backLeftWheel.brakeForce = _brakeForce;
        }
        else
        {
            frontLeftWheel.brakeForce = 0;
            frontRightWheel.brakeForce = 0;
            backLeftWheel.brakeForce = 0;
            backLeftWheel.brakeForce = 0;
        }

        frontLeftWheel.UpdateVisualRotation(true);
        frontRightWheel.UpdateVisualRotation(true);
        backLeftWheel.UpdateVisualRotation(false);
        backRightWheel.UpdateVisualRotation(false);

        frontRightWheel.steerAngle = playerInput.horizontalDirection * _steerAngle;
        frontLeftWheel.steerAngle = playerInput.horizontalDirection * _steerAngle;
        backLeftWheel.steerAngle = playerInput.horizontalDirection * _steerAngle;
        backRightWheel.steerAngle= playerInput.horizontalDirection * _steerAngle;


    }

}
