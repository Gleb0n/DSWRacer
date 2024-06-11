using UnityEngine;

public class StarterPoint : MonoBehaviour
{
    [SerializeField] private CarController carController;
    [SerializeField] private LapTimer lapTimer; 

    private void Awake()
    {
        // TODO: make function work for multiple players
        if (carController == null)
        {
            Debug.Log("Car not set");
        }
        else
        {
            Instantiate(carController);
            carController.transform.position = this.transform.position;
        }
    }

    private void Start()
    {
        if (lapTimer != null && !lapTimer.startFromCheckpoint)
        {
            lapTimer.StartRace(); // start
        }
    }
}
