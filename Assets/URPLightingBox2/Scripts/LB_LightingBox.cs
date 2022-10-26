
// AliyerEdon@gmail.com/
// Lighting Box is an "paid" asset. Don't share it for free

#if UNITY_EDITOR   
using UnityEngine;   
using System.Collections;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class LB_LightingBox : EditorWindow
{
	#region Variables

	public WindowMode winMode = WindowMode.Part1;
	public LB_LightingBoxHelper helper;
	public bool webGL_Mobile = false;

    // Global Fog
    public FogMode fogMode = FogMode.Exponential;
    public Color fogColor = Color.white;
    public float fogIntensity;
    public float linearStart;
    public float linearEnd;

    // AA
    public AAMode aaMode;

	// AO
	/*public AOType aoType;
	public float aoRadius;
	public float aoIntensity = 1f;
	public bool ambientOnly = true;
	public Color aoColor = Color.black;
	public AmbientOcclusionQuality aoQuality = AmbientOcclusionQuality.Medium;
    */
	// Bloom
	public float bIntensity = 1f;
	public float bThreshould = 0.5f;
	public Color bColor = Color.white;
	public Texture2D dirtTexture;
	public float dirtIntensity;
	public bool mobileOptimizedBloom;
	public float bRotation;

	public bool visualize;

	// Color settings
	public float exposureIntensity = 1.43f;
	public float contrastValue = 30f;
	public float temp = 0;
	public ColorMode colorMode;
	public float gamma = 0;
	public Color colorGamma = Color.black;
	public Color colorLift = Color.black;
	public float saturation = 0;
	public Texture lut;

	public float vignetteIntensity = 0.1f;
	public float CA_Intensity = 0.1f;

	//public Render_Path renderPath;

	// Profiles
	public LB_LightingProfile LB_LightingProfile;
    public VolumeProfile volumeProfileMain;

    public LightingMode lightingMode;
	public AmbientLight ambientLight;
	public LightSettings lightSettings;
	public LightProbeMode lightprobeMode;

   // Depth of Field
    public float dofDistance = 1f;
    public float dofFocusLength = 30f;
    public float dofAperture = 5f;

    // Sky and Sun
    public Material skyBox;
	public Light sunLight;
	public Flare sunFlare;
	public Color sunColor = Color.white;
	public float sunIntensity = 2.1f;
	public float indirectIntensity = 1.43f;
	public  Color ambientColor = new Color32(96,104,116,255);
	public Color skyColor;
	public Color equatorColor,groundColor;

	public bool autoMode;
	public MyColorSpace colorSpace;

	public LightsShadow psShadow;
	public float bakedResolution = 10f;
	public bool helpBox;

	// Private variabled
	Color redColor;
	bool lightError;
	bool lightChecked;
	GUIStyle myFoldoutStyle;
	bool showLogs;
	// Display window elements (Lighting Box)   
	Vector2 scrollPos = Vector2.zero;

	// Camera
	public Camera mainCamera;

	// Progressive
	public int directSamples;
	public int indirectSamples;
	public float aoIntensityDirect;
	public float aoIntensityIndirect;
	public bool aoEnabled;
	public float aoDistance;
	public float indirectBounce;
	public float backfaceTolerance = 0.7f;

	//Motion Blur
	public bool enabled;
    public float mbIntensity;
    public MotionBlurQuality mbQuality;

    #endregion

    #region Init()
    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/URP Lighting Box 2 %E")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
	////	LB_LightingBox window = (LB_LightingBox)EditorWindow.GetWindow(typeof(LB_LightingBox));
		System.Type inspectorType = System.Type.GetType("UnityEditor.InspectorWindow,UnityEditor.dll");
		LB_LightingBox window = (LB_LightingBox)EditorWindow.GetWindow<LB_LightingBox>("URP Lighting Box 2", true, new System.Type[] {inspectorType} );

		window.Show();
		window.autoRepaintOnSceneChange = true;
		window.maxSize = new Vector2 (1000f, 1000f);
		window.minSize = new Vector2 (387f, 1000f);

	}
	#endregion

	#region Options
	// Internal Usage
	public bool LightingBoxState = true,OptionsState = true;
	public bool ambientState = true;
	public bool sunState = true;
	public bool lightSettingsState = true;
	public bool cameraState = true;
	public bool profileState = true;
	public bool buildState = true;
	public bool vLightState = true;
	public bool sunShaftState = true;
	public bool fogState = true;
	public bool dofState = true;
	public bool autoFocusState =  true;
	public bool colorState = true;
	public bool bloomState = true;
	public bool aaState = true;
	public bool aoState = true;
	public bool motionBlurState = true;
	public bool vignetteState = true;
	public bool chromatticState = true;
	public bool ssrState = true;
	public bool st_ssrState;
	public bool foliageState = true;
	public bool snowState = true;

	// Effects enabled
	public bool Ambient_Enabled = true;
	public bool Scene_Enabled = true;
	public bool Sun_Enabled = true;
	public bool VL_Enabled = false;
	public bool SunShaft_Enabled = false;
	public bool Fog_Enabled = false;
	public bool DOF_Enabled = true;
	public bool Bloom_Enabled = false;
	public bool AA_Enabled = true;
	public bool AO_Enabled = false;
	public bool MotionBlur_Enabled = true;
	public bool Vignette_Enabled = true;
	public bool Chromattic_Enabled = true;
	public bool SSR_Enabled = false;
	public bool AutoFocus_Enabled = false;
	public bool ST_SSR_Enabled = false;

	Texture2D arrowOn,arrowOff;

	#endregion

	void NewSceneInit()
	{
        if (EditorSceneManager.GetActiveScene().name == "")
        {
            LB_LightingProfile = Resources.Load("DefaultSettings") as LB_LightingProfile;
            helper.Update_MainProfile(LB_LightingProfile, volumeProfileMain);

            OnLoad();
            currentScene = EditorSceneManager.GetActiveScene().name;

        }
        else
        {
            if (System.String.IsNullOrEmpty(EditorPrefs.GetString(EditorSceneManager.GetActiveScene().name)))
            {
                LB_LightingProfile = Resources.Load("DefaultSettings") as LB_LightingProfile;
                helper.Update_MainProfile(LB_LightingProfile, volumeProfileMain);

            }
            else
            {
                LB_LightingProfile = (LB_LightingProfile)AssetDatabase.LoadAssetAtPath(EditorPrefs.GetString(EditorSceneManager.GetActiveScene().name), typeof(LB_LightingProfile));
                helper.Update_MainProfile(LB_LightingProfile, volumeProfileMain);

            }

            OnLoad();
            currentScene = EditorSceneManager.GetActiveScene().name;

        }
    }

	// Load and apply default settings when a new scene opened
	void OnNewSceneOpened()
	{
		NewSceneInit ();
	}

	void OnDisable()
	{
		EditorApplication.hierarchyWindowChanged -= OnNewSceneOpened;
	}

	void OnEnable()
	{
		arrowOn = Resources.Load ("arrowOn") as Texture2D;
		arrowOff = Resources.Load ("arrowOff") as Texture2D;

		if (!GameObject.Find ("LightingBox_Helper")) 
		{
			GameObject helperObject = new GameObject ("LightingBox_Helper");
			helperObject.AddComponent<LB_LightingBoxHelper> ();
			helper = helperObject.GetComponent<LB_LightingBoxHelper> ();
		}

		EditorApplication.hierarchyWindowChanged += OnNewSceneOpened;

		currentScene = EditorSceneManager.GetActiveScene().name;

		if (System.String.IsNullOrEmpty (EditorPrefs.GetString (EditorSceneManager.GetActiveScene ().name)))
			LB_LightingProfile = Resources.Load ("DefaultSettings")as LB_LightingProfile;
		else
			LB_LightingProfile = (LB_LightingProfile)AssetDatabase.LoadAssetAtPath (EditorPrefs.GetString (EditorSceneManager.GetActiveScene ().name), typeof(LB_LightingProfile));

		OnLoad ();

	}

	void OnGUI()
	{

		#region Styles
		GUIStyle redStyle = new GUIStyle (EditorStyles.label);
		redStyle.alignment = TextAnchor.MiddleLeft;
		redStyle.normal.textColor = Color.red;

		GUIStyle blueStyle = new GUIStyle (EditorStyles.label);
		blueStyle.alignment = TextAnchor.MiddleLeft;
		blueStyle.normal.textColor = Color.blue;


		GUIStyle stateButton = new GUIStyle ();
		stateButton = "Label";
		stateButton.alignment = TextAnchor.MiddleLeft;
		stateButton.fontStyle = FontStyle.Bold;

		#endregion

		#region GUI start implementation
		Undo.RecordObject (this, "lb");

		scrollPos = EditorGUILayout.BeginScrollView (scrollPos,
			false,
			false,
			GUILayout.Width (this.position.width),
			GUILayout.Height (this.position.height));

		EditorGUILayout.Space ();

		GUILayout.Label ("URP Lighting Box 2 - v1.2 Unity 2019.4.2 on July 2021", EditorStyles.helpBox);


		EditorGUILayout.BeginHorizontal ();

		if (!helpBox) {
			if (GUILayout.Button ("Show Help",  GUILayout.Width (177), GUILayout.Height (24f))) {
				helpBox = !helpBox;
			}
		} else {
			if (GUILayout.Button ("Hide Help",  GUILayout.Width (177), GUILayout.Height (24f))) {
				helpBox = !helpBox;
			}
		}
		if (GUILayout.Button ("Refresh",  GUILayout.Width (179), GUILayout.Height (24f))) {
			UpdateSettings ();
			UpdatePostEffects ();
		}

		EditorGUILayout.EndHorizontal ();

		if (EditorPrefs.GetInt ("RateLB") != 3) {

			if (GUILayout.Button ("Rate Lighting Box")) {
				EditorPrefs.SetInt ("RateLB", 3);
				Application.OpenURL ("http://u3d.as/278u");
			}
		}

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();


		#endregion

		#region Tabs
		EditorGUILayout.BeginHorizontal ();
		//----------------------------------------------
		if (winMode == WindowMode.Part1)
			GUI.backgroundColor = Color.green;
		else
			GUI.backgroundColor = Color.white;
		//----------------------------------------------
		if (GUILayout.Button ("Scene",  GUILayout.Width (87), GUILayout.Height (43)))
			winMode = WindowMode.Part1;
		//----------------------------------------------
		if (winMode == WindowMode.Part2)
			GUI.backgroundColor = Color.green;
		else
			GUI.backgroundColor = Color.white;
		//----------------------------------------------
		if (GUILayout.Button ("Effect",  GUILayout.Width (87), GUILayout.Height (43)))
			winMode = WindowMode.Part2;
		//----------------------------------------------
		if (winMode == WindowMode.Part3)
			GUI.backgroundColor = Color.green;
		else
			GUI.backgroundColor = Color.white;
		//----------------------------------------------
		if (GUILayout.Button ("Color",  GUILayout.Width (87), GUILayout.Height (43)))
			winMode = WindowMode.Part3;
		//----------------------------------------------
		if (winMode == WindowMode.Finish)
			GUI.backgroundColor = Color.green;
		else
			GUI.backgroundColor = Color.white;
		//----------------------------------------------
		if (GUILayout.Button ("Screen",  GUILayout.Width (87), GUILayout.Height (43)))
			winMode = WindowMode.Finish;
		//----------------------------------------------
		GUI.backgroundColor = Color.white;
		//----------------------------------------------//----------------------------------------------//----------------------------------------------
		
	    EditorGUILayout.EndHorizontal ();

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		#endregion

		#region Toolbar

		EditorGUILayout.BeginHorizontal();
		if (LightingBoxState) 
		{
			if (GUILayout.Button ("Effects On",  GUILayout.Width (177), GUILayout.Height (24))) {
				helper.Toggle_Effects ();

				LightingBoxState = !LightingBoxState;

				if(LB_LightingProfile)
					LB_LightingProfile.LightingBoxState = LightingBoxState;
			}
		} else {
			if (GUILayout.Button ("Effects Off",  GUILayout.Width (177), GUILayout.Height (24))) {
				helper.Toggle_Effects ();
				LightingBoxState = !LightingBoxState;

				if(LB_LightingProfile)
					LB_LightingProfile.LightingBoxState = LightingBoxState;
			}
		}
		if(OptionsState)
		{
			if (GUILayout.Button ("Expand All",  GUILayout.Width (179), GUILayout.Height (24))){
				ambientState = sunState = lightSettingsState = true;
				cameraState = profileState = buildState = true;
				vLightState = sunShaftState = fogState = true;
				dofState = colorState = true;
				bloomState = aaState = aoState = true;
				motionBlurState = vignetteState = chromatticState = true;
				ssrState = foliageState = snowState = st_ssrState = true;
				OptionsState = !OptionsState;

				if(LB_LightingProfile)
				{
					LB_LightingProfile.ambientState = ambientState;
					LB_LightingProfile.sunState = sunState;
					LB_LightingProfile.lightSettingsState = lightSettingsState;
					LB_LightingProfile.cameraState = cameraState;
					LB_LightingProfile.profileState = profileState;
					LB_LightingProfile.buildState = buildState;
					LB_LightingProfile.fogState = fogState;
					LB_LightingProfile.dofState = dofState;
					LB_LightingProfile.colorState = colorState;
					LB_LightingProfile.bloomState = bloomState;
					LB_LightingProfile.aaState = aaState;
					LB_LightingProfile.aoState = aoState;
					LB_LightingProfile.motionBlurState = motionBlurState;
					LB_LightingProfile.vignetteState = vignetteState;
					LB_LightingProfile.chromatticState = chromatticState;
					LB_LightingProfile.OptionsState  = OptionsState;
					EditorUtility.SetDirty (LB_LightingProfile);
				}

			}
		}
		else
		{
			if (GUILayout.Button ("Close All",  GUILayout.Width (179), GUILayout.Height (24))) {

				ambientState = sunState = lightSettingsState = false;
				cameraState = profileState = buildState = false;
				vLightState = sunShaftState = fogState = false;
				dofState =  colorState = false;
				bloomState = aaState = aoState = false;
				motionBlurState = vignetteState = chromatticState = false;
				ssrState = foliageState = snowState = st_ssrState = false;
				OptionsState = !OptionsState;

				if(LB_LightingProfile)
				{
					LB_LightingProfile.ambientState = ambientState;
					LB_LightingProfile.sunState = sunState;
					LB_LightingProfile.lightSettingsState = lightSettingsState;
					LB_LightingProfile.cameraState = cameraState;
					LB_LightingProfile.profileState = profileState;
					LB_LightingProfile.buildState = buildState;
					LB_LightingProfile.fogState = fogState;
					LB_LightingProfile.dofState = dofState;
					LB_LightingProfile.colorState = colorState;
					LB_LightingProfile.bloomState = bloomState;
					LB_LightingProfile.aaState = aaState;
					LB_LightingProfile.aoState = aoState;
					LB_LightingProfile.motionBlurState = motionBlurState;
					LB_LightingProfile.vignetteState = vignetteState;
					LB_LightingProfile.chromatticState = chromatticState;					
					LB_LightingProfile.OptionsState  = OptionsState;
					EditorUtility.SetDirty (LB_LightingProfile);
				}

			}
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		#endregion
		   
		if (winMode == WindowMode.Part1) {

            #region Toggle Settings


            #endregion

            #region Profiles

            //-----------Profile----------------------------------------------------------------------------
            GUILayout.BeginVertical("Box");

            EditorGUILayout.BeginHorizontal();

            if (profileState)
                GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
            else
                GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));

            var profileStateRef = profileState;

            if (GUILayout.Button("Profile", stateButton, GUILayout.Width(300), GUILayout.Height(17f)))
            {
                profileState = !profileState;
            }

            if (profileStateRef != profileState)
            {

                if (LB_LightingProfile)
                {
                    LB_LightingProfile.profileState = profileState;
                    EditorUtility.SetDirty(LB_LightingProfile);
                }

            }
            EditorGUILayout.EndHorizontal();

            GUILayout.EndVertical();
            //---------------------------------------------------------------------------------------

            if (profileState)
            {
                if (helpBox)
                    EditorGUILayout.HelpBox("1. LB_LightingBox settings profile   2.URP Pipeline Profile", MessageType.Info);

                var lightingProfileRef = LB_LightingProfile;
                //	var postProcessingProfileRef = postProcessingProfile;

                EditorGUILayout.BeginHorizontal();
                LB_LightingProfile = EditorGUILayout.ObjectField("Lighting Profile", LB_LightingProfile, typeof(LB_LightingProfile), true) as LB_LightingProfile;

                if (GUILayout.Button("New", GUILayout.Width(43), GUILayout.Height(17)))
                {

                    if (EditorSceneManager.GetActiveScene().name == "")
                        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());

                    string path = EditorUtility.SaveFilePanelInProject("Save As ...", "Lighting_Profile_" + EditorSceneManager.GetActiveScene().name, "asset", "");

                    if (path != "")
                    {
                        LB_LightingProfile = new LB_LightingProfile();

                        AssetDatabase.CreateAsset(LB_LightingProfile, path);
                        AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(Resources.Load("DefaultSettings_LB")), path);
                        LB_LightingProfile = (LB_LightingProfile)AssetDatabase.LoadAssetAtPath(path, typeof(LB_LightingProfile));
                        helper.Update_MainProfile(LB_LightingProfile, volumeProfileMain);

                        AssetDatabase.Refresh();
                        /*
						string path2 = System.IO.Path.GetDirectoryName(path) + "/Post_Profile_"+EditorSceneManager.GetActiveScene ().name+".asset";
						// Create new post processing stack 2 profile
						postProcessingProfile = new PostProcessProfile ();
						AssetDatabase.CreateAsset (postProcessingProfile, path2);
						AssetDatabase.CopyAsset (AssetDatabase.GetAssetPath (Resources.Load ("Default_Post_Profile")), path2);
						postProcessingProfile = (PostProcessProfile)AssetDatabase.LoadAssetAtPath (path2, typeof(PostProcessProfile));
						LB_LightingProfile.postProcessingProfile = postProcessingProfile;
                       
						AssetDatabase.Refresh ();
						    */
                        string path3 = System.IO.Path.GetDirectoryName(path) + "/Volume_Profile_" + EditorSceneManager.GetActiveScene().name + ".asset";
                        // Create new HD Pipeline profile
                        volumeProfileMain = new VolumeProfile();
                        AssetDatabase.CreateAsset(volumeProfileMain, path3);
                        AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(Resources.Load("Default_Volume_Profile")), path3);
                        volumeProfileMain = (VolumeProfile)AssetDatabase.LoadAssetAtPath(path3, typeof(VolumeProfile));
                        LB_LightingProfile.volumeProfile = volumeProfileMain;

                        AssetDatabase.Refresh();
                    }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();

                if (lightingProfileRef != LB_LightingProfile)
                {

                    helper.Update_MainProfile(LB_LightingProfile, volumeProfileMain);
                    OnLoad();
                    EditorPrefs.SetString(EditorSceneManager.GetActiveScene().name, AssetDatabase.GetAssetPath(LB_LightingProfile));

                    if (LB_LightingProfile)
                        EditorUtility.SetDirty(LB_LightingProfile);
                }
                /*
				if (postProcessingProfileRef != postProcessingProfile)
				{
					if (LB_LightingProfile)
					{
						LB_LightingProfile.postProcessingProfile = postProcessingProfile;
						EditorUtility.SetDirty (LB_LightingProfile);
					}
                    
					UpdatePostEffects ();

				}
				*/



                if (helpBox)
                    EditorGUILayout.HelpBox("Which camera should has effects", MessageType.Info);

                EditorGUILayout.BeginHorizontal();
                var mainCameraRef = mainCamera;

                mainCamera = EditorGUILayout.ObjectField("Target Camera", mainCamera, typeof(Camera), true) as Camera;
                if (GUILayout.Button("Save", GUILayout.Width(43), GUILayout.Height(17)))
                {
                    if (LB_LightingProfile)
                    {
                        LB_LightingProfile.mainCameraName = mainCamera.name;
                        EditorUtility.SetDirty(LB_LightingProfile);
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                if (mainCameraRef != mainCamera)
                {
                    UpdatePostEffects();
                    UpdateSettings();

                    if (LB_LightingProfile)
                    {
                        LB_LightingProfile.mainCameraName = mainCamera.name;
                        EditorUtility.SetDirty(LB_LightingProfile);
                    }
                }

                /*var webGL_MobileRef = webGL_Mobile;

				webGL_Mobile = EditorGUILayout.Toggle ("WebGL 2.0 Target", webGL_Mobile);

				if (webGL_MobileRef != webGL_Mobile) {
					if (LB_LightingProfile)
					{
						LB_LightingProfile.webGL_Mobile = webGL_Mobile;
						EditorUtility.SetDirty (LB_LightingProfile);
					}
				}
                */
                EditorGUILayout.Space();
                EditorGUILayout.Space();

            }

            #endregion

            #region Ambient

            //-----------Ambient----------------------------------------------------------------------------
            GUILayout.BeginVertical ("Box");

			EditorGUILayout.BeginHorizontal ();

			if(ambientState)
				GUILayout.Label(arrowOn,stateButton,GUILayout.Width(20),GUILayout.Height(14));
			else
				GUILayout.Label(arrowOff,stateButton,GUILayout.Width(20),GUILayout.Height(14));
			
			var Ambient_EnabledRef = Ambient_Enabled;
			var ambientStateRef = ambientState;

			if (GUILayout.Button ("Ambient", stateButton, GUILayout.Width (300), GUILayout.Height (17f))) {
				ambientState = !ambientState;
			}

			if(ambientStateRef != ambientState )
			{
				if (LB_LightingProfile)
				{
					LB_LightingProfile.ambientState = ambientState;
					EditorUtility.SetDirty (LB_LightingProfile);
				}
			}

			EditorGUILayout.EndHorizontal ();

			GUILayout.EndVertical ();
            //---------------------------------------------------------------------------------------


            if (ambientState) {
                if (helpBox)
                    EditorGUILayout.HelpBox("Assign scene skybox material here   ", MessageType.Info);

                var skyboxRef = skyBox;

                Ambient_Enabled = EditorGUILayout.Toggle("Enabled", Ambient_Enabled);
                EditorGUILayout.Space();

                skyBox = EditorGUILayout.ObjectField("SkyBox Material", skyBox, typeof(Material), true) as Material;

                if (skyboxRef != skyBox) {

                    helper.Update_SkyBox(Ambient_Enabled, skyBox);

                    if (LB_LightingProfile)
                    {
                        LB_LightingProfile.skyBox = skyBox;
                        EditorUtility.SetDirty(LB_LightingProfile);
                    }
                }


                if (helpBox)
                    EditorGUILayout.HelpBox("Set ambient lighting source as Skybox(IBL) or a simple color", MessageType.Info);

                var ambientLightRef = ambientLight;
                var ambientColorRef = ambientColor;
                var skyBoxRef = skyBox;
                var skyColorRef = skyColor;
                var equatorColorRef = equatorColor;
                var groundColorRef = groundColor;

                // choose ambient lighting mode   (color or skybox)
                ambientLight = (AmbientLight)EditorGUILayout.EnumPopup("Ambient Source", ambientLight, GUILayout.Width(343));

                if (ambientLight == AmbientLight.Color)
                {
                    ambientColor = EditorGUILayout.ColorField(new GUIContent("Color"), ambientColor, true, false, true, new ColorPickerHDRConfig(-10f, 10f, -10f, 10f), null);
                }
                if (ambientLight == AmbientLight.Gradient)
				{
                    skyColor = EditorGUILayout.ColorField(new GUIContent("Sky Tint"), skyColor, true, false, true, new ColorPickerHDRConfig(-10f, 10f, -10f, 10f), null);

                    equatorColor = EditorGUILayout.ColorField(new GUIContent("Equator Color"), equatorColor, true, false, true, new ColorPickerHDRConfig(-10f, 10f, -10f, 10f), null);
                    groundColor = EditorGUILayout.ColorField(new GUIContent("Ground Color"), groundColor, true, false, true, new ColorPickerHDRConfig(-10f, 10f, -10f, 10f), null);

				}
				
				if (ambientLightRef != ambientLight || ambientColorRef != ambientColor
					|| skyBoxRef != skyBox || skyColorRef != skyColor
					|| equatorColorRef != equatorColor || groundColorRef != groundColor
					|| Ambient_EnabledRef != Ambient_Enabled  )
				{
					helper.Update_Ambient (Ambient_Enabled,ambientLight, ambientColor,skyColor,equatorColor,groundColor);

					if (LB_LightingProfile)
					{
						LB_LightingProfile.ambientColor = ambientColor;
						LB_LightingProfile.ambientLight = ambientLight;
						LB_LightingProfile.skyBox = skyBox;
						LB_LightingProfile.skyColor = skyColor;
						LB_LightingProfile.equatorColor = equatorColor;
						LB_LightingProfile.groundColor = groundColor;
						LB_LightingProfile.Ambient_Enabled = Ambient_Enabled;
						EditorUtility.SetDirty (LB_LightingProfile);
					}
				}
				//----------------------------------------------------------------------
			}
			#endregion

			#region Sun Light
			//-----------Sun----------------------------------------------------------------------------
			GUILayout.BeginVertical ("Box");

			EditorGUILayout.BeginHorizontal ();

			if(sunState)
				GUILayout.Label(arrowOn,stateButton,GUILayout.Width(20),GUILayout.Height(14));
			else
				GUILayout.Label(arrowOff,stateButton,GUILayout.Width(20),GUILayout.Height(14));

			var sunStateRef = sunState;

			if (GUILayout.Button ("Sun", stateButton, GUILayout.Width (300), GUILayout.Height (17f))) {
				sunState = !sunState;
			}

			if(sunStateRef != sunState)
			{
				if (LB_LightingProfile)
				{
					LB_LightingProfile.sunState = sunState;
					EditorUtility.SetDirty (LB_LightingProfile);
				}
			}

			EditorGUILayout.EndHorizontal ();

			GUILayout.EndVertical ();
			//---------------------------------------------------------------------------------------


			if (sunState) {
				if (helpBox)
					EditorGUILayout.HelpBox ("Sun /  Moon light settings", MessageType.Info);
				
				var Sun_EnabledRef = Sun_Enabled;

				Sun_Enabled = EditorGUILayout.Toggle("Enabled",Sun_Enabled);

				EditorGUILayout.Space();

				EditorGUILayout.BeginHorizontal ();
				sunLight = EditorGUILayout.ObjectField ("Sun Light", sunLight, typeof(Light), true) as Light;
				if (!sunLight){
					if (GUILayout.Button ("Find"))
						Update_Sun ();
				}
				EditorGUILayout.EndHorizontal ();
				var sunColorRef = sunColor;

				sunColor = EditorGUILayout.ColorField ("Color", sunColor);

				var sunIntensityRef = sunIntensity;
				var indirectIntensityRef = indirectIntensity;

				sunIntensity = EditorGUILayout.Slider ("Intenity", sunIntensity, 0, 4f);
				indirectIntensity = EditorGUILayout.Slider ("Indirect Intensity", indirectIntensity, 0, 4f);

				var sunFlareRef = sunFlare;

				sunFlare = EditorGUILayout.ObjectField ("Lens Flare", sunFlare, typeof(Flare), true) as Flare;

				if (Sun_EnabledRef != Sun_Enabled)
				{					
					if (LB_LightingProfile)
					{
						LB_LightingProfile.Sun_Enabled = Sun_Enabled;
						EditorUtility.SetDirty (LB_LightingProfile);
					}
				}

				if(Sun_Enabled)
				{

					if (sunColorRef != sunColor || Sun_EnabledRef != Sun_Enabled) {
						
						if (sunLight)
							sunLight.color = sunColor;
						else
							Update_Sun ();		
						if (LB_LightingProfile)
						{
							LB_LightingProfile.sunColor = sunColor;
							EditorUtility.SetDirty (LB_LightingProfile);
						}
					}

					if (sunIntensityRef != sunIntensity || indirectIntensityRef != indirectIntensity
						|| Sun_EnabledRef != Sun_Enabled) {

						if (sunLight) {
							sunLight.intensity = sunIntensity;
							sunLight.bounceIntensity = indirectIntensity;
						} else
							Update_Sun ();
						if (LB_LightingProfile) {
							LB_LightingProfile.sunIntensity = sunIntensity;
							LB_LightingProfile.indirectIntensity = indirectIntensity;
							LB_LightingProfile.Sun_Enabled = Sun_Enabled;
						}
						if (LB_LightingProfile)
						{
							LB_LightingProfile.sunState = sunState;
							EditorUtility.SetDirty (LB_LightingProfile);
						}
					}
					if (sunFlareRef != sunFlare) {
						if (sunFlare) {
							if (sunLight)
								sunLight.flare = sunFlare;
						} else {
							if (sunLight)
								sunLight.flare = null;
						}

						if (LB_LightingProfile)
						{
							LB_LightingProfile.sunFlare = sunFlare;
							LB_LightingProfile.sunState = sunState;
							EditorUtility.SetDirty (LB_LightingProfile);
						}
					}
				}
			}
			#endregion

			#region Lighting Mode


			//-----------Light Settings----------------------------------------------------------------------------
			GUILayout.BeginVertical("Box");

			EditorGUILayout.BeginHorizontal();

			if (lightSettingsState)
				GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
			else
				GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));

			var lightSettingsStateRef = lightSettingsState;
			var Scene_EnabledRef = Scene_Enabled;

			if (GUILayout.Button("Lightmap", stateButton, GUILayout.Width(300), GUILayout.Height(17f)))
			{
				lightSettingsState = !lightSettingsState;
			}

			if (lightSettingsStateRef != lightSettingsState)
			{
				if (LB_LightingProfile)
				{
					LB_LightingProfile.lightSettingsState = lightSettingsState;
					LB_LightingProfile.Scene_Enabled = Scene_Enabled;
					EditorUtility.SetDirty(LB_LightingProfile);
				}
			}

			EditorGUILayout.EndHorizontal();

			GUILayout.EndVertical();
			//---------------------------------------------------------------------------------------


			if (lightSettingsState)
			{
				if (helpBox)
					EditorGUILayout.HelpBox("Fully realtime without GI, Enlighten Realtime GI or Baked Progressive Lightmapper", MessageType.Info);

				var lightingModeRef = lightingMode;


				Scene_Enabled = EditorGUILayout.Toggle("Enabled", Scene_Enabled);
				EditorGUILayout.Space();

				// Choose lighting mode (realtime GI or baked GI)
				lightingMode = (LightingMode)EditorGUILayout.EnumPopup("Lighting Mode", lightingMode, GUILayout.Width(343));

				if (lightingMode == LightingMode.BakedProgressiveCPU
					|| lightingMode == LightingMode.BakedProgressiveGPU)
				{
					EditorGUILayout.Space();

					if (helpBox)
						EditorGUILayout.HelpBox("Baked lightmapping resolution. Higher value needs more RAM and longer bake time. Check task manager about RAM usage during bake time", MessageType.Info);

					var bakedResolutionRef = bakedResolution;
					var directSamplesRef = directSamples;
					var indirectSamplesRef = indirectSamples;
					var aoIntensityDirectRef = aoIntensityDirect;
					var aoIntensityIndirectRef = aoIntensityIndirect;
					var aoEnabledRef = aoEnabled;
					var aoDistanceRef = aoDistance;
					var indirectBounceRef = indirectBounce;
					var backfaceToleranceRef = backfaceTolerance;

					// Baked lightmapping resolution   
					bakedResolution = EditorGUILayout.FloatField("Baked Resolution", bakedResolution);
					EditorGUILayout.Space();
					indirectBounce = EditorGUILayout.Slider("Indirect Bounce", indirectBounce, 0, 5);
					EditorGUILayout.Space();
					directSamples = EditorGUILayout.IntSlider("Direct Samples", directSamples, 30, 1000);
					indirectSamples = EditorGUILayout.IntSlider("Indirect Samples", indirectSamples, 50, 10000);
					backfaceTolerance = EditorGUILayout.Slider("Backface Tolerance", backfaceTolerance, 0, 0.9f);
					EditorGUILayout.Space();
					aoEnabled = EditorGUILayout.Toggle("Use AO", aoEnabled);
					if (aoEnabled)
					{
						aoDistance = EditorGUILayout.Slider("AO Distance", aoDistance, 0, 10);
						aoIntensityDirect = EditorGUILayout.Slider("AO Direct", aoIntensityDirect, 0, 5);
						aoIntensityIndirect = EditorGUILayout.Slider("AO Indirect", aoIntensityIndirect, 0, 5);
					}


					EditorGUILayout.Space();

					if (lightingModeRef != lightingMode || bakedResolutionRef != bakedResolution || directSamplesRef != directSamples
						|| indirectSamplesRef != indirectSamples || aoIntensityDirectRef != aoIntensityDirect
						|| aoIntensityIndirectRef != aoIntensityIndirect || aoEnabledRef != aoEnabled
						|| aoDistanceRef != aoDistance || indirectBounceRef != indirectBounce || backfaceToleranceRef != backfaceTolerance)
					{
						helper.Update_LightingMode(Scene_Enabled, lightingMode, indirectBounce, directSamples, indirectSamples, aoEnabled, aoDistance, aoIntensityDirect, aoIntensityIndirect, backfaceTolerance, bakedResolution);

						if (LB_LightingProfile)
						{
							LB_LightingProfile.lightingMode = lightingMode;
							LB_LightingProfile.bakedResolution = bakedResolution;
							LB_LightingProfile.directSamples = directSamples;
							LB_LightingProfile.indirectSamples = indirectSamples;
							LB_LightingProfile.backfaceTolerance = backfaceTolerance;
							LB_LightingProfile.aoIntensityDirect = aoIntensityDirect;
							LB_LightingProfile.aoIntensityIndirect = aoIntensityIndirect;
							LB_LightingProfile.aoEnabled = aoEnabled;
							LB_LightingProfile.aoDistance = aoDistance;
							LB_LightingProfile.indirectBounce = indirectBounce;
							EditorUtility.SetDirty(LB_LightingProfile);
						}
					}
				}

				if (lightingMode == LightingMode.BakedEnlighten)
				{
					EditorGUILayout.Space();

					if (helpBox)
						EditorGUILayout.HelpBox("Baked lightmapping resolution. Higher value needs more RAM and longer bake time. Check task manager about RAM usage during bake time", MessageType.Info);

					var bakedResolutionRef = bakedResolution;
					var aoIntensityDirectRef = aoIntensityDirect;
					var aoIntensityIndirectRef = aoIntensityIndirect;
					var aoEnabledRef = aoEnabled;
					var aoDistanceRef = aoDistance;
					var indirectBounceRef = indirectBounce;
					var backfaceToleranceRef = backfaceTolerance;

					// Baked lightmapping resolution   
					bakedResolution = EditorGUILayout.FloatField("Baked Resolution", bakedResolution);
					EditorGUILayout.Space();
					indirectBounce = EditorGUILayout.Slider("Indirect Bounce", indirectBounce, 0, 5);
					EditorGUILayout.Space();
					backfaceTolerance = EditorGUILayout.Slider("Backface Tolerance", backfaceTolerance, 0, 1);
					EditorGUILayout.Space();
					aoEnabled = EditorGUILayout.Toggle("Use AO", aoEnabled);
					if (aoEnabled)
					{
						aoDistance = EditorGUILayout.Slider("AO Distance", aoDistance, 0, 10);
						aoIntensityDirect = EditorGUILayout.Slider("AO Direct", aoIntensityDirect, 0, 5);
						aoIntensityIndirect = EditorGUILayout.Slider("AO Indirect", aoIntensityIndirect, 0, 5);
					}


					EditorGUILayout.Space();

					if (lightingModeRef != lightingMode || bakedResolutionRef != bakedResolution || aoIntensityDirectRef != aoIntensityDirect
						|| aoIntensityIndirectRef != aoIntensityIndirect || aoEnabledRef != aoEnabled
						|| aoDistanceRef != aoDistance || indirectBounceRef != indirectBounce || backfaceToleranceRef != backfaceTolerance)
					{
						helper.Update_LightingMode(Scene_Enabled, lightingMode, indirectBounce, directSamples, indirectSamples, aoEnabled, aoDistance, aoIntensityDirect, aoIntensityIndirect, backfaceTolerance, bakedResolution);

						if (LB_LightingProfile)
						{
							LB_LightingProfile.lightingMode = lightingMode;
							LB_LightingProfile.bakedResolution = bakedResolution;
							LB_LightingProfile.directSamples = directSamples;
							LB_LightingProfile.indirectSamples = indirectSamples;
							LB_LightingProfile.aoIntensityDirect = aoIntensityDirect;
							LB_LightingProfile.aoIntensityIndirect = aoIntensityIndirect;
							LB_LightingProfile.aoEnabled = aoEnabled;
							LB_LightingProfile.aoDistance = aoDistance;
							LB_LightingProfile.indirectBounce = indirectBounce;
							LB_LightingProfile.backfaceTolerance = backfaceTolerance;
							EditorUtility.SetDirty(LB_LightingProfile);
						}
					}
				}

				if (lightingModeRef != lightingMode || Scene_EnabledRef != Scene_Enabled)
				{
					//----------------------------------------------------------------------
					// Update Lighting Mode
					helper.Update_LightingMode(Scene_Enabled, lightingMode, indirectBounce, directSamples, indirectSamples, aoEnabled, aoDistance, aoIntensityDirect, aoIntensityIndirect, backfaceTolerance, bakedResolution);
					//----------------------------------------------------------------------
					if (LB_LightingProfile)
					{
						LB_LightingProfile.lightingMode = lightingMode;
						LB_LightingProfile.Scene_Enabled = Scene_Enabled;
						EditorUtility.SetDirty(LB_LightingProfile);
					}
				}
				#endregion

				#region Color Space
				EditorGUILayout.Space ();

			if (helpBox)
				EditorGUILayout.HelpBox ("Choose between Linear or Gamma color space , default should be Gamma for maximum performance   ", MessageType.Info);

			var colorSpaceRef = colorSpace;

			// Choose color space (Linear,Gamma) i have used Linear in post effect setting s
			colorSpace = (MyColorSpace)EditorGUILayout.EnumPopup ("Color Space", colorSpace, GUILayout.Width (343));

			if (colorSpaceRef != colorSpace) {
				// Color Space
					helper.Update_ColorSpace (Scene_Enabled,colorSpace);

				if (LB_LightingProfile)
				{
					LB_LightingProfile.colorSpace = colorSpace;
					EditorUtility.SetDirty (LB_LightingProfile);
				}
			}
			#endregion

			#region Render Path
			/*EditorGUILayout.Space ();

			if (helpBox)
				EditorGUILayout.HelpBox ("Currentlly only forward renderig path is supported by URP", MessageType.Info);

			var renderPathRef = renderPath;
				
			renderPath = (Render_Path)EditorGUILayout.EnumPopup ("Render Path", renderPath, GUILayout.Width (343));

			if (renderPathRef != renderPath) {


					helper.Update_RenderPath (Scene_Enabled,renderPath, mainCamera);

				if (LB_LightingProfile)
				{
					LB_LightingProfile.renderPath = renderPath;
					EditorUtility.SetDirty (LB_LightingProfile);
				}

			}
            */
			#endregion

			#region Light Types
			EditorGUILayout.Space ();

			if (helpBox)
				EditorGUILayout.HelpBox ("Changing the type of all light sources (Realtime,Baked,Mixed)", MessageType.Info);

			var lightSettingsRef = lightSettings;

			// Change file lightmapping type mixed,realtime baked
			lightSettings = (LightSettings)EditorGUILayout.EnumPopup ("Lights Type", lightSettings, GUILayout.Width (343));

			//----------------------------------------------------------------------
			// Light Types
			if (lightSettingsRef != lightSettings) {

					helper.Update_LightSettings (Scene_Enabled,lightSettings);

				if (LB_LightingProfile)
				{
					LB_LightingProfile.lightSettings = lightSettings;
					EditorUtility.SetDirty (LB_LightingProfile);
				}
			}
			//----------------------------------------------------------------------
			#endregion

			#region Light Shadows Settings
			EditorGUILayout.Space ();

			if (helpBox)
				EditorGUILayout.HelpBox ("Activate shadows for point and spot lights   ", MessageType.Info);

			var pshadRef = psShadow;
			// Choose hard shadows state on off for spot and point lights
			psShadow = (LightsShadow)EditorGUILayout.EnumPopup ("Shadows", psShadow, GUILayout.Width (343));

			if (pshadRef != psShadow) {

				// Shadows
					helper.Update_Shadows (Scene_Enabled,psShadow);

				//----------------------------------------------------------------------
				if (LB_LightingProfile)
				{
					LB_LightingProfile.lightsShadow = psShadow;
					EditorUtility.SetDirty (LB_LightingProfile);
				}
			}
			#endregion

			#region Light Probes
			EditorGUILayout.Space ();

			if (helpBox)
				EditorGUILayout.HelpBox ("Adjust light probes settings for non-static objects, Blend mode is more optimized", MessageType.Info);

			var lightprobeModeRef = lightprobeMode;

			lightprobeMode = (LightProbeMode)EditorGUILayout.EnumPopup ("Light Probes", lightprobeMode, GUILayout.Width (343));

			if (lightprobeModeRef != lightprobeMode) {

				// Light Probes
				helper.Update_LightProbes (Scene_Enabled, lightprobeMode);

				//----------------------------------------------------------------------
				if (LB_LightingProfile)
				{
					LB_LightingProfile.lightProbesMode = lightprobeMode;
					EditorUtility.SetDirty (LB_LightingProfile);
				}
			}
		}
		#endregion

			#region Buttons
			//-----------Buttons----------------------------------------------------------------------------
			GUILayout.BeginVertical ("Box");

			EditorGUILayout.BeginHorizontal ();

			if(buildState)
				GUILayout.Label(arrowOn,stateButton,GUILayout.Width(20),GUILayout.Height(14));
			else
				GUILayout.Label(arrowOff,stateButton,GUILayout.Width(20),GUILayout.Height(14));
			
			var buildStateRef = buildState;

			if (GUILayout.Button ("Build", stateButton, GUILayout.Width (300), GUILayout.Height (17f))) {
				buildState = !buildState;
			}

			if(buildStateRef != buildState)
			{
				if (LB_LightingProfile)
					LB_LightingProfile.buildState = buildState;
				if (LB_LightingProfile)
					EditorUtility.SetDirty (LB_LightingProfile);
			}

			EditorGUILayout.EndHorizontal ();

			GUILayout.EndVertical ();
			//---------------------------------------------------------------------------------------

			if (buildState) {
				var automodeRef = autoMode;
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                if (helpBox)
					EditorGUILayout.HelpBox ("Automatic lightmap baking", MessageType.Info);


				autoMode = EditorGUILayout.Toggle ("Auto Mode", autoMode);

				if (automodeRef != autoMode) {
					// Auto Mode
					if (autoMode)
						Lightmapping.giWorkflowMode = Lightmapping.GIWorkflowMode.Iterative;
					else
						Lightmapping.giWorkflowMode = Lightmapping.GIWorkflowMode.OnDemand;
					//----------------------------------------------------------------------
					if (LB_LightingProfile)
					{
						LB_LightingProfile.automaticLightmap = autoMode;
						EditorUtility.SetDirty (LB_LightingProfile);
					}
				}
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                // Start bake
                if (!Lightmapping.isRunning) {

					if (helpBox)
						EditorGUILayout.HelpBox ("Bake lightmap", MessageType.Info);

					if (GUILayout.Button ("Bake")) {
						if (!Lightmapping.isRunning) {
							Lightmapping.BakeAsync ();
						}
					}

					if (helpBox)
						EditorGUILayout.HelpBox ("Clear lightmap data", MessageType.Info);

					if (GUILayout.Button ("Clear")) {
						Lightmapping.Clear ();
					}
				} else {

					if (helpBox)
						EditorGUILayout.HelpBox ("Cancel lightmap baking", MessageType.Info);

					if (GUILayout.Button ("Cancel")) {
						if (Lightmapping.isRunning) {
							Lightmapping.Cancel ();
						}
					}
				}

				if (Input.GetKey (KeyCode.F)) {
					if (Lightmapping.isRunning)
						Lightmapping.Cancel ();
				}
				if (Input.GetKey (KeyCode.LeftControl) && Input.GetKey (KeyCode.E)) {
					if (!Lightmapping.isRunning)
						Lightmapping.BakeAsync ();
				}

				if (helpBox) {
					EditorGUILayout.HelpBox ("Bake : Shift + B", MessageType.Info);
					EditorGUILayout.HelpBox ("Cancel : Shift + C", MessageType.Info);
					EditorGUILayout.HelpBox ("Clear : Shift + E", MessageType.Info);

				}
				EditorGUILayout.Space ();
				EditorGUILayout.Space ();
				EditorGUILayout.Space ();
				EditorGUILayout.Space ();

				if (helpBox)
					EditorGUILayout.HelpBox ("Open unity Lighting Settings window", MessageType.Info);

				if (GUILayout.Button ("Lighting Window")) {

					EditorApplication.ExecuteMenuItem ("Window/Rendering/Lighting Settings");
				}

				EditorGUILayout.Space ();
				EditorGUILayout.Space ();

				if (GUILayout.Button ("Add Camera Move Script")) {
						if (!mainCamera.GetComponent<LB_CameraMove> ())
						mainCamera.gameObject.AddComponent<LB_CameraMove> ();
				}

                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                #endregion

            }
		}

			if (winMode == WindowMode.Part2)
			{


                #region Global Fog


            //-----------Global Fog----------------------------------------------------------------------------
            GUILayout.BeginVertical("Box");

            var Fog_EnabledRef = Fog_Enabled;

            EditorGUILayout.BeginHorizontal();

            if (fogState)
                GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
            else
                GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));

            Fog_Enabled = EditorGUILayout.Toggle("", Fog_Enabled, GUILayout.Width(30f), GUILayout.Height(17f));

            var fogStateRef = fogState;

            if (GUILayout.Button("Global Fog", stateButton, GUILayout.Width(300), GUILayout.Height(17f)))
            {
                fogState = !fogState;
            }

            if (fogStateRef != fogState)
            {
                if (LB_LightingProfile)
                {
                    LB_LightingProfile.fogState = fogState;
                    EditorUtility.SetDirty(LB_LightingProfile);
                }
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.EndVertical();
            //---------------------------------------------------------------------------------------

            if (fogState)
            {
                var fogModeRef = fogMode;
                var fogColorRef = fogColor;
                var fogIntensityRef = fogIntensity;
                var linearStartRef = linearStart;
                var linearEndRef = linearEnd;

                fogMode = (FogMode)EditorGUILayout.EnumPopup("Mode", fogMode, GUILayout.Width(343));
                fogColor = EditorGUILayout.ColorField("Color", fogColor);

                if(fogMode != FogMode.Linear)
                   fogIntensity = EditorGUILayout.Slider("Density", fogIntensity, 0, 0.1f);

                if (fogMode == FogMode.Linear)
                {
                    linearStart = EditorGUILayout.Slider("Start", linearStart, 0, 3000);
                    linearEnd = EditorGUILayout.Slider("End", linearEnd, 0, 3000);
                }

                if (fogModeRef != fogMode || fogColorRef != fogColor || fogIntensityRef != fogIntensity
                    || linearStartRef != linearStart || linearEndRef != linearEnd || Fog_EnabledRef != Fog_Enabled)
                {

                    helper.Update_GlobalFog(mainCamera, Fog_Enabled, fogMode, fogColor, fogIntensity, linearStart, linearEnd);

                    if (LB_LightingProfile)
                    {
                        LB_LightingProfile.Fog_Enabled = Fog_Enabled;
                        LB_LightingProfile.fogMode = fogMode;
                        LB_LightingProfile.fogColor = fogColor;
                        LB_LightingProfile.fogIntensity = fogIntensity;
                        LB_LightingProfile.linearStart = linearStart;
                        LB_LightingProfile.linearEnd = linearEnd;
                    }
                }
            }
            #endregion

				#region Depth of Field    
                
			//-----------Depth of Field----------------------------------------------------------------------------
			GUILayout.BeginVertical ("Box");

			var DOF_EnabledRef = DOF_Enabled;

			EditorGUILayout.BeginHorizontal ();

			if(dofState)
				GUILayout.Label(arrowOn,stateButton,GUILayout.Width(20),GUILayout.Height(14));
			else
				GUILayout.Label(arrowOff,stateButton,GUILayout.Width(20),GUILayout.Height(14));
			
			DOF_Enabled = EditorGUILayout.Toggle("",DOF_Enabled ,GUILayout.Width(30f),GUILayout.Height(17f));

			var dofStateRef = dofState;

			if (GUILayout.Button ("Depth of Field", stateButton, GUILayout.Width (300), GUILayout.Height (17f))) {
				dofState = !dofState;
			}

			if(dofStateRef != dofState)
			{
				if (LB_LightingProfile)
					LB_LightingProfile.dofState = dofState;
				if (LB_LightingProfile)					
					EditorUtility.SetDirty (LB_LightingProfile);
			}

			EditorGUILayout.EndHorizontal ();

			GUILayout.EndVertical ();
			//---------------------------------------------------------------------------------------

			if(dofState)
			{
				if (helpBox)
					EditorGUILayout.HelpBox ("Activate Depth Of Field for the camera", MessageType.Info);
			}

                
                var dofDistanceRef = dofDistance;
                var dofFocusLengthRef = dofFocusLength;
                var dofApertureRef = dofAperture;


			if(dofState)
			{
                dofDistance = (float)EditorGUILayout.Slider ("Focus Deistance", dofDistance, 1, 100);
                dofFocusLength = (float)EditorGUILayout.Slider ("Focus Length", dofFocusLength, 1, 300);
                dofAperture = (float)EditorGUILayout.Slider ("Apertur", dofAperture, 1, 32);
            }


           
                if (DOF_EnabledRef != DOF_Enabled || dofDistanceRef != dofDistance || dofFocusLengthRef != dofFocusLength || dofApertureRef != dofAperture)
                {

                    helper.Update_DOF(mainCamera, DOF_Enabled, dofDistance, dofFocusLength, dofAperture);

                    //----------------------------------------------------------------------
                    if (LB_LightingProfile)
                    {
                        LB_LightingProfile.DOF_Enabled = DOF_Enabled;
                        LB_LightingProfile.dofDistance = dofDistance;
                        LB_LightingProfile.dofFocusLength = dofFocusLength;
                        LB_LightingProfile.dofAperture = dofAperture;
                        EditorUtility.SetDirty(LB_LightingProfile);
                    }
                }
           
				#endregion
			
				#region Bloom

				//-----------Bloom----------------------------------------------------------------------------
				GUILayout.BeginVertical ("Box");

				var Bloom_EnabledRef = Bloom_Enabled;

			EditorGUILayout.BeginHorizontal ();

			if(bloomState)
				GUILayout.Label(arrowOn,stateButton,GUILayout.Width(20),GUILayout.Height(14));
			else
				GUILayout.Label(arrowOff,stateButton,GUILayout.Width(20),GUILayout.Height(14));
			
				Bloom_Enabled = EditorGUILayout.Toggle("",Bloom_Enabled ,GUILayout.Width(30f),GUILayout.Height(17f));

			var bloomStateRef = bloomState;

				if (GUILayout.Button ("Bloom", stateButton, GUILayout.Width (300), GUILayout.Height (17f))) {
					bloomState = !bloomState;
				}

			if(bloomStateRef != bloomState)
			{
				if (LB_LightingProfile)
					LB_LightingProfile.bloomState = bloomState;			
				if (LB_LightingProfile)
					EditorUtility.SetDirty (LB_LightingProfile);
			}

				EditorGUILayout.EndHorizontal ();

				GUILayout.EndVertical ();
				//---------------------------------------------------------------------------------------
				if(bloomState)
				{
					if (helpBox)
						EditorGUILayout.HelpBox ("Activate Bloom for the camera", MessageType.Info);
				}
				var bIntensityRef = bIntensity;
				var bThreshouldRef = bThreshould;
				var bColorRef = bColor;
			var dirtTextureRef = dirtTexture;
			var dirtIntensityRef = dirtIntensity;
			var mobileOptimizedBloomRef = mobileOptimizedBloom;
			var bRotationRef = bRotation;
				if (bloomState)
				{
					bIntensity = (float)EditorGUILayout.Slider ("Intensity", bIntensity, 0, 3f);
					bThreshould = (float)EditorGUILayout.Slider ("Threshould", bThreshould, 0, 2f);
				    bRotation = (float)EditorGUILayout.Slider ("Rotation", bRotation, -1, 0.7f);

					bColor = (Color)EditorGUILayout.ColorField ("Color", bColor);
					mobileOptimizedBloom = EditorGUILayout.Toggle("High Quality Filtering",mobileOptimizedBloom);
					EditorGUILayout.Space();

				dirtTexture = EditorGUILayout.ObjectField ("Dirt Texture", dirtTexture, typeof(Texture2D), true) as Texture2D;
				dirtIntensity = (float)EditorGUILayout.Slider ("Dirt Intensity", dirtIntensity, 0, 10f);
				}

			if (dirtTextureRef != dirtTexture || dirtIntensityRef != dirtIntensity || Bloom_EnabledRef != Bloom_Enabled || bIntensityRef != bIntensity || bColorRef != bColor || bThreshouldRef != bThreshould || bIntensityRef != bIntensity
				|| mobileOptimizedBloomRef != mobileOptimizedBloom  || bRotationRef != bRotation) {


				helper.Update_Bloom(Bloom_Enabled,bIntensity,bThreshould,bColor,dirtTexture,dirtIntensity,mobileOptimizedBloom,bRotation);


					//----------------------------------------------------------------------

				if (LB_LightingProfile)
				{
						LB_LightingProfile.Bloom_Enabled = Bloom_Enabled;
					LB_LightingProfile.bIntensity = bIntensity;
					LB_LightingProfile.bRotation = bRotation;
					LB_LightingProfile.bThreshould = bThreshould;
						LB_LightingProfile.mobileOptimizedBloom = mobileOptimizedBloom;						
						LB_LightingProfile.bColor = bColor;		
						LB_LightingProfile.dirtTexture = dirtTexture;		
						LB_LightingProfile.dirtIntensity = dirtIntensity;	
						EditorUtility.SetDirty (LB_LightingProfile);
				}
				}

				#endregion

			}

			if (winMode == WindowMode.Part3) {

				#region Color Grading

			//-----------Color Grading----------------------------------------------------------------------------
			GUILayout.BeginVertical ("Box");

			EditorGUILayout.BeginHorizontal ();

			if(colorState)
				GUILayout.Label(arrowOn,stateButton,GUILayout.Width(20),GUILayout.Height(14));
			else
				GUILayout.Label(arrowOff,stateButton,GUILayout.Width(20),GUILayout.Height(14));
			

			var colorStateRef = colorState;

			if (GUILayout.Button ("Color Grading", stateButton, GUILayout.Width (300), GUILayout.Height (17f))) {
				colorState = !colorState;
			}

			if(colorStateRef != colorState)
			{
				if (LB_LightingProfile)
					LB_LightingProfile.colorState = colorState;				
				if (LB_LightingProfile)
					EditorUtility.SetDirty (LB_LightingProfile);
			}

			EditorGUILayout.EndHorizontal ();

			GUILayout.EndVertical ();
			//---------------------------------------------------------------------------------------

			if(colorState)
			{
				if (helpBox)
					EditorGUILayout.HelpBox ("Color grading settings", MessageType.Info);
			}
				var colorModeRef = colorMode;
				var exposureIntensityRef = exposureIntensity;
				var contrastValueRef = contrastValue;
				var tempRef = temp;
				var gammaRef = gamma;
				var colorGammaRef = colorGamma;
				var colorLiftRef = colorLift;
				var saturationRef = saturation;
				var lutRef = lut;

			if(colorState)
			{
				if (!webGL_Mobile)
					colorMode = (ColorMode)EditorGUILayout.EnumPopup ("Mode", colorMode, GUILayout.Width (343));
				
				if(colorMode == ColorMode.LUT)
				{
					lut = EditorGUILayout.ObjectField ("LUT Texture   ", lut, typeof(Texture), true) as Texture;


				}
				else
				{
					exposureIntensity = (float)EditorGUILayout.Slider ("Exposure", exposureIntensity, 0, 3f);
					contrastValue = (float)EditorGUILayout.Slider ("Contrast", contrastValue, 0, 1f);
					saturation = (float)EditorGUILayout.Slider ("Saturation", saturation, -1f, 0.3f);
					temp = (float)EditorGUILayout.Slider ("Temperature", temp, 0, 100f);

					EditorGUILayout.Space();

					colorGamma = (Color)EditorGUILayout.ColorField ("Gamma Color", colorGamma);
					colorLift = (Color)EditorGUILayout.ColorField ("Lift Color", colorLift);
					EditorGUILayout.Space();

					gamma = (float)EditorGUILayout.Slider ("Gamma", gamma, -1f, 1f);
				}
			}

				if (exposureIntensityRef != exposureIntensity || contrastValueRef != contrastValue || tempRef != temp 
				  || colorModeRef != colorMode || gammaRef != gamma || colorGammaRef != colorGamma || colorLiftRef != colorLift || saturationRef != saturation
				|| lutRef != lut) {


				helper.Update_ColorGrading (colorMode, exposureIntensity, contrastValue, temp, saturation, colorGamma, colorLift, gamma,lut);

					//----------------------------------------------------------------------
				if (LB_LightingProfile)
				{
						LB_LightingProfile.exposureIntensity = exposureIntensity;
						LB_LightingProfile.lut = lut;
						LB_LightingProfile.contrastValue = contrastValue;
						LB_LightingProfile.temp = temp;
						LB_LightingProfile.colorMode = colorMode;
						LB_LightingProfile.colorLift = colorLift;
						LB_LightingProfile.colorGamma = colorGamma;
						LB_LightingProfile.gamma = gamma;
						LB_LightingProfile.saturation = saturation;
						EditorUtility.SetDirty (LB_LightingProfile);
				}
			}

				#endregion
			
			}

		if (winMode == WindowMode.Finish)
		{

			#region Anti Aliasing

			//-----------Anti Aliasing----------------------------------------------------------------------------
			GUILayout.BeginVertical("Box");

			var AA_EnabledRef = AA_Enabled;

			EditorGUILayout.BeginHorizontal();

			if (aaState)
				GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
			else
				GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));

			AA_Enabled = EditorGUILayout.Toggle("", AA_Enabled, GUILayout.Width(30f), GUILayout.Height(17f));

			var aaStateRef = aaState;

			if (GUILayout.Button("Anti Aliasing", stateButton, GUILayout.Width(300), GUILayout.Height(17f)))
			{
				aaState = !aaState;
			}

			if (aaStateRef != aaState)
			{
				if (LB_LightingProfile)
					LB_LightingProfile.aaState = aaState;
				if (LB_LightingProfile)
					EditorUtility.SetDirty(LB_LightingProfile);
			}

			EditorGUILayout.EndHorizontal();

			GUILayout.EndVertical();
			//---------------------------------------------------------------------------------------

			var aaModeRef = aaMode;

			if (aaState)
			{
				if (helpBox)
					EditorGUILayout.HelpBox("Activate Antialiasing for the camera", MessageType.Info);

				aaMode = (AAMode)EditorGUILayout.EnumPopup("Anti Aliasing", aaMode, GUILayout.Width(343));
			}
			if (aaModeRef != aaMode || AA_EnabledRef != AA_Enabled)
			{

				helper.Update_AA(mainCamera, aaMode, AA_Enabled);

				//----------------------------------------------------------------------
				if (LB_LightingProfile)
				{
					LB_LightingProfile.aaMode = aaMode;
					LB_LightingProfile.AA_Enabled = AA_Enabled;
				}

				if (LB_LightingProfile)
					EditorUtility.SetDirty(LB_LightingProfile);


			}

			#endregion

			#region AO
			/*
			//-----------Ambient Occlusion----------------------------------------------------------------------------
			GUILayout.BeginVertical ("Box");

			var AO_EnabledRef = AO_Enabled;

			EditorGUILayout.BeginHorizontal ();

			if(aoState)
				GUILayout.Label(arrowOn,stateButton,GUILayout.Width(20),GUILayout.Height(14));
			else
				GUILayout.Label(arrowOff,stateButton,GUILayout.Width(20),GUILayout.Height(14));

			AO_Enabled = EditorGUILayout.Toggle("",AO_Enabled ,GUILayout.Width(30f),GUILayout.Height(17f));

			var aoStateRef = aoState;

			if (GUILayout.Button ("Ambient Occlusion", stateButton, GUILayout.Width (300), GUILayout.Height (17f))) {
				aoState = !aoState;
			}

			if(aoStateRef != aoState)
			{
				if (LB_LightingProfile)
					LB_LightingProfile.aoState = aoState;
				if (LB_LightingProfile)
					EditorUtility.SetDirty (LB_LightingProfile);
			}

			EditorGUILayout.EndHorizontal ();

			GUILayout.EndVertical ();
			//---------------------------------------------------------------------------------------

			if(aoState)
			{
				if (helpBox)
					EditorGUILayout.HelpBox ("Activate AO for the camera", MessageType.Info);
			}
				var aoIntensityRef = aoIntensity;
				var ambientOnlyRef = ambientOnly;
				var aoTypeRef = aoType;
				var aoRadiusRef = aoRadius;
				var aoColorRef = aoColor;
				var aoQualityRef = aoQuality;




				if(aoState)
				{
					aoType = (AOType)EditorGUILayout.EnumPopup ("Type", aoType, GUILayout.Width (343));
				}
					if (aoType == AOType.Modern)
				{
					if(aoState)
					{
						aoIntensity = (float)EditorGUILayout.Slider ("Intensity", aoIntensity, 0, 2f);
						aoColor	= (Color)EditorGUILayout.ColorField ("Color", aoColor);
						ambientOnly = (bool)EditorGUILayout.Toggle ("Ambient Only", ambientOnly);
					}
					}
					if (aoType == AOType.Classic) {
					if(aoState)
					{
						aoRadius = (float)EditorGUILayout.Slider ("Radius", aoRadius, 0, 4.3f);
						aoIntensity = (float)EditorGUILayout.Slider ("Intensity", aoIntensity, 0, 4f);
						aoColor	= (Color)EditorGUILayout.ColorField ("Color", aoColor);
						aoQuality = (AmbientOcclusionQuality)EditorGUILayout.EnumPopup ("Quality", aoQuality, GUILayout.Width (343));
						ambientOnly = (bool)EditorGUILayout.Toggle ("Ambient Only", ambientOnly);
					}
					}


				if (AO_EnabledRef != AO_Enabled || aoIntensityRef != aoIntensity || ambientOnlyRef != ambientOnly
				  || aoTypeRef != aoType || aoRadiusRef != aoRadius || aoColorRef != aoColor || aoQualityRef != aoQuality) {

					if (AO_Enabled)
						helper.Update_AO (mainCamera, true, aoType, aoRadius, aoIntensity, ambientOnly, aoColor, aoQuality);
					if (!AO_Enabled)
						helper.Update_AO (mainCamera, false, aoType, aoRadius, aoIntensity, ambientOnly, aoColor, aoQuality);

					//----------------------------------------------------------------------
				if (LB_LightingProfile)
				{
						LB_LightingProfile.AO_Enabled = AO_Enabled;
						LB_LightingProfile.aoIntensity = aoIntensity;
						LB_LightingProfile.ambientOnly = ambientOnly;
						LB_LightingProfile.aoColor = aoColor;
						LB_LightingProfile.aoRadius = aoRadius;
						LB_LightingProfile.aoType = aoType;
						LB_LightingProfile.aoQuality = aoQuality;
						EditorUtility.SetDirty (LB_LightingProfile);
				}
			}
							*/
			#endregion

			#region Vignette


			//-----------Vignette----------------------------------------------------------------------------
			GUILayout.BeginVertical("Box");

			var Vignette_EnabledRef = Vignette_Enabled;

			EditorGUILayout.BeginHorizontal();

			if (vignetteState)
				GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
			else
				GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));

			Vignette_Enabled = EditorGUILayout.Toggle("", Vignette_Enabled, GUILayout.Width(30f), GUILayout.Height(17f));

			var vignetteStateRef = vignetteState;

			if (GUILayout.Button("Vignette", stateButton, GUILayout.Width(300), GUILayout.Height(17f)))
			{
				vignetteState = !vignetteState;
			}

			if (vignetteStateRef != vignetteState)
			{
				if (LB_LightingProfile)
					LB_LightingProfile.vignetteState = vignetteState;
				if (LB_LightingProfile)
					EditorUtility.SetDirty(LB_LightingProfile);
			}

			EditorGUILayout.EndHorizontal();

			GUILayout.EndVertical();
			//---------------------------------------------------------------------------------------

			if (vignetteState)
			{
				if (helpBox)
					EditorGUILayout.HelpBox("Activate Vignette effect for your camera", MessageType.Info);
			}

			var vignetteIntensityRef = vignetteIntensity;

			if (vignetteState)
				vignetteIntensity = EditorGUILayout.Slider("Intensity", vignetteIntensity, 0, 0.3f);


			if (Vignette_EnabledRef != Vignette_Enabled || vignetteIntensityRef != vignetteIntensity)
			{
				helper.Update_Vignette(Vignette_Enabled, vignetteIntensity);
			}

			if (LB_LightingProfile)
			{
				LB_LightingProfile.Vignette_Enabled = Vignette_Enabled;
				LB_LightingProfile.vignetteIntensity = vignetteIntensity;
				EditorUtility.SetDirty(LB_LightingProfile);
			}

			#endregion

			#region Motion Blur


			//-----------Motion Blur----------------------------------------------------------------------------
			GUILayout.BeginVertical("Box");

			var MotionBlur_EnabledRef = MotionBlur_Enabled;

			EditorGUILayout.BeginHorizontal();

			if (motionBlurState)
				GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
			else
				GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));

			MotionBlur_Enabled = EditorGUILayout.Toggle("", MotionBlur_Enabled, GUILayout.Width(30f), GUILayout.Height(17f));

			var motionBlurStateRef = motionBlurState;

			if (GUILayout.Button("Motion Blur", stateButton, GUILayout.Width(300), GUILayout.Height(17f)))
			{
				motionBlurState = !motionBlurState;
			}

			if (motionBlurStateRef != motionBlurState)
			{
				if (LB_LightingProfile)
					LB_LightingProfile.motionBlurState = motionBlurState;
				if (LB_LightingProfile)
					EditorUtility.SetDirty(LB_LightingProfile);
			}

			EditorGUILayout.EndHorizontal();

			GUILayout.EndVertical();
			//---------------------------------------------------------------------------------------

			if (motionBlurState)
			{
				if (helpBox)
					EditorGUILayout.HelpBox("Activate Motion Blur effect for your camera", MessageType.Info);


				var mbIntensityRef = mbIntensity;
				var mbQualityRef = mbQuality;

				mbQuality = (MotionBlurQuality)EditorGUILayout.EnumPopup("Qality", mbQuality, GUILayout.Width(343));
				mbIntensity = (float)EditorGUILayout.Slider("Intesnity", mbIntensity, 0, 10f);

				if (MotionBlur_EnabledRef != MotionBlur_Enabled || mbIntensityRef != mbIntensity
					|| mbQualityRef != mbQuality)
				{
					helper.Update_MotionBlur(MotionBlur_Enabled, mbIntensity, mbQuality);
				}

				if (LB_LightingProfile)
				{
					LB_LightingProfile.mbQuality = mbQuality;
					LB_LightingProfile.mbIntensity = mbIntensity;
					LB_LightingProfile.MotionBlur_Enabled = MotionBlur_Enabled;

				}
				if (LB_LightingProfile)
					EditorUtility.SetDirty(LB_LightingProfile);
			}
				#endregion

				#region Chromattic Aberration


				//-----------Chromattic Aberration----------------------------------------------------------------------------
				GUILayout.BeginVertical ("Box");

				var Chromattic_EnabledRef = Chromattic_Enabled;

			EditorGUILayout.BeginHorizontal ();

			if(chromatticState)
				GUILayout.Label(arrowOn,stateButton,GUILayout.Width(20),GUILayout.Height(14));
			else
				GUILayout.Label(arrowOff,stateButton,GUILayout.Width(20),GUILayout.Height(14));
			
				Chromattic_Enabled = EditorGUILayout.Toggle("",Chromattic_Enabled ,GUILayout.Width(30f),GUILayout.Height(17f));

			var chromatticStateRef = chromatticState;

			if (GUILayout.Button ("Chromattic Aberration", stateButton, GUILayout.Width (300), GUILayout.Height (17f))) {
					chromatticState = !chromatticState;
				}

			if(chromatticStateRef != chromatticState)
			{
				if (LB_LightingProfile)
					LB_LightingProfile.chromatticState = chromatticState;
				if (LB_LightingProfile)
					EditorUtility.SetDirty (LB_LightingProfile);
			}

				EditorGUILayout.EndHorizontal ();

				GUILayout.EndVertical ();
				//---------------------------------------------------------------------------------------

				if(chromatticState)
				{
					if (helpBox)
						EditorGUILayout.HelpBox ("Activate Chromattic Aberration effect for your camera", MessageType.Info);				
				}

				var CA_IntensityRef = CA_Intensity;

			if(chromatticState)
			{
				CA_Intensity = EditorGUILayout.Slider("Intensity", CA_Intensity, 0,1f ) ;
			}

				if(Chromattic_EnabledRef != Chromattic_Enabled || CA_IntensityRef != CA_Intensity )
				{
				helper.Update_ChromaticAberration(Chromattic_Enabled,CA_Intensity);
				}

			if (LB_LightingProfile)
			{
				LB_LightingProfile.Chromattic_Enabled = Chromattic_Enabled;
				LB_LightingProfile.CA_Intensity = CA_Intensity;
				EditorUtility.SetDirty (LB_LightingProfile);
			}

				#endregion

				
				#region Check for updates
            /*
				if (GUILayout.Button ("Check for updates")) {
				
					EditorApplication.ExecuteMenuItem ("Assets/Lighting Box Updates");
				}
                */
				EditorGUILayout.Space ();
				EditorGUILayout.Space ();
				EditorGUILayout.Space ();
				EditorGUILayout.Space ();

				#endregion
			
			}

			EditorGUILayout.EndScrollView ();
		}


		#region Update Settings
		void UpdateSettings()
		{
		
			// Sun Light Update
			if (sunLight) {
				sunLight.color = sunColor;
				sunLight.intensity = sunIntensity;
				sunLight.bounceIntensity = indirectIntensity;
			} else {
				Update_Sun ();
			}

			if (sunFlare)
			{
				if(sunLight)
					sunLight.flare = sunFlare;
			}
			else
			{
				if(sunLight)
					sunLight.flare = null;
			}

			// Skybox
		helper.Update_SkyBox (Ambient_Enabled,skyBox);

		// Update Lighting Mode
		helper.Update_LightingMode(Scene_Enabled, lightingMode, indirectBounce, directSamples, indirectSamples, aoEnabled, aoDistance, aoIntensityDirect, aoIntensityIndirect, backfaceTolerance, bakedResolution);

		// Update Ambient
		helper.Update_Ambient (Ambient_Enabled,ambientLight, ambientColor,skyColor,equatorColor,groundColor);

			// Lights settings
		helper.Update_LightSettings (Scene_Enabled,lightSettings);

			// Color Space
		helper.Update_ColorSpace(Scene_Enabled,colorSpace);

			// Render Path
	//	helper.Update_RenderPath (Scene_Enabled,renderPath, mainCamera);

			// Shadows
		helper.Update_Shadows(Scene_Enabled,psShadow);

			// Light Probes
		helper.Update_LightProbes(Scene_Enabled,lightprobeMode);

			// Auto Mode
			helper.Update_AutoMode(autoMode);

        // Global Fog
        //helper.Update_GlobalFog(mainCamera,Fog_Enabled,vFog,fDistance,fHeight,fheightDensity,fColor,fogIntensity);
        helper.Update_GlobalFog(mainCamera, Fog_Enabled, fogMode, fogColor, fogIntensity, linearStart, linearEnd);
        if(mainCamera && mainCamera.GetComponent<UniversalAdditionalCameraData>())
           mainCamera.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = true;
    }
    #endregion

    #region On Load
    // load saved data based on project and scene name
    void OnLoad()
		{

		if (!mainCamera) {
			if (GameObject.Find (LB_LightingProfile.mainCameraName))
				mainCamera = GameObject.Find (LB_LightingProfile.mainCameraName).GetComponent<Camera> ();
			else
				mainCamera = GameObject.FindObjectOfType<Camera> ();
		}
		
		if (!GameObject.Find ("LightingBox_Helper")){
			GameObject helperObject = new GameObject ("LightingBox_Helper");
			helperObject.AddComponent<LB_LightingBoxHelper> ();
			helper = helperObject.GetComponent<LB_LightingBoxHelper> ();
		}


		if (LB_LightingProfile) {

			lightingMode = LB_LightingProfile.lightingMode;
			if (LB_LightingProfile.skyBox)
				skyBox = LB_LightingProfile.skyBox;
			else
				skyBox = RenderSettings.skybox;
			sunFlare = LB_LightingProfile.sunFlare;
			ambientLight = LB_LightingProfile.ambientLight;
			//renderPath = LB_LightingProfile.renderPath;
			lightSettings = LB_LightingProfile.lightSettings;
			sunColor = LB_LightingProfile.sunColor;

			// Color Space
			colorSpace = LB_LightingProfile.colorSpace;

			lightprobeMode = LB_LightingProfile.lightProbesMode;

			// Shadows
			psShadow = LB_LightingProfile.lightsShadow;

                // DOF 
                dofDistance = LB_LightingProfile.dofDistance;
                dofFocusLength = LB_LightingProfile.dofFocusLength;
                dofAperture = LB_LightingProfile.dofAperture;

                // AA
                aaMode = LB_LightingProfile.aaMode;

            // Fog
            fogMode = LB_LightingProfile.fogMode;
            fogColor = LB_LightingProfile.fogColor;
            fogIntensity = LB_LightingProfile.fogIntensity;
            linearStart = LB_LightingProfile.linearStart;
            linearEnd = LB_LightingProfile.linearEnd;

            // AO
            /*aoIntensity = LB_LightingProfile.aoIntensity;
			aoColor = LB_LightingProfile.aoColor;
			ambientOnly = LB_LightingProfile.ambientOnly;
			aoRadius = LB_LightingProfile.aoRadius;
			aoType = LB_LightingProfile.aoType;
			aoQuality = LB_LightingProfile.aoQuality;*/

            // Bloom
            bIntensity = LB_LightingProfile.bIntensity;
			bColor = LB_LightingProfile.bColor;
			bThreshould = LB_LightingProfile.bThreshould;
			dirtTexture = LB_LightingProfile.dirtTexture;
			dirtIntensity = LB_LightingProfile.dirtIntensity;
			mobileOptimizedBloom = LB_LightingProfile.mobileOptimizedBloom;
			bRotation = LB_LightingProfile.bRotation;

			// Color Grading
			exposureIntensity = LB_LightingProfile.exposureIntensity;
			contrastValue = LB_LightingProfile.contrastValue;
			temp = LB_LightingProfile.temp;
			colorMode = LB_LightingProfile.colorMode;
			colorGamma = LB_LightingProfile.colorGamma;
			colorLift = LB_LightingProfile.colorLift;
			gamma = LB_LightingProfile.gamma;
			saturation = LB_LightingProfile.saturation;
			lut = LB_LightingProfile.lut;

			// Effects
			Vignette_Enabled = LB_LightingProfile.Vignette_Enabled;
			vignetteIntensity = LB_LightingProfile.vignetteIntensity;
			Chromattic_Enabled = LB_LightingProfile.Chromattic_Enabled;
			CA_Intensity = LB_LightingProfile.CA_Intensity;

			// Motion Blur
			MotionBlur_Enabled = LB_LightingProfile.MotionBlur_Enabled;
            mbIntensity = LB_LightingProfile.mbIntensity;
            mbQuality = LB_LightingProfile.mbQuality;

            // Lightmap
            bakedResolution = LB_LightingProfile.bakedResolution;
			sunIntensity = LB_LightingProfile.sunIntensity;
			indirectIntensity = LB_LightingProfile.indirectIntensity;

			// Lightmapping
			directSamples = LB_LightingProfile.directSamples;
			indirectSamples = LB_LightingProfile.indirectSamples;
			aoIntensityDirect = LB_LightingProfile.aoIntensityDirect;
			aoIntensityIndirect = LB_LightingProfile.aoIntensityIndirect;
			aoEnabled = LB_LightingProfile.aoEnabled;
			aoDistance = LB_LightingProfile.aoDistance;
			indirectBounce = LB_LightingProfile.indirectBounce;
			backfaceTolerance = LB_LightingProfile.backfaceTolerance;


			ambientColor = LB_LightingProfile.ambientColor;
			ambientLight = LB_LightingProfile.ambientLight;
			skyBox = LB_LightingProfile.skyBox;
			skyColor = LB_LightingProfile.skyColor;
			equatorColor = LB_LightingProfile.equatorColor;
			groundColor = LB_LightingProfile.groundColor;

			// Auto lightmap
			autoMode = LB_LightingProfile.automaticLightmap;

			Ambient_Enabled = LB_LightingProfile.Ambient_Enabled;
			Scene_Enabled = LB_LightingProfile.Scene_Enabled;
			Sun_Enabled = LB_LightingProfile.Sun_Enabled;
			VL_Enabled = LB_LightingProfile.VL_Enabled;
			Fog_Enabled = LB_LightingProfile.Fog_Enabled;
			DOF_Enabled = LB_LightingProfile.DOF_Enabled;
			Bloom_Enabled = LB_LightingProfile.Bloom_Enabled;
			AA_Enabled = LB_LightingProfile.AA_Enabled;
			AO_Enabled = LB_LightingProfile.AO_Enabled;

			buildState = LB_LightingProfile.buildState;
			profileState = LB_LightingProfile.profileState;
			cameraState = LB_LightingProfile.cameraState;
			lightSettingsState = LB_LightingProfile.lightSettingsState;
			sunState = LB_LightingProfile.sunState;
			ambientState = LB_LightingProfile.ambientState;


			chromatticState = LB_LightingProfile.chromatticState;
			vignetteState = LB_LightingProfile.vignetteState;
			motionBlurState = LB_LightingProfile.motionBlurState;
			aoState = LB_LightingProfile.aoState;
			aaState = LB_LightingProfile.aaState;
			bloomState = LB_LightingProfile.bloomState;
			colorState = LB_LightingProfile.colorState;
			dofState = LB_LightingProfile.dofState;
			fogState = LB_LightingProfile.fogState;
			OptionsState = LB_LightingProfile.OptionsState;
			LightingBoxState = LB_LightingProfile.LightingBoxState;

				//mainCamera.allowHDR = true;
				//mainCamera.allowMSAA = false;
			
			if (LB_LightingProfile.volumeProfile)
                volumeProfileMain = LB_LightingProfile.volumeProfile;
		}

			UpdatePostEffects ();

			UpdateSettings ();

			Update_Sun();

	}
		#endregion

		#region Update Post Effects Settings

		public void UpdatePostEffects()
		{

			if(!helper)
				helper = GameObject.Find("LightingBox_Helper").GetComponent<LB_LightingBoxHelper> ();

			if (!volumeProfileMain)
				return;

        helper.UpdateProfiles(mainCamera,volumeProfileMain);


        // MotionBlur
        if (MotionBlur_Enabled)
				helper.Update_MotionBlur (true, mbIntensity, mbQuality);
			else
				helper.Update_MotionBlur (false, mbIntensity, mbQuality);

			// Vignette
			helper.Update_Vignette (Vignette_Enabled,vignetteIntensity);


		// _ChromaticAberration
		helper.Update_ChromaticAberration(Chromattic_Enabled,CA_Intensity);

		helper.Update_Bloom(Bloom_Enabled,bIntensity,bThreshould,bColor,dirtTexture,dirtIntensity,mobileOptimizedBloom,bRotation);



            // Depth of Field
            helper.Update_DOF(mainCamera, DOF_Enabled, dofDistance, dofFocusLength, dofAperture);

            // AO
            /*if (AO_Enabled)
                    helper.Update_AO(mainCamera,true,aoType,aoRadius,aoIntensity,ambientOnly,aoColor, aoQuality);
                else
                    helper.Update_AO(mainCamera,false,aoType,aoRadius,aoIntensity,ambientOnly,aoColor, aoQuality);
    */

            // Color Grading
            helper.Update_ColorGrading(colorMode,exposureIntensity,contrastValue,temp,saturation,colorGamma,colorLift,gamma,lut);

	}
		#endregion

		#region Scene Delegate

		string currentScene;    
		void SceneChanging ()
	{
		if (currentScene != EditorSceneManager.GetActiveScene ().name) {
			if (System.String.IsNullOrEmpty (EditorPrefs.GetString (EditorSceneManager.GetActiveScene ().name)))
				LB_LightingProfile = Resources.Load ("DefaultSettings")as LB_LightingProfile;
			else
				LB_LightingProfile = (LB_LightingProfile)AssetDatabase.LoadAssetAtPath (EditorPrefs.GetString (EditorSceneManager.GetActiveScene ().name), typeof(LB_LightingProfile));

            helper.Update_MainProfile(LB_LightingProfile, volumeProfileMain);

            OnLoad();
			currentScene = EditorSceneManager.GetActiveScene ().name;
		}

	}
		#endregion

		#region Sun Light
		public void Update_Sun()
		{
		if (Sun_Enabled) {
			if (!RenderSettings.sun) {
				Light[] lights = GameObject.FindObjectsOfType<Light> ();
				foreach (Light l in lights) {
					if (l.type == LightType.Directional) {
						sunLight = l;

						if (sunColor != Color.clear)
							sunColor = l.color;
						else
							sunColor = Color.white;

						//sunLight.shadowNormalBias = 0.05f;  
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

				//	sunLight.shadowNormalBias = 0.05f;  
				sunLight.color = sunColor;
				if (sunLight.bounceIntensity == 1f)
					sunLight.bounceIntensity = indirectIntensity;
			}
		}
	}

		#endregion

		#region On Download Completed
		void DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
	{
		if (e.Error != null)
			Debug.Log (e.Error);
	}
		#endregion
}
#endif