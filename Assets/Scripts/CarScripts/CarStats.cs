using UnityEngine;

[CreateAssetMenu(fileName = "Car Stats")]
public class CarStats : ScriptableObject
{
    

    public float wheelRadious;

    //Suspension parameters
    public float springStrength = 100f;
    public float springDamper = 15f;

    //Steering parameters
    public float tireMass = 20f;
    [Range(0f, 1f)] public float tireGripFactor = 0f;
    public float maxSteeringAngle = 30f;
    public float turningSpeed = 1f;

    //Acceleration parameters
    public float carTopSpeed;
    [Range(0f, 1f)] public float availableBrakeTorque;
    public float motorTorque = 2000f;
    public AnimationCurve powerCurve;
}
