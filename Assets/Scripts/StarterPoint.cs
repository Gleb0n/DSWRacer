using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterPoint : MonoBehaviour
{
    [SerializeField] CarController carController;
    void Awake()
    {
        if (carController == null)
        {
            Debug.Log("Car doestn set");
        }
        else
        {
            Instantiate(carController);
        }
    }

    
}
