using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBehaviour : MonoBehaviour
{
    public float goalDistance = 0.3f;

    public GameObject ball;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isGoal())
        {
            transform.parent.GetComponent<RestartLabyrinth>().StartLabyrinth();
        }
    }

    public bool isGoal()
    {
        if (Vector3.Distance(transform.position, ball.transform.position) < goalDistance)
        {
            return true;
        } else
        { return false; }
    }
}
