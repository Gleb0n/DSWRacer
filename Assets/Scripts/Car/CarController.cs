using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class CarController : MonoBehaviour
{
    [SerializeField] WheelHandler frontLeftWheel;
    [SerializeField] WheelHandler frontRightWheel;
    [SerializeField] WheelHandler backLeftWheel;
    [SerializeField] WheelHandler backRightWheel;

    PlayerInput playerInput;




    //public float CarSpeed => carSpeed;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        
    }
    private void FixedUpdate()
    {
        
        frontLeftWheel.ApplyTorque();
        frontRightWheel.ApplyTorque();
        backLeftWheel.ApplyTorque();
        backRightWheel.ApplyTorque();

        frontLeftWheel.UpdateVisualRotation(true);
        frontRightWheel.UpdateVisualRotation(true);
        backLeftWheel.UpdateVisualRotation(false);
        backRightWheel.UpdateVisualRotation(false);
        
    }
    



}
