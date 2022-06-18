using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weather Preset", menuName = "Weather")]
public class WeatherPreset : ScriptableObject
{
    [Header("Weather Settings")]
    public bool HasRain;
    public bool HasThunder;
    public float MinTime;
    public float MaxTime;

    [Header("Skybox Settings")]
    public Texture[] Skybox; //morning, day, evening, night
    public Gradient DaySkyColor;
    public Gradient NightSkyColor;
    public float MaxDaySkyExposure = 2.0f;
    public float MinDaySkyExposure = 0.0f;
    public float MaxNightSkyExposure = 0.2f;
    public float MinNightSkyExposure = 0.0f;

    [Header("Light Settings")]
    public Gradient DayLightColor;
    public Gradient NightLightColor;
    public float MaxDayAmbientIntensity = 2.0f;
    public float MinDayAmbientIntensity = 0.0f;
    public float MaxDayDirIntensity = 0.2f;
    public float MinDayDirIntensity = 0.0f;
    public float MaxNightAmbientIntensity = 0.2f;
    public float MinNightAmbientIntensity = 0.0f;
    public float MaxNightDirIntensity = 0.2f;
    public float MinNightDirIntensity = 0.0f;
    public float DayShadowIntensity = 1.0f;
    public float NightShadowIntensity = 1.0f;

    [Header("Fog Settings")]
    public bool HasFogInDay;
    public Color DayFogColor;
    public float DayFogDensity;
    public bool HasFogInNight;
    public Color NightFogColor;
    public float NightFogDensity;

    [Header("Transition Settings")]
    public Pair[] possibleTransitions;

    public WeatherPreset PickWeather()
    {
        int maxWeight = 0;
        for (int i = 0; i < possibleTransitions.Length; i++)
        {
            maxWeight += possibleTransitions[i].weight;
        }

        int _w = Random.Range(0, maxWeight);
        for (int i = 0; i < possibleTransitions.Length; i++)
        {
            _w -= possibleTransitions[i].weight;
            if (_w <= 0)
                return possibleTransitions[i].preset;
        }

    }
}

[System.Serializable]
public struct Pair
{
    public WeatherPreset preset;
    public int weight;
}
