using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager s_instance;
    public static GameManager Instance => s_instance;

    private Field[] m_fields;

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;   
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        this.m_fields = FindObjectsOfType<Field>();
    }

    public void ProcessCurrentDay()
    {
        foreach (var field in this.m_fields)
        {
            field.ProcessGrowth();
        }
        
        foreach (var field in this.m_fields)
        {
            field.ProcessNewDay();
        }
    }
}