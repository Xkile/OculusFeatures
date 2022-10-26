
using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Lighting Data", menuName = "Lighting Profile", order = 1)]
public class LB_LightingProfile : ScriptableObject {

	[Header("Camera")]
	public string mainCameraName = "Main Camera";

	public string objectName = "LB_LightingProfile";
	[Header("Profiles")]
    public VolumeProfile volumeProfile;

    [Header("Global")]   
	//public Render_Path renderPath = Render_Path.Forward;
	public  LightingMode lightingMode = LightingMode.RealtimeGI;
	public  float bakedResolution = 10f;
	public  LightSettings lightSettings = LightSettings.Mixed;
	public MyColorSpace colorSpace = MyColorSpace.Linear;


	// Progressive
	public int directSamples;
	public int indirectSamples;
	public float aoIntensityDirect;
	public float aoIntensityIndirect;
	public bool aoEnabled;
	public float aoDistance;
	public float indirectBounce;
	public float backfaceTolerance = 0.7f;

	[Header("Environment")]
	public Material skyBox;
	public  AmbientLight ambientLight = AmbientLight.Color;
	public  Color ambientColor = new Color32(96,104,116,255);
	public Color skyColor = Color.white;
	public Color equatorColor = Color.white,groundColor = Color.white;

	[Header("Sun")]
	public  Color sunColor = Color.white;
	public float sunIntensity = 2.1f;
	public Flare sunFlare;
	public float indirectIntensity = 1.43f;

    [Header("Fog")]
    public FogMode fogMode = FogMode.Exponential;
    public Color fogColor = Color.white;
    public float fogIntensity;
    public float linearStart;
    public float linearEnd;

    [Header("Bloom")]
	public float bIntensity = 0.73f;
	public float bThreshould = 0.5f;
	public Color bColor = Color.white;
	public Texture2D dirtTexture;
	public float dirtIntensity;
	public bool mobileOptimizedBloom = false;
	public float bRotation;

	[Header("AO")]
	/*public AOType aoType = AOType.Modern;
	public float aoRadius = 0.3f;
	public float aoIntensity = 1f;
	public bool ambientOnly = false;
	public Color aoColor = Color.black;
	//public AmbientOcclusionQuality aoQuality = AmbientOcclusionQuality.Medium;*/

	[Header("Other")]
	public LightsShadow lightsShadow = LightsShadow.OnlyDirectionalSoft;
	public LightProbeMode lightProbesMode;
	public bool automaticLightmap = false;

	[Header("Depth of Field")]
	public float dofDistance = 1f;
	public float dofFocusLength = 30f;   
	public float dofAperture = 5f;
    
	[Header("Color settings")]
	public float exposureIntensity = 1.43f;
	public float contrastValue = 30f;
	public float temp = 0;
	public ColorMode colorMode = ColorMode.ACES;
	public float saturation = 0;
	public float gamma = 0;
	public Color colorGamma = Color.black;
	public Color colorLift = Color.black;
	public Texture lut;

	[Header("Effects")]
	public float vignetteIntensity = 0.1f;
	public float CA_Intensity = 0.1f;
    
	[Header("AA")]
	public AAMode aaMode;

    [Header("Motion Blur")]
    public bool enabled;
    public float mbIntensity;
    public MotionBlurQuality mbQuality;

    [Header("Enabled Options")]
	public bool Ambient_Enabled = true;
	public bool Scene_Enabled = true;
	public bool Sun_Enabled = true;
	public bool VL_Enabled = false;
	public bool Fog_Enabled = false;
	public bool DOF_Enabled = true;
	public bool Bloom_Enabled = false;
	public bool AA_Enabled = true;
	public bool AO_Enabled = false;
	public bool MotionBlur_Enabled = true;
	public bool Vignette_Enabled = true;
	public bool Chromattic_Enabled = true;

	public bool ambientState = false;
	public bool sunState = false;
	public bool lightSettingsState = false;
	public bool cameraState = false;
	public bool profileState = false;
	public bool buildState = false;
	public bool fogState = false;
	public bool dofState = false;
	public bool colorState = false;
	public bool bloomState = false;
	public bool aaState = false;
	public bool aoState = false;
	public bool motionBlurState = false;
	public bool vignetteState = false;
	public bool chromatticState = false;
	public bool OptionsState = true;
	public bool LightingBoxState = true;
}