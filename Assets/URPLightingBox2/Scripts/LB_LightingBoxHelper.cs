// Use this script to get runtime access to the lighting box econtrolled effects
/// <summary>
/// example :
/// 
/// // Update bloom effect .
/// void Start ()
/// {
///   	GameObject.FindObjectOfType<LB_LightingBoxHelper> ().Update_Bloom (true, 1f, 0.5f, Color.white);
/// }
/// </summary>
using UnityEngine;   
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

#if UNITY_EDITOR   
using UnityEditor;
#endif

#region Emum Types

public enum CameraMode
{
	Single,All,Custom
}
public enum WindowMode
{
	Part1,Part2,Part3,
	Finish
}
public enum AmbientLight
{
	Skybox,
	Color,
	Gradient
}
public enum LightingMode
{
	BakedProgressiveCPU, BakedProgressiveGPU,
	BakedEnlighten,
	RealtimeGI, FullyRealtime
}
public enum LightSettings
{
	Default,
	Realtime,
	Mixed,
	Baked
}
public enum MyColorSpace
{
	Linear,
	Gamma
}
public enum CustomFog
{
	Global,
	Height,
	Distance
}
public enum LightsShadow
{
	OnlyDirectionalSoft,OnlyDirectionalHard,
	AllLightsSoft,AllLightsHard,
	Off
}
public enum LightProbeMode
{
	Blend,
	Proxy
}
public enum Render_Path
{
	Forward
}

public enum DOFQuality
{
	Low,Medium,High
}

public enum AOType
{
	Classic,Modern
}

public enum ColorMode
{
	ACES,Neutral,LUT
}

public enum AAMode
{
	FXAA,SMAA,MSAA,None
}
#endregion

public class LB_LightingBoxHelper : MonoBehaviour {

	public LB_LightingProfile mainLightingProfile;
    public VolumeProfile volumeProfileMain;

    #region Runtime Update

    Light sunLight;
	Camera mainCamera;
	LB_LightingBoxHelper helper;

	void Start()
	{
		if (!mainCamera) {
			if (GameObject.Find (mainLightingProfile.mainCameraName))
				mainCamera = GameObject.Find (mainLightingProfile.mainCameraName).GetComponent<Camera> ();
			else
				mainCamera = GameObject.FindObjectOfType<Camera> ();
		}

		Update_SunRuntime (mainLightingProfile);
		UpdatePostEffects (mainLightingProfile);
		UpdateSettings (mainLightingProfile);
	}

	void UpdatePostEffects(LB_LightingProfile profile)
	{
		if(!helper)
			helper = GameObject.Find("LightingBox_Helper").GetComponent<LB_LightingBoxHelper> ();

		if (!profile)
			return;

		helper.UpdateProfiles (mainCamera, profile.volumeProfile);

		// MotionBlur
		helper.Update_MotionBlur (profile.MotionBlur_Enabled, profile.mbIntensity,profile.mbQuality);
		
		// Vignette
		helper.Update_Vignette (profile.Vignette_Enabled,profile.vignetteIntensity);


		// _ChromaticAberration
		helper.Update_ChromaticAberration(profile.Chromattic_Enabled,profile.CA_Intensity);

		helper.Update_Bloom(profile.Bloom_Enabled,profile.bIntensity,profile.bThreshould,profile.bColor,profile.dirtTexture,profile.dirtIntensity,profile.mobileOptimizedBloom,profile.bRotation);
       
        // Depth of Field
        helper.Update_DOF(mainCamera,profile.DOF_Enabled,profile.dofDistance, profile.dofFocusLength, profile.dofAperture);
/*
		// AO
		if (profile.AO_Enabled)
			helper.Update_AO(mainCamera,true,profile.aoType,profile.aoRadius,profile.aoIntensity,profile.ambientOnly,profile.aoColor, profile.aoQuality);
		else
			helper.Update_AO(mainCamera,false,profile.aoType,profile.aoRadius,profile.aoIntensity,profile.ambientOnly,profile.aoColor, profile.aoQuality);
        */

		// Color Grading
		helper.Update_ColorGrading(profile.colorMode,profile.exposureIntensity,profile.contrastValue,profile.temp,profile.saturation,profile.colorGamma,profile.colorLift,profile.gamma,profile.lut);


	}

	void UpdateSettings(LB_LightingProfile profile)
	{
		// Sun Light Update
		if (sunLight) {
			sunLight.color = profile.sunColor;
			sunLight.intensity = profile.sunIntensity;
			sunLight.bounceIntensity = profile.indirectIntensity;
		} else {
			Update_SunRuntime (profile);
		}

		if (profile.sunFlare)
		{
			if(sunLight)
				sunLight.flare = profile.sunFlare;
		}
		else
		{
			if(sunLight)
				sunLight.flare = null;
		}

		// Skybox
		helper.Update_SkyBox (profile.Ambient_Enabled, profile.skyBox);

		// Update Ambient
		helper.Update_Ambient (profile.Ambient_Enabled,profile.ambientLight, profile.ambientColor,profile.skyColor,profile.equatorColor,profile.groundColor);

        // Global Fog
        helper.Update_GlobalFog(mainCamera,profile.Fog_Enabled,profile.fogMode,profile.fogColor, profile.fogIntensity, profile.linearStart, profile.linearEnd);

    }

    void Update_SunRuntime(LB_LightingProfile profile)
	{
		if (profile.Sun_Enabled) {
			if (!RenderSettings.sun) {
				Light[] lights = GameObject.FindObjectsOfType<Light> ();
				foreach (Light l in lights) {
					if (l.type == LightType.Directional) {
						sunLight = l;

						if (profile.sunColor != Color.clear)
							profile.sunColor = l.color;
						else
							profile.sunColor = Color.white;

						//sunLight.shadowNormalBias = 0.05f;  
						sunLight.color = profile.sunColor;
						if (sunLight.bounceIntensity == 1f)
							sunLight.bounceIntensity = profile.indirectIntensity;
					}
				}
			} else {		
				sunLight = RenderSettings.sun;

				if (profile.sunColor != Color.clear)
					profile.sunColor = sunLight.color;
				else
					profile.sunColor = Color.white;

				//	sunLight.shadowNormalBias = 0.05f;  
				sunLight.color = profile.sunColor;
				if (sunLight.bounceIntensity == 1f)
					sunLight.bounceIntensity = profile.indirectIntensity;
			}
		}


	}

	#endregion

	public void Update_MainProfile(LB_LightingProfile profile, VolumeProfile volumeProfile)
	{
		if(profile)
			mainLightingProfile = profile;
        if (volumeProfile)
            volumeProfileMain = volumeProfile;
    }

	public void UpdateProfiles(Camera mainCamera, VolumeProfile volumeProfile)
	{
        if (!volumeProfile)
            return;

       /* if (GameObject.Find("Global URP Volume"))
            GameObject.DestroyImmediate(GameObject.Find("Sky and Fog Volume"));
            */
        if (!GameObject.Find("Global URP Volume"))
        {
            GameObject urpVolume = new GameObject();
            urpVolume.name = "Global URP Volume";
            urpVolume.AddComponent<Volume>();
            urpVolume.GetComponent<Volume>().isGlobal = true;
            urpVolume.GetComponent<Volume>().priority = 1f;
            if (volumeProfile)
                urpVolume.GetComponent<Volume>().sharedProfile = volumeProfile;
        }
        else
        {
            if (volumeProfile)
            {
                GameObject.Find("Global URP Volume").GetComponent<Volume>().sharedProfile = volumeProfile;
            }


        }
    
	}

	public void Update_MotionBlur(bool enabled,float mbIntensity, MotionBlurQuality mbQuality)
	{
	/*	MotionBlur mb;
		GameObject.Find("Global Volume").GetComponent<PostProcessVolume>().sharedProfile.TryGetSettings(out mb);
        */
        GameObject.Find("Global URP Volume").GetComponent<Volume>().sharedProfile.
                TryGet<UnityEngine.Rendering.Universal.MotionBlur>(out var mb);

        mb.active = enabled;
        mb.intensity.overrideState = true;
        mb.intensity.value = mbIntensity;

        mb.quality.overrideState = true;
        mb.quality.value = mbQuality;
    }

public void Update_Vignette(bool enabled, float intensity)
	{
        /*Vignette vi;
		GameObject.Find("Global Volume").GetComponent<PostProcessVolume>().sharedProfile.TryGetSettings(out vi);
        */
        GameObject.Find("Global URP Volume").GetComponent<Volume>().sharedProfile.
               TryGet<UnityEngine.Rendering.Universal.Vignette>(out var vi);

        vi.active = enabled; 

		vi.intensity.overrideState = true;
		vi.intensity.value = intensity;

		vi.smoothness.overrideState = true;
		vi.smoothness.value = 1f;

		vi.rounded.overrideState = true;
		vi.rounded.value = false;

	}

	public void Update_ChromaticAberration(bool enabled,float intensity)
	{
        GameObject.Find("Global URP Volume").GetComponent<Volume>().sharedProfile.
               TryGet<UnityEngine.Rendering.Universal.ChromaticAberration>(out var ca);

        ca.active = enabled;

        ca.intensity.overrideState = true;
		ca.intensity.value = intensity;

	}

	public void Update_Bloom(bool enabled,float intensity,float threshold,Color color,Texture2D dirtTexture,float dirtIntensity,bool highQuality,float bRotation)
	{
            GameObject.Find("Global URP Volume").GetComponent<Volume>().sharedProfile.
               TryGet<UnityEngine.Rendering.Universal.Bloom>(out var b);

            b.intensity.overrideState = true;
			b.intensity.value = intensity;
			b.threshold.overrideState = true;
			b.threshold.value = threshold;
			b.tint.overrideState = true;
			b.tint.value = color;


			b.highQualityFiltering.overrideState = true;
			b.highQualityFiltering.value = highQuality;

			b.dirtTexture.overrideState = true;
			b.dirtTexture.value = dirtTexture;

			b.dirtIntensity.overrideState = true;
			b.dirtIntensity.value = dirtIntensity;

			b.active = enabled;
		
	}

    [Header("Depth of Field")]
    public float dofDistance = 1f;
    public float dofFocusLength = 30f;
    public float dofAperture = 5f;

    public void Update_DOF(Camera mainCamera ,bool enabled,float dofDistance, float dofFocusLength, float dofAperture)
	{

        GameObject.Find("Global URP Volume").GetComponent<Volume>().sharedProfile.
               TryGet<UnityEngine.Rendering.Universal.DepthOfField>(out var dof);

        dof.active = enabled;

        dof.mode.overrideState = true;
        dof.mode.value = DepthOfFieldMode.Bokeh;

        if (!enabled)
        {
            dof.mode.overrideState = true;
            dof.mode.value = DepthOfFieldMode.Off;
        }

        dof.focusDistance.overrideState = true;
        dof.focusDistance.value = dofDistance;

        dof.focalLength.overrideState = true;
        dof.focalLength.value = dofFocusLength;

        dof.aperture.overrideState = true;
        dof.aperture.value = dofAperture;
    }

    public void Update_AA(Camera mainCamera ,AAMode aaMode, bool  enabled)
	{
		if (enabled) {
			
			if (aaMode == AAMode.FXAA) {
                if (mainCamera && mainCamera.GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>())
                {
                    mainCamera.GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>().antialiasing
                    = AntialiasingMode.FastApproximateAntialiasing;
                    mainCamera.allowMSAA = false;
                }
            }
            if (aaMode == AAMode.SMAA) {
                if (mainCamera && mainCamera.GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>())
                {
                    mainCamera.GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>().antialiasing
                    = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
                    mainCamera.allowMSAA = false;
                }
            }
            if (aaMode == AAMode.MSAA)
            {
                if (mainCamera && mainCamera.GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>())
                {
                    mainCamera.GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>().antialiasing
                       = AntialiasingMode.None;
                    mainCamera.allowMSAA = true;
                }
            }
            if (aaMode == AAMode.None)
            {
                if (mainCamera && mainCamera.GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>())
                {
                    mainCamera.GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>().antialiasing
                    = AntialiasingMode.None;
                    mainCamera.allowMSAA = false;
                }
            }
        } else {
            mainCamera.GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>().antialiasing
                    = AntialiasingMode.None;
        }
	}

	public void Update_AO(Camera mainCamera ,bool enabled,AOType aoType,float aoRadius,float aoIntensity,bool ambientOnly,Color aoColor)
	{

		/*AmbientOcclusion ao;
		GameObject.Find("Global Volume").GetComponent<PostProcessVolume>().sharedProfile.TryGetSettings(out ao);

		if (enabled) {
			if (aoType == AOType.Classic) {
				ao.enabled.overrideState = true;
				ao.enabled.value = true;
				ao.mode.overrideState = true;
				ao.mode.value = AmbientOcclusionMode.ScalableAmbientObscurance;
				ao.radius.overrideState = true;
				ao.radius.value = aoRadius;
				ao.ambientOnly.overrideState = true;
				ao.ambientOnly.value = ambientOnly;
				ao.color.overrideState = true;
				ao.color.value = aoColor;
				ao.intensity.overrideState = true;
				ao.intensity.value = aoIntensity;
				ao.quality.overrideState = true;
				ao.quality.value = aoQuality;
			}
			if (aoType == AOType.Modern) {		
				ao.enabled.overrideState = true;
				ao.enabled.value = true;
				ao.mode.overrideState = true;
				ao.mode.value = AmbientOcclusionMode.MultiScaleVolumetricObscurance;
				ao.radius.overrideState = true;
				ao.radius.value = aoRadius;
				ao.ambientOnly.overrideState = true;
				ao.ambientOnly.value = ambientOnly;
				ao.color.overrideState = true;
				ao.color.value = aoColor;
				ao.intensity.overrideState = true;
				ao.intensity.value = aoIntensity;
			}
		} else {
			ao.enabled.overrideState = true;
			ao.enabled.value = false;
		}*/
	}

    public void Update_ColorGrading(ColorMode colorMode, float exposureIntensity, float contrastValue, float temp
        , float saturation, Color colorGamma, Color colorLift, float gamma, Texture lut)
    {

        #region Tonemmaper

        GameObject.Find("Global URP Volume").GetComponent<Volume>().sharedProfile.
               TryGet<UnityEngine.Rendering.Universal.Tonemapping>(out var toneMap);

        if (colorMode == ColorMode.ACES)
        {
            toneMap.active = true;
            toneMap.mode.value = TonemappingMode.ACES;

        }
        if (colorMode == ColorMode.Neutral)
        {
            toneMap.active = true;
            toneMap.mode.value = TonemappingMode.Neutral;

        }
        #endregion

        #region ColorAdjustments

        GameObject.Find("Global URP Volume").GetComponent<Volume>().sharedProfile.
               TryGet<UnityEngine.Rendering.Universal.ColorAdjustments>(out var colorAdj);

        colorAdj.saturation.overrideState = true;
        colorAdj.saturation.value = saturation * 100;


        colorAdj.postExposure.overrideState = true;
        colorAdj.postExposure.value = exposureIntensity;

        colorAdj.contrast.overrideState = true;
        colorAdj.contrast.value = contrastValue * 100;
        colorAdj.active = true;

        #endregion

        #region WhiteBalance

        GameObject.Find("Global URP Volume").GetComponent<Volume>().sharedProfile.
        TryGet<UnityEngine.Rendering.Universal.WhiteBalance>(out var wBalance);

        wBalance.temperature.overrideState = true;
        wBalance.temperature.value = temp;
        wBalance.active = true;
        #endregion

        #region LiftGammaGain

        GameObject.Find("Global URP Volume").GetComponent<Volume>().sharedProfile.
             TryGet<UnityEngine.Rendering.Universal.LiftGammaGain>(out var gammaLiftGain);

            gammaLiftGain.lift.overrideState = true;
            gammaLiftGain.lift.value = new Vector4(colorLift.r, colorLift.g, colorLift.b, 0);

            gammaLiftGain.gamma.overrideState = true;
            gammaLiftGain.gamma.value = new Vector4(colorGamma.r, colorGamma.g, colorGamma.b, gamma);

            gammaLiftGain.gain.overrideState = false;
            gammaLiftGain.gain.value = new Vector4(gammaLiftGain.gain.value.x, gammaLiftGain.gain.value.y, gammaLiftGain.gain.value.z, 0);

            #endregion

    }

        public void Update_SkyBox(bool enabled ,Material material)
	{
		if (enabled) {
			if (material)
				RenderSettings.skybox = material;
		}
		
	}


	public void Update_LightingMode(bool enabled, LightingMode lightingMode, float indirectBounce, int directSamples, int indirectSamples, bool aoEnabled, float aoDistance, float aoIntensityDirect, float aoIntensityIndirect, float backfaceTolerance, float resolution)
	{
		if (enabled)
		{
#if UNITY_EDITOR

			LB_LightingSettingsHepler.SetDirectSamples(directSamples);
			LB_LightingSettingsHepler.SetIndirectSamples(indirectSamples);
			LightmapEditorSettings.bakeResolution = resolution;
			LightmapParameters a = new LightmapParameters();
			a.name = "Lighting Box Very-Low";
			a.resolution = 0.125f;
			a.clusterResolution = 0.4f;
			a.irradianceBudget = 96;
			a.irradianceQuality = 8192;
			a.modellingTolerance = 0.001f;
			a.stitchEdges = true;
			a.isTransparent = false;
			a.systemTag = -1;
			a.blurRadius = 2;
			a.antiAliasingSamples = 8;
			a.directLightQuality = 64;
			a.bakedLightmapTag = -1;
			a.AOQuality = 256;
			a.AOAntiAliasingSamples = 16;
			a.backFaceTolerance = backfaceTolerance;

			LB_LightingSettingsHepler.SetLightmapParameters(a);

			LB_LightingSettingsHepler.SetDirectionalMode(LightmapsMode.NonDirectional);
			LB_LightingSettingsHepler.SetAmbientOcclusion(aoEnabled);
			LB_LightingSettingsHepler.SetAmbientOcclusionDirect(aoIntensityDirect);
			LB_LightingSettingsHepler.SetAmbientOcclusionDistance(aoDistance);
			LB_LightingSettingsHepler.SetAmbientOcclusionIndirect(aoIntensityIndirect);
			LB_LightingSettingsHepler.SetBounceIntensity(indirectBounce);
#endif
			RenderSettings.reflectionIntensity = 0;
			RenderSettings.defaultReflectionMode = UnityEngine.Rendering.DefaultReflectionMode.Custom;

#if UNITY_EDITOR
			if (lightingMode == LightingMode.RealtimeGI)
			{
				Lightmapping.realtimeGI = true;
				Lightmapping.bakedGI = false;
				LightmapEditorSettings.lightmapper = LightmapEditorSettings.Lightmapper.Enlighten;
			}
			if (lightingMode == LightingMode.BakedProgressiveCPU)
			{
				Lightmapping.realtimeGI = false;
				Lightmapping.bakedGI = true;
				LightmapEditorSettings.lightmapper = LightmapEditorSettings.Lightmapper.ProgressiveCPU;
			}
			if (lightingMode == LightingMode.BakedProgressiveGPU)
			{
				Lightmapping.realtimeGI = false;
				Lightmapping.bakedGI = true;
				LightmapEditorSettings.lightmapper = LightmapEditorSettings.Lightmapper.ProgressiveGPU;
			}
			if (lightingMode == LightingMode.BakedEnlighten)
			{
				Lightmapping.realtimeGI = false;
				Lightmapping.bakedGI = true;
				LightmapEditorSettings.lightmapper = LightmapEditorSettings.Lightmapper.Enlighten;
			}
			if (lightingMode == LightingMode.FullyRealtime)
			{
				Lightmapping.realtimeGI = false;
				Lightmapping.bakedGI = false;
				LightmapEditorSettings.lightmapper = LightmapEditorSettings.Lightmapper.Enlighten;
			}
#endif
		}
	}

	public void Update_Ambient(bool enabled,AmbientLight ambientLight,Color ambientColor,Color skyColor,Color equatorColor
		,Color groundColor)
	{
		if (enabled) {
			if (ambientLight == AmbientLight.Color) {
				RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
				RenderSettings.ambientLight = ambientColor;
			}
			if (ambientLight == AmbientLight.Skybox)
				RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
			if (ambientLight == AmbientLight.Gradient) {
				RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
				RenderSettings.ambientSkyColor = skyColor;
				RenderSettings.ambientEquatorColor = equatorColor;
				RenderSettings.ambientGroundColor = groundColor;
			}


		}
	}

	#if UNITY_EDITOR
	public void Update_LightSettings(bool enabled, LightSettings lightSettings)
	{
		if(enabled)
		{
			if (lightSettings == LightSettings.Baked) {

				Light[] lights = GameObject.FindObjectsOfType<Light> ();

				foreach (Light l in lights) {
					SerializedObject serialLightSource = new SerializedObject(l);
					SerializedProperty SerialProperty  = serialLightSource.FindProperty("m_Lightmapping");
					SerialProperty.intValue = 2;
					serialLightSource.ApplyModifiedProperties ();
				}
			} 
			if (lightSettings == LightSettings.Realtime) {

				Light[] lights = GameObject.FindObjectsOfType<Light> ();

				foreach (Light l in lights) {
					SerializedObject serialLightSource = new SerializedObject(l);
					SerializedProperty SerialProperty  = serialLightSource.FindProperty("m_Lightmapping");
					SerialProperty.intValue = 4;
					serialLightSource.ApplyModifiedProperties ();
				}
			}
			if (lightSettings == LightSettings.Mixed) {

				Light[] lights = GameObject.FindObjectsOfType<Light> ();

				foreach (Light l in lights) {
					SerializedObject serialLightSource = new SerializedObject(l);
					SerializedProperty SerialProperty  = serialLightSource.FindProperty("m_Lightmapping");
					SerialProperty.intValue = 1;
					serialLightSource.ApplyModifiedProperties ();
				}

			}
		}
	}


	public void Update_ColorSpace(bool enabled, MyColorSpace colorSpace)
	{
		if (enabled) {
			if (colorSpace == MyColorSpace.Gamma)
				PlayerSettings.colorSpace = ColorSpace.Gamma;
			else
				PlayerSettings.colorSpace = ColorSpace.Linear;
		}
	}

	public void Update_AutoMode(bool enabled)
	{
		if(enabled)
			Lightmapping.giWorkflowMode = Lightmapping.GIWorkflowMode.Iterative;
		else
			Lightmapping.giWorkflowMode = Lightmapping.GIWorkflowMode.OnDemand;
	}

    public void Update_LightProbes(bool enabled, LightProbeMode lightProbesMode)
	{
		if (enabled) {
			if (lightProbesMode == LightProbeMode.Blend) {

				MeshRenderer[] renderers = GameObject.FindObjectsOfType<MeshRenderer> ();

				foreach (MeshRenderer mr in renderers) {
					if (!mr.gameObject.isStatic) {
						if (mr.gameObject.GetComponent<LightProbeProxyVolume> ()) {
							if (Application.isPlaying)
								Destroy (mr.gameObject.GetComponent<LightProbeProxyVolume> ());
							else
								DestroyImmediate (mr.gameObject.GetComponent<LightProbeProxyVolume> ());
						}
						mr.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.BlendProbes;
					}
				}
			}
			if (lightProbesMode == LightProbeMode.Proxy) {

				MeshRenderer[] renderers = GameObject.FindObjectsOfType<MeshRenderer> ();

				foreach (MeshRenderer mr in renderers) {

					if (!mr.gameObject.isStatic) {
						if (!mr.gameObject.GetComponent<LightProbeProxyVolume> ())
							mr.gameObject.AddComponent<LightProbeProxyVolume> ();
						mr.gameObject.GetComponent<LightProbeProxyVolume> ().resolutionMode = LightProbeProxyVolume.ResolutionMode.Custom;
						mr.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.UseProxyVolume;
					}
				}
			}
		}
	}

	public void Update_Shadows(bool enabled, LightsShadow lightsShadow)
	{
		if (enabled) {
			if (lightsShadow == LightsShadow.AllLightsSoft) {

				Light[] lights = GameObject.FindObjectsOfType<Light> ();

				foreach (Light l in lights) {
					if (l.type == LightType.Directional)
						l.shadows = LightShadows.Soft;

					if (l.type == LightType.Spot)
						l.shadows = LightShadows.Soft;

					if (l.type == LightType.Point)
						l.shadows = LightShadows.Soft;
				}
			}
			if (lightsShadow == LightsShadow.AllLightsHard) {

				Light[] lights = GameObject.FindObjectsOfType<Light> ();

				foreach (Light l in lights) {
					if (l.type == LightType.Directional)
						l.shadows = LightShadows.Hard;

					if (l.type == LightType.Spot)
						l.shadows = LightShadows.Hard;

					if (l.type == LightType.Point)
						l.shadows = LightShadows.Hard;
				}
			}
			if (lightsShadow == LightsShadow.OnlyDirectionalSoft) {

				Light[] lights = GameObject.FindObjectsOfType<Light> ();

				foreach (Light l in lights) {
					if (l.type == LightType.Directional)
						l.shadows = LightShadows.Soft;

					if (l.type == LightType.Spot)
						l.shadows = LightShadows.None;

					if (l.type == LightType.Point)
						l.shadows = LightShadows.None;
				}
			}
			if (lightsShadow == LightsShadow.OnlyDirectionalHard) {

				Light[] lights = GameObject.FindObjectsOfType<Light> ();

				foreach (Light l in lights) {
					if (l.type == LightType.Directional)
						l.shadows = LightShadows.Hard;

					if (l.type == LightType.Spot)
						l.shadows = LightShadows.None;

					if (l.type == LightType.Point)
						l.shadows = LightShadows.None;
				}
			}
			if (lightsShadow == LightsShadow.Off) {
				Light[] lights = GameObject.FindObjectsOfType<Light> ();

				foreach (Light l in lights)
					l.shadows = LightShadows.None;



			}
		}
	}

	#endif

	public void Update_RenderPath(bool enabled, Render_Path renderPath,Camera mainCamera)
	{
		if (enabled) {
			/*
            if (renderPath == Render_Path.Forward)
                mainCamera.GetComponent<UniversalAdditionalCameraData>().SetRenderer(0);

            mainCamera.allowHDR = true;
			mainCamera.allowMSAA = false;
			mainCamera.useOcclusionCulling = true;
            */
		}
	}

    public void Update_GlobalFog(Camera mainCamera, bool enabled, FogMode fogMode, Color fogColor,
        float fogIntensity, float linearStart, float linearEnd)
    {
        RenderSettings.fog = enabled;
        RenderSettings.fogColor = fogColor;
        RenderSettings.fogDensity = fogIntensity;
        RenderSettings.fogMode = fogMode;
        RenderSettings.fogStartDistance = linearStart;
        RenderSettings.fogEndDistance = linearEnd;
    }

    public void Update_Sun(bool enabled,Light sunLight,Color sunColor,float indirectIntensity)
	{
		if (enabled) {
			if (!RenderSettings.sun) {
				Light[] lights = GameObject.FindObjectsOfType<Light> ();

				foreach (Light l in lights) {
					if (l.type == LightType.Directional) {
						sunLight = l;

						if (sunColor != Color.clear)
							sunColor = sunLight.color;
						else
							sunColor = Color.white;

						//	sunLight.shadowNormalBias = 0.05f;  
						sunLight.color = sunColor;
						if (sunLight.bounceIntensity == 1f)
							sunLight.bounceIntensity = indirectIntensity;
					}
				}
			} else {
				sunLight = RenderSettings.sun;

				if (sunColor != Color.clear)
					sunColor = sunLight.color;
				else
					sunColor = Color.white;

				//sunLight.shadowNormalBias = 0.05f;  
				sunLight.color = sunColor;
				if (sunLight.bounceIntensity == 1f)
					sunLight.bounceIntensity = indirectIntensity;
			}
		}
	}

	bool effectsIsOn = true;

	public void Toggle_Effects()
	{
		effectsIsOn = !effectsIsOn;

        GameObject.Find("Global URP Volume").GetComponent<Volume>().enabled = effectsIsOn;
    }
}
