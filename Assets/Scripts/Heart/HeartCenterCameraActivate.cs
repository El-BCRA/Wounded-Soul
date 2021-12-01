using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartCenterCameraActivate : MonoBehaviour
{
    [SerializeField] private CameraStateMachine instance;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Player enters activation collider
        if (collision.gameObject.CompareTag("Player"))
        {
            instance.m_Zone = CameraStateMachine.Zone.HeartCenter;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Player leaves activation collider
        if (collision.gameObject.CompareTag("Player"))
        {
            instance.m_Zone = CameraStateMachine.Zone.Heart;
        }
    }
}
