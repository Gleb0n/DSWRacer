using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] float carSpeed, carAcceleration;
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        float vector3 = transform.localPosition.x + carSpeed + (carAcceleration * Time.fixedDeltaTime);
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddRelativeForce(0f, 0f, vector3);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddRelativeForce(vector3, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddRelativeForce(-vector3, 0f, 0f);
        }
    }
}
