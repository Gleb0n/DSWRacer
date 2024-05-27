using System.Collections.Generic;
using UnityEngine;

public class CarAI : Car
{
    [SerializeField] private float trackerSpeed;
    [SerializeField] private TrackCheckpoints trackCheckpoints;

    private List<CheckpointSingle> checkpointSingles;
    private int currentCheckpointCount;

    private GameObject tracker;

    private void Awake()
    {
        tracker = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Destroy(tracker.GetComponent<Collider>());
        tracker.transform.position = this.transform.position;
        tracker.transform.rotation = this.transform.rotation;   

    }
    private void Start()
    {
        SetUpWheels();
        carRigidbody = GetComponent<Rigidbody>();
        checkpointSingles = trackCheckpoints.GetCheckpoints();
    }


    private void FixedUpdate()
    {  
        ProgressTracker();

        Vector3 normalizedDirection = (tracker.transform.position - this.transform.position).normalized;
        Quaternion lookAtCheckpoint = Quaternion.LookRotation(tracker.transform.position - this.transform.position);

        ApplyTorque(normalizedDirection.z, _motorTorque);
        UpdateRotation(lookAtCheckpoint.normalized.y, _steerAngle);
        Debug.Log(lookAtCheckpoint.normalized.y);
    }
    private void ProgressTracker()
    {
        Vector3 currentCheckpointPosition = checkpointSingles[currentCheckpointCount].gameObject.transform.position;
        if(Vector3.Distance(tracker.transform.position, currentCheckpointPosition) < 3 )
        {
            currentCheckpointCount++;
        }
        if(currentCheckpointCount >= checkpointSingles.Count)
        {
            currentCheckpointCount = 0;
        }

        tracker.transform.LookAt(currentCheckpointPosition);
        tracker.transform.Translate(0, 0, carRigidbody.velocity.magnitude * trackerSpeed * Time.deltaTime);
    }
}
