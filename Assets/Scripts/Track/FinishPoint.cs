using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    [SerializeField] private TrackCheckpoints trackCheckpoints;
    [SerializeField] private int numberOfCircles = 1;
    public int currentNumberOfCircles
    {
        get; private set;
    }
    private void Awake()
    {
        currentNumberOfCircles = numberOfCircles;
        trackCheckpoints.OnLastCheckpointPassed += TrackCheckpoints_OnLastCheckpointPassed;
        this.gameObject.SetActive(false);
    }
    private void TrackCheckpoints_OnLastCheckpointPassed()
    {
        currentNumberOfCircles--;
        if(currentNumberOfCircles == 0)
        {
            this.gameObject.SetActive(true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<CarController>(out CarController controller))
        {
            Debug.Log("FINISH!!!");
        }
    }
}
