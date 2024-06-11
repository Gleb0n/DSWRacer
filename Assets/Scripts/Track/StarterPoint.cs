using UnityEngine;

public class StarterPoint : MonoBehaviour
{
    [SerializeField] private CarController carController;

    private void Awake()
    {
        //TODO: make function work for multiple players
        if (carController == null)
        {
            Debug.Log("Car doestn set");
        }
        else
        {
            Instantiate(carController);
            carController.transform.position = this.transform.position;
        }
    }

    

    
}
