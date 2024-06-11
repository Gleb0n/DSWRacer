using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAudio : MonoBehaviour
{
    [SerializeField] private float maxPith;
    [SerializeField] private float minPith;
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;


    private AudioSource engineSound;
    private Rigidbody carRigidbody;
    private float pithFromCar;
    private float currentSpeed;

    private void Start()
    {
        engineSound = GetComponent<AudioSource>();
        carRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        currentSpeed = carRigidbody.velocity.magnitude;
        pithFromCar = currentSpeed / 50f;

        if (currentSpeed < minSpeed)
        {
            engineSound.pitch = minPith;
        }
        else if (currentSpeed > minSpeed &&  currentSpeed < maxSpeed)
        {
            engineSound.pitch = minPith + pithFromCar;
        }
        else
        {
            engineSound.pitch = maxPith + pithFromCar;
        }
        
    }
}
