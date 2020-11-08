using System;
using Scriptables;
using UnityEngine;

public class DayTimeManager : MonoBehaviour
{
    [SerializeField] private Light m_directionalLight;
    [SerializeField] private LightningPreset m_lightningPreset;

    [SerializeField] private float m_timeOfDay;

    [SerializeField] private float m_dayInSeconds;

    private int m_currentDay;
    private const int REAL_DAY_IN_SECONDS = 86400;
    private void Awake()
    {
        this.m_timeOfDay = this.m_dayInSeconds/3.333f;
        this.m_currentDay = 1;
    }

    private void Update()
    {
        if (this.m_timeOfDay <= this.m_dayInSeconds)
        {
            this.m_timeOfDay += Time.deltaTime;
            this.UpdateLightning(this.m_timeOfDay / m_dayInSeconds);   
        }
    }

    public void StartNewDay()
    {
        this.m_timeOfDay = this.m_dayInSeconds/4;
        this.m_currentDay++;
    }

    private void UpdateLightning(float timePercent)
    {
        RenderSettings.ambientLight = this.m_lightningPreset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = this.m_lightningPreset.FogColor.Evaluate(timePercent);

        this.m_directionalLight.color = this.m_lightningPreset.DirectionalColor.Evaluate(timePercent);
        this.m_directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0f));
    }

    public string GetDateTime()
    {
        var dayProgress = this.m_timeOfDay / this.m_dayInSeconds;
        var realTime = TimeSpan.FromSeconds(REAL_DAY_IN_SECONDS * dayProgress);
        return $"Day {this.m_currentDay} - {realTime.Hours:00}:{realTime.Minutes:00}";
    }
}