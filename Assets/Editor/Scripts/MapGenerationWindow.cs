using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

enum DisplayMode
{
	GroundNoise,
	MountainNoise
}

public class MapGenerationWindow : EditorWindow
{
	float width = 1200.0f;
	float height = 700.0f;

	float lineFactor = 0.75f;
	Rect leftRect = Rect.zero;
	Rect rightRect = Rect.zero;
	Rect lineRect = Rect.zero;

	float textureFactor = 0.90f;
	Rect textureRect = Rect.zero;
	int textureResolution = 256;
	Rect resolutionRect = Rect.zero;

	Rect enumRect = Rect.zero;
	DisplayMode displayMode = DisplayMode.GroundNoise;

	MapSetting mapSetting = null;
	int squareSize = 255;

	Texture mapTexture = null;

	void DisplayNoiseSetting(NoiseSetting setting, string name)
	{
		EditorGUILayout.LabelField(name);
		EditorGUI.indentLevel = 1;
		{
			setting.seed = EditorGUILayout.IntField("Seed", setting.seed);
			setting.octaves = EditorGUILayout.IntField("Octaves", setting.octaves);
			setting.persistance = EditorGUILayout.Slider("Persistance", setting.persistance, 0.0f, 1.0f);
			setting.lacunarity = EditorGUILayout.FloatField("Lacunarity", setting.lacunarity);
			setting.scale = EditorGUILayout.Vector2Field("Scale", setting.scale);
		}
		EditorGUI.indentLevel = 0;
	}
	void DisplayGroundNoiseTexture(NoiseSetting setting)
	{
		mapTexture = NoiseUtility.GenerateNoiseMap(textureResolution, TextureFormat.RGBAHalf, squareSize, setting);
	}
	void DisplayLeftArea()
	{
		GUILayout.BeginArea(leftRect);
		{
			displayMode = (DisplayMode)EditorGUI.EnumPopup(enumRect, displayMode);

			GUI.DrawTexture(textureRect, mapTexture);

			textureResolution = EditorGUI.IntField(resolutionRect, "Resolution", textureResolution);
		}
		GUILayout.EndArea();
	}
	void DisplayRightArea()
	{
		GUILayout.BeginArea(rightRect);
		{
			EditorGUILayout.BeginVertical();
			{
				mapSetting = (MapSetting)EditorGUILayout.ObjectField("MapSetting", mapSetting, typeof(MapSetting), false);
				squareSize = EditorGUILayout.IntField("SquareSize", squareSize);

				if (mapSetting != null)
				{
					DisplayNoiseSetting(mapSetting.groundNoiseSetting, "Ground Noise Setting");
					DisplayNoiseSetting(mapSetting.mountainNoiseSetting, "Mountain Noise Setting");
				}

				if (GUILayout.Button("Generate"))
				{
					switch (displayMode)
					{
						case DisplayMode.GroundNoise:
							DisplayGroundNoiseTexture(mapSetting.groundNoiseSetting);
							break;
						case DisplayMode.MountainNoise:
							DisplayGroundNoiseTexture(mapSetting.mountainNoiseSetting);
							break;
					}
				}
			}
			EditorGUILayout.EndVertical();
		}
		GUILayout.EndArea();
	}

	private void Awake()
	{
		this.minSize = new Vector2(width, height);
		this.maxSize = new Vector2(width, height);

		leftRect = new Rect(0.0f, 0.0f, lineFactor * width, height);
		rightRect = new Rect(lineFactor * width, 0.0f, (1 - lineFactor) * width, height);
		lineRect = new Rect(lineFactor * width, 0.0f, 1.0f, height);

		textureRect.width = textureFactor * leftRect.height;
		textureRect.height = textureFactor * leftRect.height;
		textureRect.center = leftRect.center;

		enumRect = new Rect(textureRect.x, textureRect.y - EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing, textureRect.width, EditorGUIUtility.singleLineHeight);
		resolutionRect = new Rect(textureRect.x, textureRect.yMax + EditorGUIUtility.standardVerticalSpacing, textureRect.width, EditorGUIUtility.singleLineHeight);

		mapTexture = Texture2D.whiteTexture;
	}
	private void OnGUI()
	{
		DisplayLeftArea();

		EditorGUI.DrawRect(lineRect, Color.black);

		DisplayRightArea();
	}
	
	[MenuItem("Window/MapSetting")]
	public static void OpenWindow()
	{
		GetWindow<MapGenerationWindow>("MapGeneration");
	}
}
