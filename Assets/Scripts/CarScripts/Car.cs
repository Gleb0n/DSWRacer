using UnityEngine;
using UnityEngine.InputSystem;

public class Car : MonoBehaviour
{
    [SerializeField] protected WheelHandler frontLeftWheel;
    [SerializeField] protected WheelHandler frontRightWheel;
    [SerializeField] protected WheelHandler backLeftWheel;
    [SerializeField] protected WheelHandler backRightWheel;

    [SerializeField] protected Transform frontLeftWheelTransform;
    [SerializeField] protected Transform frontRightWheelTransform;
    [SerializeField] protected Transform backLeftWheelTransform;
    [SerializeField] protected Transform backRightWheelTransform;

    [SerializeField] public float _motorTorque = 4000f, _brakeForce = 30f, _steerAngle = 30f;
    private WheelHandler[] wheels = new WheelHandler[4];
    private Transform[] wheelsTransform = new Transform[4];

    protected Rigidbody carRigidbody;

    private float xRotation;
    private float yRotation;

    protected void SetUpWheels()
    {
        if (frontLeftWheel == null || frontRightWheel == null || backLeftWheel == null || backRightWheel == null)
        {
            Debug.LogError("One or more of the wheel handlers have not been plugged in on the car");
            Debug.Break();
        }
        else if (frontLeftWheelTransform == null || frontRightWheelTransform == null || backLeftWheelTransform == null || backRightWheelTransform == null)
        {
            Debug.LogError("One or more of the wheel transforms have not been plugged in on the car");
            Debug.Break();
        }
        else
        {
            wheels[0] = frontLeftWheel;
            wheels[1] = frontRightWheel;
            wheels[2] = backLeftWheel;
            wheels[3] = backRightWheel;

            wheelsTransform[0] = frontLeftWheelTransform;
            wheelsTransform[1] = frontRightWheelTransform;
            wheelsTransform[2] = backLeftWheelTransform;
            wheelsTransform[3] = backRightWheelTransform;

        }

    }

    public void ApplyTorque(float torqueDirection, float motorTorque)
    {
        for(int i = 0; i < wheels.Length; i++)
        {
            if (wheels[i] == null)
            {
                Debug.Log("not exist");
            }
            else
            {
                wheels[i].motorTorque = torqueDirection * motorTorque;
            }
        }
    }
    public void UpdateRotation(float steerDirection, float steerAngle)
    {
        Vector3 carVelocity = carRigidbody.velocity;
        float forwardSpeed = Vector3.Dot(transform.forward, carVelocity);
        xRotation += (float) forwardSpeed * 0.1f * Time.deltaTime * Mathf.Rad2Deg;
        if (xRotation >= 360f)
        {
            xRotation -= 360f;
        }

        for (int i = 0; i < wheelsTransform.Length; i++)
        {
            yRotation = wheels[i].yRotationAngle;
            if (i < 2)
            {
                wheels[i].steerAngle = steerDirection * steerAngle;
            }

            wheelsTransform[i].localRotation = Quaternion.Euler(xRotation, yRotation, 0f);             
        }
    }
    public void BrakeTorque(float brakeInput)
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            if (brakeInput > 0)
            {
                wheels[i].BrakeTorque();
                wheels[i].brakeForce = _brakeForce;
            }
            else
            {
                wheels[i].brakeForce = 0;
            }
        }
        
    }



}
