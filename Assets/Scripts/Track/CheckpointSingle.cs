using UnityEngine;

public class CheckpointSingle : MonoBehaviour
{
    private TrackCheckpoints trackCheckpoints;
    private bool isFirstCheckpointPassed = false; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CarController>(out CarController carController))
        {
            trackCheckpoints.CarThroughCheckpoint(this, other.transform);

            // Od pierwszego checkpointa 
            if (!isFirstCheckpointPassed && trackCheckpoints.GetCheckpoints().IndexOf(this) == 0)
            {
                isFirstCheckpointPassed = true;
                FindObjectOfType<LapTimer>().StartRaceFromCheckpoint();
            }
        }
    }

    public void SetTrackCheckpoint(TrackCheckpoints trackCheckpoints)
    {
        this.trackCheckpoints = trackCheckpoints;
    }
}
