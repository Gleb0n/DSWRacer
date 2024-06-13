using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]
public struct CarAIStats
{

    public List<CheckpointSingle> checkpointSingles;
    public int currentCheckpointCount;

    public GameObject tracker;
    public float currentTrackerSpeed;

    public CarController player;
}
public class CarAI : Car, IStationStateSwitcher
{
    //TODO: set the enum type with the parameters move, brake, decelerate
    //in move, set the conditions for the movement of the car, for example, when the tractor is moving away from the car
    //in brake, set the conditions for braking, for example, when turning 
    //in decelerate, set conditions for deceleration, for example, when the player is too far from a given object

    [SerializeField] private float trackerSpeed;
    [SerializeField] private TrackCheckpoints trackCheckpoints;

    private CarAIStats _stats;
    private BaseState currentState;
    private List<BaseState> allStates;
    private void Awake()
    {

        _stats.tracker = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Destroy(_stats.tracker.GetComponent<Collider>());
        Destroy(_stats.tracker.GetComponent<MeshRenderer>());

        _stats.tracker.transform.position = this.transform.position;
        _stats.tracker.transform.rotation = this.transform.rotation;   

    }
    private void Start()
    {
        SetUpWheels();
        carRigidbody = GetComponent<Rigidbody>();
        _stats.player = FindObjectOfType<CarController>();
        _stats.checkpointSingles = trackCheckpoints.GetCheckpoints();
        _stats.currentTrackerSpeed = trackerSpeed;

        allStates = new List<BaseState>()
        {
            new DriveState(_stats, this, stateSwitcher: this),
            new TurnState(_stats, this, stateSwitcher: this),
            new BrakeState(_stats, this, stateSwitcher: this)
        };
        currentState = allStates[0];
    }

    private void FixedUpdate()
    {  
        ProgressTracker();
        FollowTracker();
    }
    private void ProgressTracker()
    {
        Vector3 currentCheckpointPosition = _stats.checkpointSingles[_stats.currentCheckpointCount].gameObject.transform.position;
        if(Vector3.Distance(_stats.tracker.transform.position, currentCheckpointPosition) < 5 )
        {
            _stats.currentCheckpointCount++;
        }
        if(_stats.currentCheckpointCount >= _stats.checkpointSingles.Count)
        {
            _stats.currentCheckpointCount = 0;
        }

        _stats.tracker.transform.LookAt(currentCheckpointPosition);

        currentState.MoveTracker(carRigidbody);
        
    }
    private void FollowTracker()
    {
        currentState.FollowTracker();
    }

    public bool IsWaitingForCar(float distanceToCar)
    {
        if (Vector3.Distance(_stats.tracker.transform.position, transform.position) > distanceToCar)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsWaitingForPlayer(float distanceToPlayer)
    {
        if (_stats.player == null)
        {
            return false;
        }
        else
        {
            if (Vector3.Distance(transform.position, _stats.player.transform.position) > distanceToPlayer)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public void SwitchState<T>() where T : BaseState
    {
        var state = allStates.FirstOrDefault(predicate: s => s is T);
        currentState = state;
    }
}
