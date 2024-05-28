using System.Collections.Generic;
using UnityEngine;

public class CarAI : Car
{
    [SerializeField] private float trackerSpeed;
    [SerializeField] private TrackCheckpoints trackCheckpoints;

    private List<CheckpointSingle> checkpointSingles;
    private int currentCheckpointCount;

    private GameObject tracker;
    private float currentTrackerSpeed;

    private CarController player;

    private void Awake()
    {
        tracker = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Destroy(tracker.GetComponent<Collider>());
        Destroy(tracker.GetComponent<MeshRenderer>());
        tracker.transform.position = this.transform.position;
        tracker.transform.rotation = this.transform.rotation;   

    }
    private void Start()
    {
        SetUpWheels();
        carRigidbody = GetComponent<Rigidbody>();
        player = FindObjectOfType<CarController>();
        checkpointSingles = trackCheckpoints.GetCheckpoints();
        currentTrackerSpeed = trackerSpeed;
    }

    private void FixedUpdate()
    {  
        ProgressTracker();
        FollowTracker();
    }
    private void ProgressTracker()
    {
        Vector3 currentCheckpointPosition = checkpointSingles[currentCheckpointCount].gameObject.transform.position;
        if(Vector3.Distance(tracker.transform.position, currentCheckpointPosition) < 5 )
        {
            currentCheckpointCount++;
        }
        if(currentCheckpointCount >= checkpointSingles.Count)
        {
            currentCheckpointCount = 0;
        }

        tracker.transform.LookAt(currentCheckpointPosition);
        tracker.transform.Translate(0, 0, (carRigidbody.velocity.magnitude * currentTrackerSpeed) * Time.deltaTime);
    }
    private void FollowTracker()
    {

        Vector3 normalizedDirection = (tracker.transform.position - this.transform.position).normalized;
        float lookAtTracker = Vector3.SignedAngle(transform.forward, normalizedDirection, Vector3.up);

        lookAtTracker = Mathf.Clamp(lookAtTracker / 100f, -1, 1);
        if (IsWaitingForCar(20))
        {
            currentTrackerSpeed = 0;
        }
        else
        {
            currentTrackerSpeed = trackerSpeed;
        }

        if (IsWaitingForPlayer(100f))
        {
            ApplyTorque(Mathf.Abs(normalizedDirection.z), _motorTorque / 5);
            UpdateRotation(lookAtTracker, _steerAngle);
        }
        else
        {
            ApplyTorque(Mathf.Abs(normalizedDirection.z), _motorTorque);
            UpdateRotation(lookAtTracker, _steerAngle);
        }
        
    }

    private bool IsWaitingForCar(float distanceToCar)
    {
        if (Vector3.Distance(tracker.transform.position, transform.position) > distanceToCar)
        {
            return true;
        }
        return false;
    }
    private bool IsWaitingForPlayer(float distanceToPlayer)
    {
        if (player == null)
        {
            return false;
        }
        else
        {
            if (Vector3.Distance(transform.position, player.transform.position) > distanceToPlayer)
            {
                return true;
            }
            return false;
        }
    }
}
