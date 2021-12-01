using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;
    public float waitTime;
    public float dropTime;

    // Start is called before the first frame update
    void Awake()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            waitTime = 0.5f;
        }
        if(Input.GetKey(KeyCode.S))
        {
            if (waitTime <= 0)
            {
                effector.rotationalOffset = 180f;
                waitTime = 0.5f;
                dropTime = 0f;
            } else
            {
                waitTime -= Time.deltaTime;
                dropTime += Time.deltaTime;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) || dropTime >= 0.5)
        {
            effector.rotationalOffset = 0;
        }
    }
}
