using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAcidSequence : MonoBehaviour
{
    [SerializeField] private AcidRiseScript m_ARS;

    public GameObject[] thingsToChange;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && m_ARS.active)
        {
            m_ARS.active = false;
            foreach (GameObject g in thingsToChange)
            {
                g.SetActive(!g.activeSelf);
            }
        }
    }
}
