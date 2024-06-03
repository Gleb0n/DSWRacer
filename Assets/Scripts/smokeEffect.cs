using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using UnityEngine;

public class smokeEffect : MonoBehaviour
{
    //smoke test
    public GameObject smokeTire; 
    private Car car;
    public GameObject[] smokes;
    public ParticleSystem[] SmokeParticles;
    private WheelHandler[] wheelHandler;
    public Transform[] wheelTransform;

    // Start is called before the first frame update
    void Start()
    {
        findValues();
        spawnSmoke();
    }

    // Update is called once per frame
    
    
    void FixedUpdate()
    {
        for (int i = 0; i < wheelHandler.Length; i++)
        {         
            if ((wheelHandler[i].carSpeed > 0 && wheelHandler[i].carSpeed < 50) && ((wheelHandler[i].availableTorque > 1500 && wheelHandler[i].availableTorque < 3500) || wheelHandler[i].availableTorque <-4500 ))
            {
                SmokeParticles[i].Play();
            }
            else
            {
                SmokeParticles[i].Stop();
            }
        }
    }

    void findValues()
    {
        car = GetComponent<CarController>();
        wheelHandler = car.wheels;
        wheelTransform = car.wheelsTransform;
        smokes = new GameObject [wheelTransform.Length];
        SmokeParticles = new ParticleSystem [wheelTransform.Length];
    }

    void spawnSmoke()
    {
        for (int i = 0; i < wheelHandler.Length; i++)
        {
            smokes[i] = Instantiate(smokeTire);
            SmokeParticles[i] = smokes[i].GetComponent<ParticleSystem>();
            smokes[i].transform.parent = wheelTransform[i].transform;
            smokes[i].transform.rotation = new Quaternion(90,90,90,0);
            smokes[i].transform.position = wheelTransform[i].transform.position;
            smokes[i].transform.localScale = new Vector3(1,1,1);
        }
    }
}


