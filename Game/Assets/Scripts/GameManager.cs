using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager s_instance;
    public static GameManager Instance => s_instance;

    private Field[] m_fields;
    [SerializeField] private AudioClip m_roosterClip;
    [SerializeField] private InputProcessor m_inputProcessor;
    [SerializeField] private DayTimeManager dayTimeManager;

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

        this.m_fields = FindObjectsOfType<Field>(true);
    }

    public void ProcessCurrentDay()
    {
        StartCoroutine(this.StartNewDay());
        foreach (var field in this.m_fields)
        {
            if(field.enabled && field.IsUnlocked)
                field.ProcessGrowth();
        }
        
        foreach (var field in this.m_fields)
        {
            if(field.IsUnlocked)
                field.ProcessNewDay();
        }
    }

    private IEnumerator StartNewDay()
    {
        PlayerHudUI.Instance.PlayFadeAnimation();
        m_inputProcessor.enabled = false;
        yield return new WaitForSeconds(1.3f);
        this.dayTimeManager.StartNewDay();
        AudioSource.PlayClipAtPoint(this.m_roosterClip, Camera.main.transform.position, 0.33f);
        yield return new WaitForSeconds(0.2f);
        m_inputProcessor.enabled = true;
    }
}