using UnityEngine;

public abstract class BaseState
{
    protected readonly CarAIStats _stats;
    protected readonly CarAI _carAI;
    protected readonly IStationStateSwitcher _stateSwitcher;

    public BaseState(CarAIStats stats, CarAI carAI, IStationStateSwitcher stateSwitcher)
    {
        _stats = stats;
        _carAI = carAI;
        _stateSwitcher = stateSwitcher;
    }

    public abstract void MoveTracker(Rigidbody carRigidbody);

    public abstract void FollowTracker();
}
