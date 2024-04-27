using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    public int resetDepth = -10;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Failsafe();
    }

    //check if the ball fell out from the map
    private void Failsafe()
    {
        if (transform.position.y < resetDepth)
        {
            transform.parent.GetComponent<RestartLabyrinth>().StartLabyrinth();
        }
    }
}
