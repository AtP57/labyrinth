using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenutitleAnimation : MonoBehaviour
{
    private RectTransform rt;
    private bool animationDirection = true;
    public float animationSpeed = 1.2f; // Adjust this for speed

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check bounds
        if (rt.localScale.x > 1.1f)
        {
            animationDirection = false;
        }

        if (rt.localScale.x < 0.95f)
        {
            animationDirection = true;
        }

        // Calculate scaling factor
        float scaleFactor = animationDirection ? animationSpeed : (2 - animationSpeed);

        // Apply scaling with Time.deltaTime
        float scaleAdjustment = Mathf.Pow(scaleFactor, Time.deltaTime);
        rt.localScale = rt.localScale * scaleAdjustment;
    }
}
