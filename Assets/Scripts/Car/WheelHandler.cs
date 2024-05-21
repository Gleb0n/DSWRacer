using Unity.VisualScripting;
using UnityEngine;


public class WheelHandler : MonoBehaviour
{

    private Transform wheelTransform;
    private Rigidbody carRigidbody;
    private float suspensionRestDist;

    [SerializeField] private float wheelRadious;

    //Suspension parameters
    [SerializeField] private float springStrength = 100f;
    [SerializeField] private float springDamper = 15f;

    //Steering parameters
    [SerializeField] private float tireMass = 20f;
    [SerializeField][Range(0f, 1f)] private float tireGripFactor = 0f;
    [SerializeField] private float turningSpeed = 1f;

    //Acceleration parameters
    [SerializeField] private float carTopSpeed;
    [SerializeField][Range(0f, 1f)] private float availableBrakeTorque;
    [SerializeField] private AnimationCurve powerCurve;



    public float motorTorque { get; set; }
    public float brakeForce { get; set; }
    public float steerAngle { get; set; }

    RaycastHit tireHit;

    private void Awake()
    {
        wheelTransform = GetComponent<Transform>();
        carRigidbody = GetComponentInParent<Rigidbody>();
        suspensionRestDist = wheelRadious;
    }
    public void FixedUpdate()
    {   
        bool rayDidHit = Physics.Raycast(wheelTransform.position, Vector3.down, out tireHit);
        if (rayDidHit)
        {
            ApplySuspension();

            ApplySteering();

            if (brakeForce > 0f)
            {
                BrakeTorque();
            }
            else
            {
                ApplyAcceleration();
            }

        }
    }
    private void ApplySuspension()
    {
        Vector3 springDir = carRigidbody.transform.up;

        Vector3 tireWorldVel = carRigidbody.GetPointVelocity(wheelTransform.position);
        float offset = suspensionRestDist - tireHit.distance;

        float vel = Vector3.Dot(springDir, tireWorldVel);

        float force = (offset * springStrength) - (vel * springDamper);

        carRigidbody.AddForceAtPosition(springDir * force, wheelTransform.position);

    }
    private void ApplySteering()
    {
        Vector3 steeringDir = wheelTransform.right;

        carRigidbody.AddForceAtPosition(steeringDir * tireMass * 
                                        GetVelocityChange(steeringDir, tireGripFactor),
                                        wheelTransform.position);
    }
    private void ApplyAcceleration()
    {
        Vector3 accelDir = carRigidbody.transform.forward;
        

        float carSpeed = Vector3.Dot(transform.root.forward, carRigidbody.velocity);

        float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / carTopSpeed);

        float availableTorque = powerCurve.Evaluate(normalizedSpeed) * motorTorque;

        if (motorTorque > 0)
        {
            carRigidbody.AddForceAtPosition(accelDir * availableTorque,
                                            wheelTransform.position);
        }
        else if (motorTorque < 0)
        {
            carRigidbody.AddForceAtPosition(accelDir * availableTorque * 0.5f,
                                            wheelTransform.position);
        }
        else
        {
            //TODO: Serialize magic values
            carRigidbody.AddForceAtPosition(accelDir * GetVelocityChange(accelDir, 0.1f) * 10f, wheelTransform.position);
        }
        
    }
    private void BrakeTorque()
    {

        Vector3 brakeDir = wheelTransform.forward;

        carRigidbody.AddForceAtPosition(brakeDir * brakeForce * 
                                        GetVelocityChange(brakeDir, availableBrakeTorque),
                                        wheelTransform.position);
    }

    public void UpdateVisualRotation(bool isTurning)
    {

        float yRotationAngle = 0f;
        float xRotationAngle = CalculatePitchAngle();
        if (isTurning)
        {
            yRotationAngle = CalculateYawAngle();
        }

        wheelTransform.localRotation = Quaternion.Euler(new Vector3(xRotationAngle, yRotationAngle, 0f));


    }
    private float yRotationAngle;
    private float CalculateYawAngle()
    {
        float currentSteerAngle = yRotationAngle;
        if (steerAngle > 0f && currentSteerAngle <= steerAngle)
        {
            yRotationAngle += turningSpeed * Time.deltaTime;

        }
        else if (steerAngle < 0f && currentSteerAngle >= steerAngle)
        {
            yRotationAngle -= turningSpeed * Time.deltaTime;
        }
        else
        {
            if (yRotationAngle < -0.3f)
            {
                yRotationAngle += turningSpeed * Time.deltaTime;
            }
            else if (yRotationAngle > 0.3f)
            {
                yRotationAngle -= turningSpeed * Time.deltaTime;
            }
            else if (yRotationAngle <= 0.3f && yRotationAngle >= -0.3f)
            {
                yRotationAngle = 0f;
            }
        }

        return yRotationAngle;
    }
    private float xRotation;
    private float CalculatePitchAngle()
    {

        Vector3 carVelocity = carRigidbody.velocity;
        float forwardSpeed = Vector3.Dot(carVelocity, transform.root.forward);
        xRotation += (float) forwardSpeed * 0.1f * Time.deltaTime * Mathf.Rad2Deg;
        if (xRotation >= 360f)
        {
            xRotation -= 360f;
        }
        return xRotation;

    }

    private float GetVelocityChange(Vector3 direction, float changeFactor)
    {
        
        Vector3 tireWorldVel = carRigidbody.GetPointVelocity(wheelTransform.position);

        float steeringVel = Vector3.Dot(direction, tireWorldVel);

        float desiredVelChange = -steeringVel * changeFactor;

        return desiredVelChange / Time.deltaTime;

    }

}
