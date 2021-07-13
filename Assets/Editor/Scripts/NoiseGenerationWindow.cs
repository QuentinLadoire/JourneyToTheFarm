using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

enum DisplayMode
{
	SimpleNoise,
	FractalNoise,
	TurbulenceNoise,
	MarbleNoise,
	WoodNoise
}

public class NoiseGenerationWindow : EditorWindow
{
	readonly float width = 1200.0f;
	readonly float height = 700.0f;

	Rect windowSettingRect = Rect.zero;
	Rect areaTextureRect = Rect.zero;
	Rect noiseSettingRect = Rect.zero;

	DisplayMode displayMode = DisplayMode.SimpleNoise;
	int textureResolution = 256;
	TextureFormat textureFormat = TextureFormat.RGBAHalf;
	 int contrast = 3;

	Texture noiseTexture = null;
	Rect textureRect = Rect.zero;

	Vector2 noiseInput = Vector2.zero;
	readonly SimpleNoiseSetting noiseSetting = new SimpleNoiseSetting();
	readonly FractalNoiseSetting fractalNoiseSetting = new FractalNoiseSetting();
	readonly FractalNoiseSetting turbulenceNoiseSetting = new FractalNoiseSetting();
	readonly FractalNoiseSetting marbleNoiseSetting = new FractalNoiseSetting();
	readonly FractalNoiseSetting woodNoiseSetting = new FractalNoiseSetting();

	bool autoGenerate = true;
	bool modified = false;

	void InitRect()
	{
		windowSettingRect.x = 0.0f;
		windowSettingRect.y = 0.0f;
		windowSettingRect.width = width * 0.25f;
		windowSettingRect.height = height;

		areaTextureRect.x = windowSettingRect.xMax;
		areaTextureRect.y = 0.0f;
		areaTextureRect.width = width * 0.50f;
		areaTextureRect.height = height;

		noiseSettingRect.x = areaTextureRect.xMax;
		noiseSettingRect.y = 0.0f;
		noiseSettingRect.width = width * 0.25f;
		noiseSettingRect.height = height;
	}
	void InitTexture()
	{
		noiseTexture = Texture2D.whiteTexture;

		textureRect.width = areaTextureRect.width;
		textureRect.height = areaTextureRect.width;
		textureRect.center = new Vector2(areaTextureRect.width * 0.5f, areaTextureRect.height * 0.5f);
	}

	int IntField(string label, int value)
	{
		var tmp = value;
		value = EditorGUILayout.IntField(label, value);
		if (value != tmp)
			modified = true;

		return value;
	}
	float FloatField(string label, float value)
	{
		var tmp = value;
		value = EditorGUILayout.FloatField(label, value);
		if (value != tmp)
			modified = true;

		return value;
	}
	Vector2 Vector2Field(string label, Vector2 value)
	{
		var tmp = value;
		value = EditorGUILayout.Vector2Field(label, value);
		if (value != tmp)
			modified = true;

		return value;
	}
	float Slider(string label, float value, float leftValue, float rightValue)
	{
		var tmp = value;
		value = EditorGUILayout.Slider(label, value, leftValue, rightValue);
		if (value != tmp)
			modified = true;

		return value;
	}

	void DisplaySimpleNoiseSetting()
	{
		GUILayout.Label("Simple Noise Setting");

		noiseSetting.resolution = IntField("Resolution", noiseSetting.resolution);
		noiseSetting.tiling = Vector2Field("Tiling", noiseSetting.tiling);

		noiseInput = Vector2Field("Input", noiseInput);
	}
	void DisplayFractalNoiseSetting()
	{
		GUILayout.Label("Fractal Noise Setting");
		fractalNoiseSetting.resolution = IntField("Resolution", fractalNoiseSetting.resolution);
		fractalNoiseSetting.tiling = Vector2Field("Tiling", fractalNoiseSetting.tiling);
		fractalNoiseSetting.layer = IntField("Layer", fractalNoiseSetting.layer);
		fractalNoiseSetting.lacunarity = FloatField("Lacunarity", fractalNoiseSetting.lacunarity);
		fractalNoiseSetting.persistance = Slider("Persistance", fractalNoiseSetting.persistance, 0.0f, 1.0f);
		fractalNoiseSetting.seed = IntField("Seed", fractalNoiseSetting.seed);
		
		noiseInput = Vector2Field("Input", noiseInput);
	}
	void DisplayTurbulenceNoiseSetting()
	{
		GUILayout.Label("Turbulence Noise Setting");
		turbulenceNoiseSetting.resolution = IntField("Resolution", turbulenceNoiseSetting.resolution);
		turbulenceNoiseSetting.tiling = Vector2Field("Tiling", turbulenceNoiseSetting.tiling);
		turbulenceNoiseSetting.layer = IntField("Layer", turbulenceNoiseSetting.layer);
		turbulenceNoiseSetting.lacunarity = FloatField("Lacunarity", turbulenceNoiseSetting.lacunarity);
		turbulenceNoiseSetting.persistance = Slider("Persistance", turbulenceNoiseSetting.persistance, 0.0f, 1.0f);
		turbulenceNoiseSetting.seed = IntField("Seed", turbulenceNoiseSetting.seed);

		noiseInput = Vector2Field("Input", noiseInput);
	}
	void DisplayMarbleNoiseSetting()
	{
		GUILayout.Label("Marble Noise Setting");
		marbleNoiseSetting.resolution = IntField("Resolution", marbleNoiseSetting.resolution);
		marbleNoiseSetting.tiling = Vector2Field("Tiling", marbleNoiseSetting.tiling);
		marbleNoiseSetting.layer = IntField("Layer", marbleNoiseSetting.layer);
		marbleNoiseSetting.lacunarity = FloatField("Lacunarity", marbleNoiseSetting.lacunarity);
		marbleNoiseSetting.persistance = Slider("Persistance", marbleNoiseSetting.persistance, 0.0f, 1.0f);
		marbleNoiseSetting.seed = IntField("Seed", marbleNoiseSetting.seed);

		noiseInput = Vector2Field("Input", noiseInput);
	}
	void DisplayWoodNoiseSetting()
	{
		GUILayout.Label("Wood Noise Setting");

		woodNoiseSetting.resolution = IntField("Resolution", woodNoiseSetting.resolution);
		woodNoiseSetting.tiling = Vector2Field("Tiling", woodNoiseSetting.tiling);
		woodNoiseSetting.layer = IntField("Layer", woodNoiseSetting.layer);
		woodNoiseSetting.lacunarity = FloatField("Lacunarity", woodNoiseSetting.lacunarity);
		woodNoiseSetting.persistance = Slider("Persistance", woodNoiseSetting.persistance, 0.0f, 1.0f);
		woodNoiseSetting.seed = IntField("Seed", woodNoiseSetting.seed);

		noiseInput = Vector2Field("Input", noiseInput);
	}

	void DisplayWindowSetting()
	{
		GUILayout.BeginArea(windowSettingRect);
		{
			GUILayout.Label("Texture Setting");
			displayMode = (DisplayMode)EditorGUILayout.EnumPopup("Display Mode", displayMode);
			textureResolution = EditorGUILayout.IntField("Texture Resolution", textureResolution);
			textureFormat = (TextureFormat)EditorGUILayout.EnumPopup("Texture Format", textureFormat);
			contrast = EditorGUILayout.IntField("Contrast", contrast);
		}
		GUILayout.EndArea();
	}
	void DisplayAreaTexture()
	{
		GUILayout.BeginArea(areaTextureRect);
		{
			GUI.DrawTexture(textureRect, noiseTexture);
		}
		GUILayout.EndArea();
	}
	void DisplayNoiseSetting()
	{
		GUILayout.BeginArea(noiseSettingRect);
		{
			if (displayMode == DisplayMode.SimpleNoise)
				DisplaySimpleNoiseSetting();
			else if (displayMode == DisplayMode.FractalNoise)
				DisplayFractalNoiseSetting();
			else if (displayMode == DisplayMode.TurbulenceNoise)
				DisplayTurbulenceNoiseSetting();
			else if (displayMode == DisplayMode.MarbleNoise)
				DisplayMarbleNoiseSetting();
			else if (displayMode == DisplayMode.WoodNoise)
				DisplayWoodNoiseSetting();

			autoGenerate = GUILayout.Toggle(autoGenerate, "AutoGenerate");

			if (GUILayout.Button("Generate") || (autoGenerate && modified))
			{
				if (displayMode == DisplayMode.SimpleNoise)
					noiseTexture = NoiseUtility.GenerateSimpleNoiseTexture(noiseInput, noiseSetting, contrast, textureResolution, textureFormat);
				else if (displayMode == DisplayMode.FractalNoise)
					noiseTexture = NoiseUtility.GenerateFractalNoiseTexture(noiseInput, fractalNoiseSetting, contrast, textureResolution, textureFormat);
				else if (displayMode == DisplayMode.TurbulenceNoise)
					noiseTexture = NoiseUtility.GenerateTurbulenceNoiseTexture(noiseInput, turbulenceNoiseSetting, contrast, textureResolution, textureFormat);
				else if (displayMode == DisplayMode.MarbleNoise)
					noiseTexture = NoiseUtility.GenerateMarbleNoiseTexture(noiseInput, marbleNoiseSetting, contrast, textureResolution, textureFormat);
				else if (displayMode == DisplayMode.WoodNoise)
					noiseTexture = NoiseUtility.GenerateWoodNoiseTexture(noiseInput, woodNoiseSetting, contrast, textureResolution, textureFormat);

				modified = false;
			}
		}
		GUILayout.EndArea();
	}

	void DisplayLine()
	{
		Handles.color = Color.black;

		var p1 = new Vector2(areaTextureRect.x, areaTextureRect.y);
		var p2 = new Vector2(areaTextureRect.x, areaTextureRect.yMax);
		Handles.DrawLine(p1, p2);

		p1 = new Vector2(noiseSettingRect.x, noiseSettingRect.y);
		p2 = new Vector2(noiseSettingRect.x, noiseSettingRect.yMax);
		Handles.DrawLine(p1, p2);
	}

	private void Awake()
	{
		this.minSize = new Vector2(width, height);
		this.maxSize = new Vector2(width, height);
		
		InitRect();
		InitTexture();
	}
	private void OnGUI()
	{
		DisplayWindowSetting();
		DisplayAreaTexture();
		DisplayNoiseSetting();
		
		DisplayLine();
	}

	[MenuItem("Tools/NoiseGenerator")]
	public static void OpenWindow()
	{
		GetWindow<NoiseGenerationWindow>("NoiseGeneration");
	}
}
