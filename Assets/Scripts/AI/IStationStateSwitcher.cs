using UnityEngine;

public interface IStationStateSwitcher
{
    void SwitchState<T>() where T : BaseState;
}
