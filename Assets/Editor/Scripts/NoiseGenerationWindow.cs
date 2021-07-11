using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

enum DisplayMode
{
	Noise,
	FractalNoise
}

public class NoiseGenerationWindow : EditorWindow
{
	float width = 1200.0f;
	float height = 700.0f;

	Rect windowSettingRect = Rect.zero;
	Rect areaTextureRect = Rect.zero;
	Rect noiseSettingRect = Rect.zero;


	[SerializeField] DisplayMode displayMode = DisplayMode.Noise;
	[SerializeField] int textureResolution = 256;
	[SerializeField] TextureFormat textureFormat = TextureFormat.RGBAHalf;
	[SerializeField]  int contrast = 3;

	Texture noiseTexture = null;
	Rect textureRect = Rect.zero;

	[SerializeField] Vector2 noiseInput = Vector2.zero;
	[SerializeField] SimpleNoiseSetting noiseSetting = new SimpleNoiseSetting();
	[SerializeField] FractalNoiseSetting fractalNoiseSetting = new FractalNoiseSetting();

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
			if (displayMode == DisplayMode.Noise)
			{
				GUILayout.Label("Noise Setting");
				noiseSetting.resolution = EditorGUILayout.IntField("Resolution", noiseSetting.resolution);
				noiseSetting.tiling = EditorGUILayout.Vector2Field("Tiling", noiseSetting.tiling);

				noiseInput = EditorGUILayout.Vector2Field("Input", noiseInput);
			}
			else if (displayMode == DisplayMode.FractalNoise)
			{
				GUILayout.Label("Fractal Noise Setting");
				fractalNoiseSetting.resolution = EditorGUILayout.IntField("Resolution", fractalNoiseSetting.resolution);
				fractalNoiseSetting.tiling = EditorGUILayout.Vector2Field("Tiling", fractalNoiseSetting.tiling);
				fractalNoiseSetting.layer = EditorGUILayout.IntField("Layer", fractalNoiseSetting.layer);
				fractalNoiseSetting.lacunarity = EditorGUILayout.FloatField("Lacunarity", fractalNoiseSetting.lacunarity);
				fractalNoiseSetting.persistance = EditorGUILayout.Slider("Persistance", fractalNoiseSetting.persistance, 0.0f, 1.0f);
				fractalNoiseSetting.seed = EditorGUILayout.IntField("Seed", fractalNoiseSetting.seed);

				noiseInput = EditorGUILayout.Vector2Field("Input", noiseInput);
			}

			if (GUILayout.Button("Generate"))
			{
				if (displayMode == DisplayMode.Noise)
					noiseTexture = NoiseUtility.GenerateSimpleNoiseTexture(noiseInput, noiseSetting, contrast, textureResolution, textureFormat);
				else if (displayMode == DisplayMode.FractalNoise)
					noiseTexture = NoiseUtility.GenerateFractalNoiseTexture(noiseInput, fractalNoiseSetting, contrast, textureResolution, textureFormat);
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
