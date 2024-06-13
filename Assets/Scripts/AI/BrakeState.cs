using UnityEngine;

public class BrakeState : BaseState
{

    public BrakeState(CarAIStats stats, CarAI carAI, IStationStateSwitcher stateSwitcher) :
        base(stats, carAI, stateSwitcher)
    {

    }
    Vector3 normalizedDirection;
    public override void MoveTracker(Rigidbody carRigidbody)
    {
        float stopTracker = Mathf.Lerp(carRigidbody.velocity.magnitude * _stats.currentTrackerSpeed * Time.deltaTime, 0, Time.deltaTime * 30f);
        _stats.tracker.transform.Translate(0, 0, stopTracker);
    }

    public override void FollowTracker()
    {
        normalizedDirection = (_stats.tracker.transform.position - _carAI.transform.position).normalized;
        float lookAtTracker = Vector3.SignedAngle(_carAI.transform.forward, normalizedDirection, Vector3.up);

        lookAtTracker = Mathf.Clamp(lookAtTracker * Mathf.Deg2Rad, -1, 1);

        _carAI.BrakeTorque(Mathf.Abs(normalizedDirection.z));
        _carAI.UpdateRotation(lookAtTracker, _carAI._steerAngle);

        CheckSwitch();
    }


    private void CheckSwitch()
    {
        if (!_carAI.IsWaitingForPlayer(100f))
        {
            _carAI._brakeForce = 0f;
            normalizedDirection = Vector3.zero;
           _stateSwitcher.SwitchState<DriveState>();
        }
        else
        {
            _carAI._brakeForce = 10f;
        }

    }
}
