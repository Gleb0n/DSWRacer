using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAudio : MonoBehaviour
{
    AudioSource engineSound;
    CarController controller;

    float pithFromCar;
    float minPith = 0.75f;


    void Start()
    {
        engineSound = GetComponent<AudioSource>();
        controller = GetComponent<CarController>();
    }

    // Update is called once per frame
    void Update()
    {
        float normalizedCarSpeed = Mathf.Clamp(Mathf.Abs(50f ) / 50f * 3, 0f, 3f);
        pithFromCar = normalizedCarSpeed;

        if ( pithFromCar < minPith )
        {
            engineSound.pitch = minPith;
        }
        else
        {
            engineSound.pitch = pithFromCar;
        }
        
    }
}
