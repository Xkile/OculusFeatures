// Modified by AliyerEdon@gmail.com feb 2021
// Main source : https://www.youtube.com/watch?v=y6TCQfFB2xg
// How to use : Attach this component to the directional light  
 

using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

[ExecuteInEditMode]
public class LB_DayNightCycleMobile : MonoBehaviour
{
    [Space(3)]
    [Header("Sun and Moon")]
    public Light moonLight;
    public Light sunLight;

    [Space(3)]
    [Header("Overall settings")]
    // Use slider or time to determine time of the day night
    public bool timeBased = false;
    // Update fog settings according to the tome of the day
    // public bool updateFog = true;
    // Update custom skybox color (cube mapped skybox shader), procedural is updated automatically based on the sun
    public bool updateSkyBox = true;
    // Update fog settings based on the sun color settings
    //  public bool fogColorFromSun = false;
    // Transition speed between sky day color and night color
    public float nightFadeSpeed = 1f;

    [Space(3)]
    [Header("Time settings")]
    [Range(0, 86400)]
    public float timeSlider = 53653f; // Use this to control time of the day
    // [Range(0, 1000)]
    // public float fogDensityMultiplier = 170f; // Fog density multiplier
    [Range(0, 3)]
    public float sunIntensityMultiplier = 1f; // Sun intensity multiplier
    [Range(0, 360)]
    public float geoRotation = 0; // Sun intensity multiplier
    [Range(0, 3000)]
    // Speed of the day time cycle
    public int speed = 300;

    // calculate time
    float time; 
    TimeSpan currenttime;

    // Current day (is not important)   
    int days;

    // Calculated sun intensity
    float intensity;
    /*
    [Space(3)]
    [Header("Fog Color")]
    // Color of the fog in day and night   
    public Color fogday = new Color32(194, 222, 255, 255);
    public Color fognight = Color.gray;
    */
    [Space(3)]
    [Header("Sun Color")]
    // Color of the sun in the sunset and  day 
    public Color sunsetColor = new Color32(255, 150, 90, 255);
    public Color dayColor = new Color32(255, 247, 218, 255);


    [Space(3)]
    [Header("Skybox Color")]
    public Color skyNightColor = new Color32(64, 64, 67, 255);
    [HideInInspector] public bool isDay = true;
    public Color AmbientDay = new Color32(128, 128, 128, 255);
    public Color AmbientNight = new Color32(103, 110, 140, 255);

    private void Start()
    {
        sunLight = GetComponent<Light>();
    }

    void Update()
    {
        ChangeTime();
    }

    public void ChangeTime()
    {
        // Use slider to change time of the day or time count   
        if (timeBased)
            time += Time.deltaTime * speed;
        else
            time = timeSlider;

        // End of the day
        if (time > 86400)
        {
            days += 1;
            time = 0;
        }
        if (time >= 21600 && time  <= 65000)
        {
            try
            {
                sunLight.enabled = true;
                moonLight.enabled = false;
                RenderSettings.ambientLight= Color.Lerp(RenderSettings.ambientLight, AmbientDay,  Time.deltaTime*3);
            }
            catch { }
        }
        else 
        {
            try
            {
                sunLight.enabled = false;
                moonLight.enabled = true;
                RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, AmbientNight,  Time.deltaTime*3);
            }
            catch { }
        }
        currenttime = TimeSpan.FromSeconds(time);
        // string[] temptime = currenttime.ToString().Split(":"[0]);
        // timetext.text = temptime[0] + ":" + temptime[1];
        try
        {
            // Update the sun rotation
            sunLight.transform.rotation = Quaternion.Euler(new Vector3((time - 21600) / 86400 * 360, geoRotation, 0));
        }
        catch { }

        // Update the sun intensity based on the time of day
        if (time > 43200)
            intensity = (sunIntensityMultiplier + 1) + (43200 - time) / 43200;
        else
            intensity = (sunIntensityMultiplier + 1) + ((43200 - time) / 43200 * -1) ;// ((-1) - intensityMultiplier));

        // Update fog color and density (turn on from Window->Rendering->Lighting window)
        /* if (updateFog)
         {
             if (!fogColorFromSun)
                 RenderSettings.fogColor = Color.Lerp(fognight, fogday, intensity * intensity);
             else
                 RenderSettings.fogColor = Color.Lerp(fognight, sunLight.color, intensity * intensity);

             RenderSettings.fogDensity = intensity / fogDensityMultiplier;
         }*/

        // Update sun intensity and color based on the time of the day
        try
        {
            sunLight.intensity = intensity;
             sunLight.color = Color.Lerp(sunsetColor, dayColor, intensity - sunIntensityMultiplier);
        }
        catch { }

        // Update skybox ccolor
        if (updateSkyBox)
        {
            if (time >= 21600 && time <= 63000)
            {
                if (RenderSettings.skybox.shader == Shader.Find("Skybox/Cubemap"))
                    RenderSettings.skybox.SetColor("_Tint", Color.Lerp(sunsetColor, dayColor, intensity - sunIntensityMultiplier));
            }
            else
            {
                if (RenderSettings.skybox.shader == Shader.Find("Skybox/Cubemap"))
                    RenderSettings.skybox.SetColor("_Tint", Color.Lerp(RenderSettings.skybox.GetColor("_Tint"), skyNightColor, Time.deltaTime * (nightFadeSpeed)));
            }
        }
    }
}




