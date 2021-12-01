using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBubbles : MonoBehaviour
{
    // Collection of bubble prefabs
    [SerializeField] private GameObject m_BubblePrefab1;
    [SerializeField] private GameObject m_BubblePrefab2;
    [SerializeField] private GameObject m_BubblePrefab3;
    [SerializeField] private GameObject m_BubblePrefab4;
    [SerializeField] private GameObject m_BubblePrefab5;
    [SerializeField] private GameObject m_BubblePrefab6;

    [SerializeField] private float m_FirstSpawnDelay;
    [SerializeField] private float m_LocalSpeed;

    // For random bubble spawn
    public struct Bubble
    {
        public Bubble(GameObject prefab, float spawnTime, float riseSpeed)
        {
            m_prefab = prefab;
            m_SpawnTime = spawnTime;
            m_RiseSpeed = riseSpeed;
        }

        public GameObject m_prefab;
        public float m_SpawnTime;
        public float m_RiseSpeed;
    }

    private float m_SpawnTimer;
    private float m_TimeSinceLastSpawn = 0f;
    private List<Bubble> m_Bubbles;
    private Bubble m_nextSpawn;

    private void Awake()
    {
        // Set up bubble structs for eventual spawning
        Bubble bubble1 = new Bubble(m_BubblePrefab1, 6.0f, 2.0f); // Slowest spawn and rise speed
        Bubble bubble2 = new Bubble(m_BubblePrefab2, 5.25f, 2.75f); // 2nd slowest spawn and rise speed
        Bubble bubble3 = new Bubble(m_BubblePrefab3, 4.75f, 3.25f); // 2nd fastest spawn and rise speed
        Bubble bubble4 = new Bubble(m_BubblePrefab4, 6.0f, 4.0f); // Fastest spawn and rise speed
        Bubble bubble5 = new Bubble(m_BubblePrefab5, 5.25f, 2.75f); // 2nd slowest spawn and rise speed
        Bubble bubble6 = new Bubble(m_BubblePrefab6, 4.75f, 3.25f); // 2nd fastest spawn and rise speed
        m_Bubbles = new List<Bubble>();
        m_Bubbles.Add(bubble1);
        m_Bubbles.Add(bubble2);
        m_Bubbles.Add(bubble3);
        m_Bubbles.Add(bubble4);
        m_Bubbles.Add(bubble5);
        m_Bubbles.Add(bubble6);

        m_SpawnTimer = m_FirstSpawnDelay;
        m_nextSpawn = m_Bubbles[Random.Range(0, 6)];
    }

    // Update is called once per frame
    void Update()
    {
        if (m_TimeSinceLastSpawn >= m_SpawnTimer)
        {
            Vector3 myPosition = transform.position;
            myPosition.x += Random.Range(-5.0f, 5.0f);
            GameObject bubble = Instantiate(m_nextSpawn.m_prefab, myPosition, Quaternion.identity);
            bubble.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, m_LocalSpeed + m_nextSpawn.m_RiseSpeed);
            m_nextSpawn = m_Bubbles[Random.Range(0, 6)];
            m_SpawnTimer = m_nextSpawn.m_SpawnTime;
            m_TimeSinceLastSpawn = 0f;
        } else
        {
            m_TimeSinceLastSpawn += Time.deltaTime;
        }
    }
}
