using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RelativeText : MonoBehaviour
{
    [SerializeField] private TextMeshPro m_TextPrefab;
    [SerializeField] private GameObject m_TargetLocation;

    // Update is called once per frame
    void Update()
    {
        
    }

    public TextMeshPro Create(string text)
    {
        TextMeshPro newText = Instantiate(m_TextPrefab, m_TargetLocation.transform.position, Quaternion.identity);
        newText.SetText(text);
        return newText;
    }
}