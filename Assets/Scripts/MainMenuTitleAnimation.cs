using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenutitleAnimation : MonoBehaviour
{
    public float animationSpeed = 1.01f;
    private bool animationDirection = true;
    private RectTransform rt;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rt.localScale.x > 1.1f || rt.localScale.x < 0.95f) 
        {
            animationDirection = !animationDirection;
        } 
        if (animationDirection)
        {
            rt.localScale = new Vector3(rt.localScale.x, rt.localScale.y, rt.localScale.z) * (animationSpeed);
        } else
        {
            rt.localScale = new Vector3(rt.localScale.x, rt.localScale.y, rt.localScale.z) * (2-animationSpeed);
        }
    }
}
