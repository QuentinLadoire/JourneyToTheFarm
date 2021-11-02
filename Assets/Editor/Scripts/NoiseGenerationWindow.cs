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
	WoodNoise,
	BlendNoise
}

#region Layer
public enum LayerType
{
	Default,
	Color,
	Noise
}

public class Layer
{
	public virtual LayerType LayerType => LayerType.Default;

	public bool layerSettingFoldout = false;
	public BlendMode blendMode = BlendMode.Normal;
	public float opacity = 1.0f;

	public HeightmapData heightmapData = null;

	public virtual void GenerateHeightmap()
	{
		
	}

	void DisplaySetting()
	{
		layerSettingFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(layerSettingFoldout, "Layer Setting");
		if (layerSettingFoldout)
		{
			EditorGUI.indentLevel = 1;
			{
				blendMode = (BlendMode)EditorGUILayout.EnumPopup("BlendMode", blendMode);
				opacity = EditorGUILayout.Slider("Opacity", opacity, 0.0f, 1.0f);
			}
			EditorGUI.indentLevel = 0;
		}
		EditorGUILayout.EndFoldoutHeaderGroup();
	}
	public virtual void Display()
	{
		DisplaySetting();
	}
}
public class ColorLayer : Layer
{
	public override LayerType LayerType => LayerType.Color;

	public bool colorSettingFoldout = false;
	public SimpleColorSetting setting = new SimpleColorSetting();

	public override void GenerateHeightmap()
	{
		heightmapData = HeightmapUtility.GenerateHeightmapFrom(setting);
	}

	void DisplaySimpleColorSetting()
	{
		colorSettingFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(colorSettingFoldout, "Color Setting");
		if (colorSettingFoldout)
		{
			EditorGUI.indentLevel = 1;
			{
				setting.resolution = EditorGUILayout.IntField("Resolution", setting.resolution);
				setting.grayValue = EditorGUILayout.Slider("Color", setting.grayValue, 0.0f, 1.0f);
			}
			EditorGUI.indentLevel = 0;
		}
		EditorGUILayout.EndFoldoutHeaderGroup();
	}
	public override void Display()
	{
		base.Display();

		DisplaySimpleColorSetting();
	}
}
public class NoiseLayer : Layer
{
	public override LayerType LayerType => LayerType.Noise;

	public bool noiseSettingFoldout = false;
	public FractalNoiseSetting setting = new FractalNoiseSetting();

	public override void GenerateHeightmap()
	{
		heightmapData = HeightmapUtility.GenerateHeightmapFrom(Vector2.zero, setting);
	}

	void DisplayFractalNoiseSetting()
	{
		noiseSettingFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(noiseSettingFoldout, "Noise Setting");
		if (noiseSettingFoldout)
		{
			EditorGUI.indentLevel = 1;
			{
				setting.resolution = EditorGUILayout.IntField("Resolution", setting.resolution);
				setting.tiling = EditorGUILayout.Vector2Field("Tiling", setting.tiling);
				setting.layer = EditorGUILayout.IntField("Layer", setting.layer);
				setting.lacunarity = EditorGUILayout.FloatField("Lacunarity", setting.lacunarity);
				setting.persistance = EditorGUILayout.Slider("Persistance", setting.persistance, 0.0f, 1.0f);
			}
			EditorGUI.indentLevel = 0;
		}
		EditorGUILayout.EndFoldoutHeaderGroup();
	}
	public override void Display()
	{
		base.Display();

		DisplayFractalNoiseSetting();
	}
}
#endregion

public class NoiseGenerationWindow : EditorWindow
{
	readonly float width = 1200.0f;
	readonly float height = 700.0f;

	Rect windowSettingRect = Rect.zero;
	Rect areaTextureRect = Rect.zero;
	Rect noiseSettingRect = Rect.zero;
	Rect noiseLayerListRect = Rect.zero;

	DisplayMode displayMode = DisplayMode.FractalNoise;
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

	readonly List<Layer> layerList = new List<Layer>();

	float fractalNoiseValue = 0.0f;
	float fractalNoiseValueBis = 0.0f;

	bool autoGenerate = false;
	bool modified = false;

	#region CustomGUILayout
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
	#endregion

	void InitRect()
	{
		windowSettingRect.x = 0.0f;
		windowSettingRect.y = 0.0f;
		windowSettingRect.width = position.width * 0.25f;
		windowSettingRect.height = position.height;

		areaTextureRect.x = windowSettingRect.xMax;
		areaTextureRect.y = 0.0f;
		areaTextureRect.width = position.width * 0.50f;
		areaTextureRect.height = position.height;

		noiseSettingRect.x = areaTextureRect.xMax;
		noiseSettingRect.y = 0.0f;
		noiseSettingRect.width = position.width * 0.25f;
		noiseSettingRect.height = position.height;

		noiseLayerListRect.x = 0.0f;
		noiseLayerListRect.y = 17.0f;
		noiseLayerListRect.width = 300.0f;
		noiseLayerListRect.height = 300.0f;

		textureRect.width = areaTextureRect.width;
		textureRect.height = areaTextureRect.width;
		textureRect.center = new Vector2(areaTextureRect.width * 0.5f, areaTextureRect.height * 0.5f);
	}

	void GenerateSimpleNoise()
	{
		noiseTexture = NoiseUtility.GenerateSimpleNoiseTexture(noiseInput, noiseSetting, contrast, textureFormat);
	}
	void GenerateFractalNoise()
	{
		noiseTexture = NoiseUtility.GenerateFractalNoiseTexture(noiseInput, fractalNoiseSetting, fractalNoiseValue, fractalNoiseValueBis, contrast, textureFormat);
	}
	void GenerateBlendNoise()
	{
		foreach (var layer in layerList)
			layer.GenerateHeightmap();

		HeightmapData outHeightmap = layerList[0].heightmapData;
		for (int i = 0; i < layerList.Count - 1; i++)
		{
			var blendLayer = layerList[i + 1];
			outHeightmap = HeightmapUtility.Blend(outHeightmap, blendLayer.heightmapData, blendLayer.blendMode, blendLayer.opacity);
		}

		noiseTexture = HeightmapUtility.GenerateTextureFromHeightmap(outHeightmap, contrast, textureFormat);
	}

	void DisplaySimpleNoiseSettingMode()
	{
		GUILayout.Label("Simple Noise Setting");

		noiseSetting.resolution = IntField("Resolution", noiseSetting.resolution);
		noiseSetting.tiling = Vector2Field("Tiling", noiseSetting.tiling);

		noiseInput = Vector2Field("Input", noiseInput);
	}
	void DisplayFractalNoiseSettingMode()
	{
		GUILayout.Label("Fractal Noise Setting");
		fractalNoiseSetting.resolution = IntField("Resolution", fractalNoiseSetting.resolution);
		fractalNoiseSetting.tiling = Vector2Field("Tiling", fractalNoiseSetting.tiling);
		fractalNoiseSetting.layer = IntField("Layer", fractalNoiseSetting.layer);
		fractalNoiseSetting.lacunarity = FloatField("Lacunarity", fractalNoiseSetting.lacunarity);
		fractalNoiseSetting.persistance = Slider("Persistance", fractalNoiseSetting.persistance, 0.0f, 1.0f);
		
		noiseInput = Vector2Field("Input", noiseInput);

		fractalNoiseValue = Slider("Value", fractalNoiseValue, 0.0f, 1.0f);
		fractalNoiseValueBis = Slider("Value", fractalNoiseValueBis, 0.0f, 1.0f);
	}
	void DisplayTurbulenceNoiseSettingMode()
	{
		GUILayout.Label("Turbulence Noise Setting");
		turbulenceNoiseSetting.resolution = IntField("Resolution", turbulenceNoiseSetting.resolution);
		turbulenceNoiseSetting.tiling = Vector2Field("Tiling", turbulenceNoiseSetting.tiling);
		turbulenceNoiseSetting.layer = IntField("Layer", turbulenceNoiseSetting.layer);
		turbulenceNoiseSetting.lacunarity = FloatField("Lacunarity", turbulenceNoiseSetting.lacunarity);
		turbulenceNoiseSetting.persistance = Slider("Persistance", turbulenceNoiseSetting.persistance, 0.0f, 1.0f);
		
		noiseInput = Vector2Field("Input", noiseInput);
	}
	void DisplayMarbleNoiseSettingMode()
	{
		GUILayout.Label("Marble Noise Setting");
		marbleNoiseSetting.resolution = IntField("Resolution", marbleNoiseSetting.resolution);
		marbleNoiseSetting.tiling = Vector2Field("Tiling", marbleNoiseSetting.tiling);
		marbleNoiseSetting.layer = IntField("Layer", marbleNoiseSetting.layer);
		marbleNoiseSetting.lacunarity = FloatField("Lacunarity", marbleNoiseSetting.lacunarity);
		marbleNoiseSetting.persistance = Slider("Persistance", marbleNoiseSetting.persistance, 0.0f, 1.0f);
		
		noiseInput = Vector2Field("Input", noiseInput);
	}
	void DisplayWoodNoiseSettingMode()
	{
		GUILayout.Label("Wood Noise Setting");

		woodNoiseSetting.resolution = IntField("Resolution", woodNoiseSetting.resolution);
		woodNoiseSetting.tiling = Vector2Field("Tiling", woodNoiseSetting.tiling);
		woodNoiseSetting.layer = IntField("Layer", woodNoiseSetting.layer);
		woodNoiseSetting.lacunarity = FloatField("Lacunarity", woodNoiseSetting.lacunarity);
		woodNoiseSetting.persistance = Slider("Persistance", woodNoiseSetting.persistance, 0.0f, 1.0f);
		
		noiseInput = Vector2Field("Input", noiseInput);
	}
	void DisplayBlendNoiseSettingMode()
	{
		GUILayout.Label("Blend Noise Setting");

		for (int i = 0; i < layerList.Count; i++)
		{
			GUILayout.Label("Layer " + i);
			layerList[i].Display();

			GUILayout.Space(5.0f);
		}

		GUILayout.BeginHorizontal();
		{
			if (GUILayout.Button("Add Color Layer"))
			{
				layerList.Add(new ColorLayer());
			}
			if (GUILayout.Button("Add Noise Layer"))
			{
				layerList.Add(new NoiseLayer());
			}
		}
		GUILayout.EndHorizontal();
	}

	void DisplayWindowSetting()
	{
		GUILayout.BeginArea(windowSettingRect);
		{
			GUILayout.Label("Texture Setting");
			displayMode = (DisplayMode)EditorGUILayout.EnumPopup("Display Mode", displayMode);
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
				DisplaySimpleNoiseSettingMode();
			else if (displayMode == DisplayMode.FractalNoise)
				DisplayFractalNoiseSettingMode();
			else if (displayMode == DisplayMode.TurbulenceNoise)
				DisplayTurbulenceNoiseSettingMode();
			else if (displayMode == DisplayMode.MarbleNoise)
				DisplayMarbleNoiseSettingMode();
			else if (displayMode == DisplayMode.WoodNoise)
				DisplayWoodNoiseSettingMode();
			else if (displayMode == DisplayMode.BlendNoise)
				DisplayBlendNoiseSettingMode();

			autoGenerate = GUILayout.Toggle(autoGenerate, "AutoGenerate");

			if (GUILayout.Button("Generate") || (autoGenerate && modified))
			{
				if (displayMode == DisplayMode.SimpleNoise)
					GenerateSimpleNoise();
				else if (displayMode == DisplayMode.FractalNoise)
					GenerateFractalNoise();
				else if (displayMode == DisplayMode.TurbulenceNoise)
					noiseTexture = NoiseUtility.GenerateTurbulenceNoiseTexture(noiseInput, turbulenceNoiseSetting, contrast, textureFormat);
				else if (displayMode == DisplayMode.MarbleNoise)
					noiseTexture = NoiseUtility.GenerateMarbleNoiseTexture(noiseInput, marbleNoiseSetting, contrast, textureFormat);
				else if (displayMode == DisplayMode.WoodNoise)
					noiseTexture = NoiseUtility.GenerateWoodNoiseTexture(noiseInput, woodNoiseSetting, contrast, textureFormat);
				else if (displayMode == DisplayMode.BlendNoise)
					GenerateBlendNoise();

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
		var rect = position;
		rect.size = new Vector2(width, height);
		position = rect;

		noiseTexture = Texture2D.whiteTexture;

		InitRect();
	}
	private void OnGUI()
	{
		InitRect();

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
