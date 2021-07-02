using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapGenerationWindow : EditorWindow
{
	float width = 1200.0f;
	float height = 700.0f;

	float lineFactor = 0.75f;
	Rect leftRect = Rect.zero;
	Rect rightRect = Rect.zero;
	Rect lineRect = Rect.zero;

	MapSetting mapSetting = null;

	float textureFactor = 0.90f;
	Rect textureRect = Rect.zero;
	int textureResolution = 1;
	Rect resolutionRect = Rect.zero;

	Texture mapTexture = null;

	void OnPlayModeStateChanged(PlayModeStateChange stateChange)
	{
		if (stateChange == PlayModeStateChange.EnteredPlayMode)
			this.Close();
	}

	void Init(string assetPath)
	{
		mapSetting = AssetDatabase.LoadAssetAtPath<MapSetting>(assetPath);
	}

	Texture GenerateTexture()
	{
		int start = -Mathf.FloorToInt(mapSetting.squareSize / 2.0f);
		int size = Mathf.CeilToInt(mapSetting.squareSize / 2.0f);

		Texture2D texture = new Texture2D(textureResolution, textureResolution);
		texture.filterMode = FilterMode.Point;

		float ratio = mapSetting.squareSize / textureResolution;

		for (int i = 0; i < textureResolution; i++)
			for (int j = 0; j < textureResolution; j++)
			{
				float noise = Noise.CoherentNoise2D((i + 1) * ratio, (j + 1) * ratio, mapSetting.octaves, mapSetting.persistance, mapSetting.lacunarity, mapSetting.scale.x, mapSetting.scale.y, mapSetting.seed);
				texture.SetPixel(i, j, Color.Lerp(Color.black, Color.white, noise));
			}

		texture.Apply();

		return texture;
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

		resolutionRect = new Rect(textureRect.x, textureRect.yMax + EditorGUIUtility.standardVerticalSpacing, textureRect.width, EditorGUIUtility.singleLineHeight);

		mapTexture = Texture2D.whiteTexture;

		EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
	}
	private void OnDestroy()
	{
		EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
	}
	private void OnGUI()
	{
		GUILayout.BeginArea(leftRect);
		{
			GUI.DrawTexture(textureRect, mapTexture);

			textureResolution = EditorGUI.IntField(resolutionRect, "Resolution", textureResolution);
		}
		GUILayout.EndArea();


		EditorGUI.DrawRect(lineRect, Color.black);


		GUILayout.BeginArea(rightRect);
		{
			EditorGUILayout.BeginVertical();
			{
				mapSetting = (MapSetting)EditorGUILayout.ObjectField("MapSetting", mapSetting, typeof(MapSetting), false);

				if (mapSetting != null)
				{
					EditorGUI.indentLevel = 1;
					{
						mapSetting.squareSize = EditorGUILayout.IntField("SquareSize", mapSetting.squareSize);
						mapSetting.seed = EditorGUILayout.IntField("Seed", mapSetting.seed);
						mapSetting.octaves = EditorGUILayout.IntField("Octaves", mapSetting.octaves);
						mapSetting.persistance = EditorGUILayout.FloatField("Persistance", mapSetting.persistance);
						mapSetting.lacunarity = EditorGUILayout.FloatField("Lacunarity", mapSetting.lacunarity);
						mapSetting.scale = EditorGUILayout.Vector2Field("Scale", mapSetting.scale);
					}
					EditorGUI.indentLevel = 0;
				}

				if (GUILayout.Button("Generate"))
				{
					mapTexture = GenerateTexture();
				}
			}
			EditorGUILayout.EndVertical();
		}
		GUILayout.EndArea();
	}

	public static void OpenWindow()
	{
		GetWindow<MapGenerationWindow>("MapGeneration");
	}
	public static void OpenWindow(string assetPath)
	{
		var window = GetWindow<MapGenerationWindow>("MapGeneration");

		window.Init(assetPath);
	}
}
