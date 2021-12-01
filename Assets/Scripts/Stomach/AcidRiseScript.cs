using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidRiseScript : MonoBehaviour
{
    public float m_AcidRiseSpeed;

    public bool active = false;

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            Vector3 riseVector = new Vector3(0.0f, m_AcidRiseSpeed, 0.0f);
            transform.Translate(riseVector * Time.deltaTime, Space.World);
        }
        if (transform.position.y > -50)
        {
            active = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            active = true;
            transform.GetComponent<CapsuleCollider2D>().enabled = false;
        }
    }
}
