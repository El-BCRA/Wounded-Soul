using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropletSpawn : MonoBehaviour
{
    [SerializeField] private GameObject m_DropletPrefab;
    [SerializeField] public Animator animator;
    [SerializeField] private float m_DripRate;
    [SerializeField] private float m_DripSpeed;
    private float m_TimeSinceLastSpawn = 0f;

    // Update is called once per frame
    void Update()
    {
        if (m_TimeSinceLastSpawn >= m_DripRate - .9167f && m_TimeSinceLastSpawn <= m_DripRate)
        {
            animator.SetTrigger("SpawningDroplet");
        }
        if (m_TimeSinceLastSpawn >= m_DripRate)
        {
            Vector3 newPos = new Vector3(transform.position.x + 0.35f, transform.position.y - 2.0f, 0.997f);
            GameObject droplet = Instantiate(m_DropletPrefab, newPos, Quaternion.identity);
            droplet.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, m_DripSpeed);
            m_TimeSinceLastSpawn = 0f;
            animator.ResetTrigger("SpawningDroplet");
        }
        else
        {
            m_TimeSinceLastSpawn += Time.deltaTime;
        }
    }
}
