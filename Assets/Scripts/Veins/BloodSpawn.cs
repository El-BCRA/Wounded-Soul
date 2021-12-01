using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSpawn : MonoBehaviour
{
    // Collection of blood prefabs
    [SerializeField] private GameObject m_BloodPrefab1;
    [SerializeField] private GameObject m_BloodPrefab2;
    [SerializeField] private GameObject m_BloodPrefab3;
    [SerializeField] private GameObject m_BloodPrefab4;
    [SerializeField] private GameObject m_BloodPrefab5;
    [SerializeField] private GameObject m_BloodPrefab6;
    [SerializeField] private GameObject m_BloodPrefab7;

    [SerializeField] private float m_FirstSpawnDelay;

    // For random blood spawn
    public struct Blood
    {
        public Blood(GameObject prefab, float spawnTime, float moveSpeed)
        {
            m_prefab = prefab;
            m_SpawnTime = spawnTime;
            m_MoveSpeed = moveSpeed;
        }

        public GameObject m_prefab;
        public float m_SpawnTime;
        public float m_MoveSpeed;
    }

    private float m_SpawnTimer;
    private float m_TimeSinceLastSpawn = 0f;
    private List<Blood> m_Bloods;
    private Blood m_nextSpawn;

    void Awake()
    {
        // Set up blood structs for eventual spawning
        Blood blood1 = new Blood(m_BloodPrefab1, 2.2f, 20.0f); // Slowest spawn and move speed
        Blood blood2 = new Blood(m_BloodPrefab2, 2.0f, 19.0f); // 2nd slowest spawn and move speed
        Blood blood3 = new Blood(m_BloodPrefab3, 1.8f, 18.0f); // 2nd fastest spawn and move speed
        Blood blood4 = new Blood(m_BloodPrefab4, 1.6f, 17.0f); // Fastest spawn and move speed
        Blood blood5 = new Blood(m_BloodPrefab5, 1.4f, 16.0f); // 2nd slowest spawn and move speed
        Blood blood6 = new Blood(m_BloodPrefab6, 1.2f, 15.0f); // 2nd fastest spawn and move speed
        Blood blood7 = new Blood(m_BloodPrefab6, 1.0f, 14.0f); // 2nd fastest spawn and move speed
        m_Bloods = new List<Blood>();
        m_Bloods.Add(blood1);
        m_Bloods.Add(blood2);
        m_Bloods.Add(blood3);
        m_Bloods.Add(blood4);
        m_Bloods.Add(blood5);
        m_Bloods.Add(blood6);
        m_Bloods.Add(blood7);

        m_SpawnTimer = m_FirstSpawnDelay;
        m_nextSpawn = m_Bloods[Random.Range(0, 7)];
    }

    // Update is called once per frame
    void Update()
    {
        if (m_TimeSinceLastSpawn >= m_SpawnTimer)
        {
            Vector3 myPosition = transform.position;
            myPosition.y += Random.Range(-1.0f, 1.0f);
            GameObject blood = Instantiate(m_nextSpawn.m_prefab, myPosition, Quaternion.identity);
            blood.GetComponent<Rigidbody2D>().velocity = new Vector2(m_nextSpawn.m_MoveSpeed * -1, 0.0f);
            blood.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-180.0f, 180.0f);
            m_nextSpawn = m_Bloods[Random.Range(0, 7)];
            m_SpawnTimer = m_nextSpawn.m_SpawnTime;
            m_TimeSinceLastSpawn = 0f;
        }
        else
        {
            m_TimeSinceLastSpawn += Time.deltaTime;
        }
    }
}
