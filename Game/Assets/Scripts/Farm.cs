using System;
using UnityEngine;

public class Farm : MonoBehaviour
{
    private static Farm s_instance;


    [SerializeField] private Field[] m_fields;

    private int m_currentIndex = 0;

    public static Farm Instance => s_instance ?? FindObjectOfType<Farm>();
    
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
    }

    public void UnlockNextFarm()
    {
        this.m_currentIndex++;
        this.m_fields[this.m_currentIndex].Unlock();
    }
}