using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class WheelHandler : MonoBehaviour
{


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
    public float yRotationAngle { get; private set; }
    RaycastHit tireHit;

    private void Awake()
    {

        carRigidbody = GetComponentInParent<Rigidbody>();
        suspensionRestDist = wheelRadious;
    }
    private void FixedUpdate()
    {   
        bool rayDidHit = Physics.Raycast(transform.position, Vector3.down, out tireHit);
        if (rayDidHit)
        {
            ApplySuspension();

            ApplySteering();
            UpdateSteering();

            ApplyAcceleration();


        }
    }
    private void ApplySuspension()
    {
        Vector3 springDir = transform.up;

        Vector3 tireWorldVel = carRigidbody.GetPointVelocity(transform.position);
        float offset = suspensionRestDist - tireHit.distance;

        float vel = Vector3.Dot(springDir, tireWorldVel);

        float force = (offset * springStrength) - (vel * springDamper);

        carRigidbody.AddForceAtPosition(springDir * force, transform.position);

    }
    private void ApplySteering()
    {
        Vector3 steeringDir = transform.right;

        carRigidbody.AddForceAtPosition(steeringDir * tireMass * 
                                        GetVelocityChange(steeringDir, tireGripFactor),
                                        transform.position);
    }
    private void ApplyAcceleration()
    {
        Vector3 accelDir = transform.forward;
        
        float carSpeed = Vector3.Dot(transform.root.forward, carRigidbody.velocity);

        float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / carTopSpeed);

        float availableTorque = powerCurve.Evaluate(normalizedSpeed) * motorTorque;

        if (motorTorque > 0)
        {
            carRigidbody.AddForceAtPosition(accelDir * availableTorque,
                                            transform.position);
        }
        else if (motorTorque < 0)
        {
            carRigidbody.AddForceAtPosition(accelDir * availableTorque * 0.5f,
                                            transform.position);
        }
        else
        {
            //TODO: Serialize magic values
            carRigidbody.AddForceAtPosition(accelDir * GetVelocityChange(accelDir, 0.1f) * 10f, transform.position);
        }
        
    }
    public void BrakeTorque()
    {

        Vector3 brakeDir = transform.forward;

        carRigidbody.AddForceAtPosition(brakeDir * brakeForce * 
                                        GetVelocityChange(brakeDir, availableBrakeTorque),
                                        transform.position);
    }

    private float GetVelocityChange(Vector3 direction, float changeFactor)
    {
        
        Vector3 tireWorldVel = carRigidbody.GetPointVelocity(transform.position);

        float steeringVel = Vector3.Dot(direction, tireWorldVel);

        float desiredVelChange = -steeringVel * changeFactor;

        return desiredVelChange / Time.deltaTime;

    }

    private void UpdateSteering()
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
        transform.localRotation = Quaternion.Euler(0f, yRotationAngle, 0f);
    }


}
