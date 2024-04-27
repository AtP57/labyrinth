using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float rotationSpeed = 60;
    public float maxTiltAngle = 40;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (transform.localEulerAngles.z < maxTiltAngle || transform.localEulerAngles.z > 359f-maxTiltAngle)
            {
                transform.localEulerAngles = transform.localEulerAngles + new Vector3(0,0, rotationSpeed * Time.deltaTime);
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (transform.localEulerAngles.z > 360f-maxTiltAngle || transform.localEulerAngles.z < 41)
            {
                transform.localEulerAngles = transform.localEulerAngles + new Vector3(0,0, -rotationSpeed * Time.deltaTime);
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (transform.localEulerAngles.x > 360f - maxTiltAngle || transform.localEulerAngles.x < 41)
            {
                transform.localEulerAngles = transform.localEulerAngles + new Vector3(-rotationSpeed * Time.deltaTime, 0, 0);
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (transform.localEulerAngles.x < maxTiltAngle || transform.localEulerAngles.x > 359f - maxTiltAngle)
            {
                transform.localEulerAngles = transform.localEulerAngles + new Vector3(rotationSpeed * Time.deltaTime, 0, 0);
            }
        }
    }
}
