using UnityEngine;


public class WheelHandler : MonoBehaviour
{
    [SerializeField] CarStats carStats;

    private PlayerInput playerInput;
    private Vector2 _moveDirection;
    private float _brakeTorque;

    private Transform wheelTransform;
    private Rigidbody carRigidbody;
    private float suspensionRestDist;
    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        wheelTransform = GetComponent<Transform>();
        carRigidbody = GetComponentInParent<Rigidbody>();
        suspensionRestDist = carStats.wheelRadious;
    }
    public void ApplyTorque()
    {
        _moveDirection = playerInput.MoveDirection;
        _brakeTorque = playerInput.BrakeTorque;
        RaycastHit tireHit;
        bool rayDidHit = Physics.Raycast(wheelTransform.position, Vector3.down, out tireHit);
        if (rayDidHit)
        {
            ApplySuspension(tireHit);
            ApplySteering();
            if (_brakeTorque > 0f || _moveDirection.y == 0f)
            {
                BrakeTorque();
            }
            else
            {
                ApplyAcceleration();
            }

        }
    }
    private void ApplySuspension(RaycastHit tireHit)
    {
        Vector3 springDir = carRigidbody.transform.up;

        Vector3 tireWorldVel = carRigidbody.GetPointVelocity(wheelTransform.position);
        float offset = suspensionRestDist - tireHit.distance;

        float vel = Vector3.Dot(springDir, tireWorldVel);

        float force = (offset * carStats.springStrength) - (vel * carStats.springDamper);

        carRigidbody.AddForceAtPosition(springDir * force, wheelTransform.position);

    }
    private void ApplySteering()
    {
        Vector3 steeringDir = wheelTransform.right;
        Vector3 tireWorldVel = carRigidbody.GetPointVelocity(wheelTransform.position);

        float steeringVel = Vector3.Dot(steeringDir, tireWorldVel);

        float desiredVelChange = -steeringVel * carStats.tireGripFactor;

        float desiredAccel = desiredVelChange / Time.fixedDeltaTime;

        carRigidbody.AddForceAtPosition(steeringDir * carStats.tireMass * desiredAccel, wheelTransform.position);
    }
    private void ApplyAcceleration()
    {
        Vector3 accelDir = carRigidbody.transform.forward;

        float carSpeed = Vector3.Dot(transform.root.forward, carRigidbody.velocity);

        float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / carStats.carTopSpeed);

        float availableTorque = carStats.powerCurve.Evaluate(normalizedSpeed) * _moveDirection.y;

        carRigidbody.AddForceAtPosition(accelDir * availableTorque * carStats.motorTorque, wheelTransform.position);
    }
    private void BrakeTorque()
    {

        Vector3 accelDir = wheelTransform.forward;

        Vector3 tireWorldVel = carRigidbody.GetPointVelocity(wheelTransform.position);

        float brakeTorque = Vector3.Dot(accelDir, tireWorldVel);

        float desiredVelChange = -brakeTorque * carStats.availableBrakeTorque;

        float desiredAccel = desiredVelChange / Time.fixedDeltaTime;

        carRigidbody.AddForceAtPosition(accelDir * carStats.tireMass * desiredAccel, wheelTransform.position);
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
    private float t = 0.0f;
    private float CalculateYawAngle()
    {
        float yRotationAngle = Mathf.LerpUnclamped(0f, carStats.maxSteeringAngle, t);


        if (_moveDirection.x > 0f && t <= 1f)
        {
            t += carStats.turningSpeed * Time.fixedDeltaTime;

        }
        else if (_moveDirection.x < 0f && t >= -1f)
        {
            t -= carStats.turningSpeed * Time.fixedDeltaTime;
        }
        else
        {
            if (t < -0.3f)
            {
                t += carStats.turningSpeed * Time.fixedDeltaTime;
            }
            else if (t > 0.3f)
            {
                t -= carStats.turningSpeed * Time.fixedDeltaTime;
            }
            else if (t <= 0.3f && t >= -0.3f)
            {
                t = 0f;
            }
        }

        return yRotationAngle;
    }
    private float xRotation;
    private float CalculatePitchAngle()
    {

        Vector3 carVelocity = carRigidbody.velocity;
        float forwardSpeed = Vector3.Dot(carVelocity, transform.root.forward);
        xRotation += (float) forwardSpeed * 0.1f * Time.fixedDeltaTime * Mathf.Rad2Deg;
        if (xRotation >= 360f)
        {
            xRotation -= 360f;
        }
        return xRotation;

    }
}
