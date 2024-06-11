using UnityEngine;

public class DriveState : BaseState
{

    public DriveState(CarAIStats stats, CarAI carAI, IStationStateSwitcher stateSwitcher) :
        base(stats, carAI, stateSwitcher)
    {

    }

    public override void MoveTracker(Rigidbody carRigidbody)
    {
        if (_carAI.IsWaitingForCar(20f))
        {
            _stats.tracker.transform.Translate(0, 0, 0);
        }
        else
        {
            _stats.tracker.transform.Translate(0, 0, (carRigidbody.velocity.magnitude * _stats.currentTrackerSpeed) * Time.deltaTime);
        }
    }

    public override void FollowTracker()
    {
        Vector3 normalizedDirection = (_stats.tracker.transform.position - _carAI.transform.position).normalized;
        float lookAtTracker = Vector3.SignedAngle(_carAI.transform.forward, normalizedDirection, Vector3.up);

        lookAtTracker = Mathf.Clamp(lookAtTracker * Mathf.Deg2Rad, -1, 1);

        _carAI.ApplyTorque(Mathf.Abs(normalizedDirection.z), _carAI._motorTorque);
        _carAI.UpdateRotation(lookAtTracker, _carAI._steerAngle);
        Debug.Log("Drive");
        CheckSwitch();
    }

    private void CheckSwitch()
    {
        CheckpointSingle currentCheckpoint = _stats.checkpointSingles[_stats.currentCheckpointCount];
        CheckpointSingle nextCheckpoint = _stats.checkpointSingles[_stats.currentCheckpointCount + 1];
        //If turn incorrectly check out CalculateAngle() result by Debug.Log()
        if (TrackCheckpoints.CalculateAngle(currentCheckpoint, nextCheckpoint) * Mathf.Rad2Deg > 1)
        {
            _stateSwitcher.SwitchState<TurnState>();
        }
        else if (_carAI.IsWaitingForPlayer(100f))
        {
            _stateSwitcher.SwitchState<BrakeState>();
        }
        
    }

}
