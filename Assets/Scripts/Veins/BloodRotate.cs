using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodRotate : MonoBehaviour
{
    float m_AngularVelocity;

    // Start is called before the first frame update
    void Awake()
    {
        m_AngularVelocity = Random.Range(-45.0f, 45.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.0f, 0.0f, m_AngularVelocity * Time.deltaTime, Space.World);
    }
}
