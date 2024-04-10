using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{

    [SerializeField] Transform[] wheels;
    [SerializeField] float wheelRadious;
    [SerializeField] float tireMass = 20f;

    [SerializeField] float springStrength = 100f;
    [SerializeField] float springDamper = 15f;

    [SerializeField][Range(0f, 1f)] float tireGripFactor = 0f;
    [SerializeField] float maxSteeringAngle = 30f;

    [SerializeField] float carForwardTopSpeed;
    [SerializeField] [Range(0f, 1f)] float availableBrakeTorque;
    [SerializeField] AnimationCurve powerCurve;

    [SerializeField] InputAction movement;
    [SerializeField] InputAction stopMove;

    float rightThrow;
    float forwardThrow;
    float brakeTorque;
    Rigidbody carRigidbody;
    float suspensionRestDist;

    private void OnEnable()
    {
        stopMove.Enable();
        movement.Enable();
    }
    private void OnDisable()
    {
        stopMove.Disable();
        movement.Disable();
    }

    private void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
        suspensionRestDist = 0.371f;

    }

    static float t = 0.0f;
    private void FixedUpdate()
    {
        Input();
        for (int i = 0; i < wheels.Length; i++)
        {
            RaycastHit tireHit;
            bool rayDidHit = Physics.Raycast(wheels[i].position, wheels[i].TransformDirection(Vector3.down), out tireHit);
            // Does the ray intersect any objects excluding the player layer
            if (rayDidHit)
            {
                ApplySuspension(tireHit, wheels[i]);
                ApplySteering(wheels[i]);
                ApplyAcceleration(wheels[i]);

                if (i < 2)
                {
                    float currentSteerAngle = maxSteeringAngle * rightThrow;
                    
                    wheels[i].localRotation = Quaternion.Euler(0f, Mathf.Lerp(wheels[i].localRotation.y, currentSteerAngle, t), 0f);
                    t += 0.5f * Time.fixedDeltaTime;

                    if (rightThrow == 0.0)
                    {
                        t = 0.0f;
                    }
                }
            }
            else
            {
                Debug.DrawRay(wheels[i].position, wheels[i].TransformDirection(Vector3.down) * 1000, Color.white);
                Debug.Log("Did not Hit");
                return;
            }
        }
    }
    void Input()
    {
        forwardThrow = movement.ReadValue<Vector2>().y;
        rightThrow = movement.ReadValue<Vector2>().x;
        brakeTorque = stopMove.ReadValue<float>();
    }

    private void ApplySteering(Transform wheel)
    {
        Vector3 steeringDir = wheel.right;

        Vector3 tireWorldVel = carRigidbody.GetPointVelocity(wheel.position);

        float steeringVel = Vector3.Dot(steeringDir, tireWorldVel);

        float desiredVelChange = -steeringVel * tireGripFactor;

        float desiredAccel = desiredVelChange / Time.fixedDeltaTime;

        carRigidbody.AddForceAtPosition(steeringDir * tireMass * desiredAccel, wheel.position);
    }

    void ApplySuspension(RaycastHit tireHit, Transform wheel)
    {


        Vector3 springDir = wheel.up;

        Vector3 tireWorldVel = carRigidbody.GetPointVelocity(wheel.position);

        float offset = suspensionRestDist - tireHit.distance;

        float vel = Vector3.Dot(springDir, tireWorldVel);


        float force = (offset * springStrength) - (vel * springDamper);

        carRigidbody.AddForceAtPosition(springDir * force, wheel.position);
        Debug.DrawRay(wheel.position, wheel.TransformDirection(Vector3.down) * tireHit.distance, Color.yellow);
    }

    void ApplyAcceleration(Transform wheel)
    {
        Vector3 accelDire = wheel.forward;
    

        if(forwardThrow > 0)
        {
            float carSpeed = Vector3.Dot(transform.forward, carRigidbody.velocity);

            float normalizedSpeed = Mathf.Clamp01(Math.Abs(carSpeed) / carForwardTopSpeed);

            float availableTorque = powerCurve.Evaluate(normalizedSpeed) * forwardThrow;

            carRigidbody.AddForceAtPosition(accelDire * availableTorque * 1500, wheel.position);
            Debug.Log(carRigidbody.velocity.magnitude);
        }
        else if(brakeTorque == 1 || forwardThrow == 0) 
        {
            BrakeTorque(wheel);
            
        }
        else if (forwardThrow < 0)
        {
            float carSpeed = Vector3.Dot(transform.forward, carRigidbody.velocity);

            float normalizedSpeed = Mathf.Clamp01(Math.Abs(carSpeed) / carForwardTopSpeed);

            float availableTorque = powerCurve.Evaluate(normalizedSpeed) * forwardThrow;

            carRigidbody.AddForceAtPosition(accelDire * availableTorque * 1500, wheel.position);
        }
    }

   void BrakeTorque(Transform wheel)
    {
        Vector3 accelDire = wheel.forward;

        Vector3 tireWorldVel = carRigidbody.GetPointVelocity(wheel.position);

        float brakeTorque = Vector3.Dot(accelDire, tireWorldVel);
        float desiredVelChange = -brakeTorque * availableBrakeTorque;

        float desiredAccel = desiredVelChange / Time.fixedDeltaTime;
        Debug.Log(desiredAccel);

        carRigidbody.AddForceAtPosition(accelDire * tireMass * desiredAccel, wheel.position);
    }
}
