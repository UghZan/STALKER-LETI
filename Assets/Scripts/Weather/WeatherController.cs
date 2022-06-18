using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    [Header("References")]
    public Light Sun;
    public Light Moon;
    public Material Skybox;
    [Header("Time Of Day System")]
    public float TimeScale;
    public float DayLength = 960;
    public float NightLength = 480;
    public float IngameHourLength = 60;
    public bool IsDay() => TimeOfDay < DayLength;
    public bool IsNight() => TimeOfDay < DayLength + NightLength;
    public float TimeOfDay { get; private set; }
    float dayProgress, nightProgress, ambientIntensity, skyExposure, directIntensity;
    [Header("Weather System")]
    [SerializeField] WeatherPreset[] Weathers;
    public WeatherPreset CurrentWeather;
    [SerializeField] WeatherPreset NextWeather;
    [Header("Misc Settings")]
    [SerializeField] float weatherChangeSpeed = 20;
    [SerializeField] float chanceToChangeWeather = 0.25f;
    [SerializeField] float ambientChangeSpeed = -1;
    [SerializeField] float skyChangeSpeed = -1;
    [SerializeField] float fogChangeSpeed = 30;

    bool weatherChangeInProgress, skyChangeInProgress;
    float halfDayLength, halfNightLength, thirdOfDay, twoThirdsOfDay;
    float hoursPassed = 0, daysPassed = 0; //lighting updates happen every ingame hour (~30 seconds)
    bool updateHappened = true; //so that update happens only once
    // Start is called before the first frame update
    void Start()
    {
        halfDayLength = DayLength / 2;
        halfNightLength = NightLength / 2;
        thirdOfDay = DayLength / 3;
        twoThirdsOfDay = thirdOfDay * 2;

        if (skyChangeSpeed == -1) skyChangeSpeed = IngameHourLength * 2;
        if (ambientChangeSpeed == -1) ambientChangeSpeed = IngameHourLength * 0.2f;
        if (weatherChangeSpeed == -1) weatherChangeSpeed = IngameHourLength * 0.33f;
        if (fogChangeSpeed == -1) fogChangeSpeed = IngameHourLength * 0.5f;

        Skybox.SetTexture("_Tex", GetCurrentSkyboxTexture(CurrentWeather));
        Skybox.SetFloat("_Lerp", 0);
        RenderSettings.ambientIntensity = CurrentWeather.MinDayAmbientIntensity;
        HourlyUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        TimeOfDay += Time.deltaTime * TimeScale;
        if (TimeOfDay >= DayLength + NightLength)
        {
            daysPassed++;
            hoursPassed = 0;
            TimeOfDay = 0;
        }
        if (IsDay())
        {
            nightProgress = 0;
            dayProgress = TimeOfDay / DayLength;
        }
        else
        {
            dayProgress = 0;
            nightProgress = (TimeOfDay - DayLength) / NightLength;
        }

        if (Mathf.FloorToInt(TimeOfDay) % IngameHourLength == 0)
        {
            if (!updateHappened) HourlyUpdate();
        }
        else
        {
            if (updateHappened) updateHappened = false;
        }
    }

    void HourlyUpdate()
    {
        Debug.Log(hoursPassed);
        StartCoroutine(ChangeAmbientIntensity());
        if (hoursPassed == 4 || hoursPassed == 10 || hoursPassed == 15 || hoursPassed == 23)
            StartCoroutine(ChangeSkybox());
        else if (!skyChangeInProgress)
            TryChangeWeather();

        if (hoursPassed == DayLength / IngameHourLength)
            OnNightStart();
        if (hoursPassed == 0)
            OnDayStart();

        hoursPassed++;
        updateHappened = true;
    }

    private void LateUpdate()
    {
        RotateSun();
        ChangeLightingColor();
        CalculateLightIntensity();
    }

    void RotateSun()
    {
        if(IsDay())
        {
            Sun.enabled = true;
            Moon.enabled = false;
            Sun.transform.localRotation = Quaternion.AngleAxis(-10 + (TimeOfDay / DayLength) * 200, Vector3.right);
        }
        else
        {
            Sun.enabled = false;
            Moon.enabled = true;
            Sun.transform.localRotation = Quaternion.AngleAxis(190 + ((TimeOfDay - DayLength) / NightLength) * 170, Vector3.right);
        }
        Moon.transform.localRotation = Sun.transform.localRotation * Quaternion.AngleAxis(180, Vector3.right);
    }

    void OnDayStart()
    {
        Sun.shadowStrength = CurrentWeather.DayShadowIntensity;
        if (CurrentWeather.HasFogInDay)
        {
            StartCoroutine(ChangeFogSettings(CurrentWeather.DayFogColor, CurrentWeather.DayFogDensity));
        }
        else
        {
            StartCoroutine(ChangeFogSettings(Color.white, 0));
        }
    }

    void OnNightStart()
    {
        Moon.shadowStrength = CurrentWeather.NightShadowIntensity;
        if(CurrentWeather.HasFogInNight)
        {
            StartCoroutine(ChangeFogSettings(CurrentWeather.NightFogColor, CurrentWeather.NightFogDensity));
        }
        else
        {
            StartCoroutine(ChangeFogSettings(Color.white, 0));
        }
    }
    
    void ChangeLightingColor()
    {
        if(IsDay())
        {
            Sun.color = CurrentWeather.DayLightColor.Evaluate(dayProgress);
            Sun.intensity = directIntensity;
            Skybox.SetColor("_Tint", CurrentWeather.DaySkyColor.Evaluate(dayProgress) * 0.7f);
        }
        else
        {
            Moon.color = CurrentWeather.NightLightColor.Evaluate(nightProgress);
            Moon.intensity = directIntensity;
            Skybox.SetColor("_Tint", CurrentWeather.NightSkyColor.Evaluate(nightProgress) * 0.7f);
        }
        Skybox.SetFloat("_Exposure", skyExposure);
    }

    void SetSkyboxLerp(Texture prevTexture, Texture nextTexture)
    {
        Skybox.SetTexture("_Tex", prevTexture);
        Skybox.SetTexture("_Tex2", nextTexture);
    }

    Texture GetCurrentSkyboxTexture(WeatherPreset currentWeather)
    {
        if (hoursPassed >= 0 && hoursPassed < 6)
            return currentWeather.Skybox[0];
        else if (hoursPassed >= 6 && hoursPassed < 12)
            return currentWeather.Skybox[1];
        else if (hoursPassed >= 12 && hoursPassed < 18)
            return currentWeather.Skybox[2];
        else
            return currentWeather.Skybox[3];
    }

    Texture GetNextSkyboxTexture(WeatherPreset nextWeather)
    {
        if (hoursPassed >= 0 && hoursPassed < 6)
            return nextWeather.Skybox[1];
        else if (hoursPassed >= 6 && hoursPassed < 12)
            return nextWeather.Skybox[2];
        else if (hoursPassed >= 12 && hoursPassed < 18)
            return nextWeather.Skybox[3];
        else
            return nextWeather.Skybox[0];
    }

    void CalculateLightIntensity()
    {
        float intensity = 0;
        if(IsDay())
        {
            intensity = -Mathf.Pow((dayProgress - 0.5f)*2, 2) + 1;
            ambientIntensity = HelperMethods.ProjectOnRange(intensity, 0, 1, CurrentWeather.MinDayAmbientIntensity, CurrentWeather.MaxDayAmbientIntensity);
            directIntensity = HelperMethods.ProjectOnRange(intensity, 0, 1, CurrentWeather.MinDayDirIntensity, CurrentWeather.MaxDayDirIntensity);
            skyExposure = HelperMethods.ProjectOnRange(intensity, 0, 1, CurrentWeather.MinDaySkyExposure, CurrentWeather.MaxDaySkyExposure);
        }
        else
        {
            intensity = -Mathf.Pow((nightProgress - 0.5f)*2, 2) + 1;
            ambientIntensity = HelperMethods.ProjectOnRange(intensity, 0, 1, CurrentWeather.MinNightAmbientIntensity, CurrentWeather.MaxNightAmbientIntensity);
            directIntensity = HelperMethods.ProjectOnRange(intensity, 0, 1, CurrentWeather.MinNightDirIntensity, CurrentWeather.MaxNightDirIntensity);
            skyExposure = HelperMethods.ProjectOnRange(intensity, 0, 1, CurrentWeather.MinNightSkyExposure, CurrentWeather.MaxNightSkyExposure);
        }
        //Debug.Log(intensity);
        
    }

    void TryChangeWeather()
    {
        ChangeToWeather(CurrentWeather.PickWeather());
    }

    void ChangeToWeather(WeatherPreset nextWeather)
    {
        NextWeather = nextWeather;
        StartCoroutine(ChangeToWeatherGradual());
    }
    IEnumerator ChangeFogSettings(Color color, float density)
    {
        Color oldColor = RenderSettings.fogColor, temp;
        float time = 0, oldDensity = RenderSettings.fogDensity, tempDensity;
        while (time < fogChangeSpeed)
        {
            temp = Color.Lerp(oldColor, color, time / fogChangeSpeed);
            tempDensity = Mathf.Lerp(oldDensity, density, time / fogChangeSpeed);
            RenderSettings.fogColor = temp;
            RenderSettings.fogDensity = tempDensity;
            time += Time.deltaTime * TimeScale;
            yield return null;
        }
        RenderSettings.fogColor = color;
        RenderSettings.fogDensity = density;
        yield return null;
    }

    IEnumerator ChangeAmbientIntensity()
    {
        float old = RenderSettings.ambientIntensity;
        float time = 0;
        while(time < ambientChangeSpeed)
        {
            RenderSettings.ambientIntensity = Mathf.Lerp(old, ambientIntensity, time/ambientChangeSpeed);
            time += Time.deltaTime * TimeScale;
            yield return null;
        }
        RenderSettings.ambientIntensity = ambientIntensity;
        yield return null;
    }

    IEnumerator ChangeSkybox()
    {
        skyChangeInProgress = true;
        Skybox.SetFloat("_Lerp", 0);
        Texture cur = GetCurrentSkyboxTexture(CurrentWeather), next = GetNextSkyboxTexture(CurrentWeather);
        SetSkyboxLerp(cur, next);
        float time = 0;
        while(time < skyChangeSpeed)
        {
            Skybox.SetFloat("_Lerp", time / skyChangeSpeed);
            time += Time.deltaTime * TimeScale;
            yield return null;
        }
        Skybox.SetFloat("_Lerp", 1);
        SetSkyboxLerp(next, GetNextSkyboxTexture(CurrentWeather));
        Skybox.SetFloat("_Lerp", 0);
        skyChangeInProgress = false;
        yield return null;
    }

    IEnumerator ChangeToWeatherGradual()
    {
        weatherChangeInProgress = true;
        float time = 0;
        SetSkyboxLerp(GetCurrentSkyboxTexture(CurrentWeather), GetCurrentSkyboxTexture(NextWeather));
        while(time < weatherChangeSpeed)
        {
            Skybox.SetFloat("_Lerp", time / weatherChangeSpeed);
            time += Time.deltaTime * TimeScale;
            yield return null;
        }
        CurrentWeather = NextWeather;
        SetSkyboxLerp(GetCurrentSkyboxTexture(CurrentWeather), GetNextSkyboxTexture(CurrentWeather));
        Skybox.SetFloat("_Lerp", 0);
        weatherChangeInProgress = false;
        yield return null;
    }
}
