using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public GameObject target;
    public float movementSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.localEulerAngles = transform.localEulerAngles + new Vector3(0, 0, movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.localEulerAngles = transform.localEulerAngles + new Vector3(0, 0, -movementSpeed * Time.deltaTime);
        }
    }
}
