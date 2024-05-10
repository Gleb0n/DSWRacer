using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedometerUI : MonoBehaviour
{
    private CarController carController;
    private Rigidbody carRigidbody;

    private TMP_Text textMeshPro;

    private void Start()
    {
        carController = FindObjectOfType<CarController>();
        carRigidbody = carController.GetComponent<Rigidbody>();

        textMeshPro = GetComponent<TMP_Text>();

    }
    private void Update()
    {
        int carVelocity = (int) carRigidbody.velocity.magnitude;
        textMeshPro.text = carVelocity.ToString() + "Km/h";
    }
}
