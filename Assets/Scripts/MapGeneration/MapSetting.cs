using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMapSetting", menuName = "MapSetting")]
public class MapSetting : ScriptableObject
{
	public static MapSetting Default { get => CreateDefault(); }
	static MapSetting CreateDefault()
	{
		var setting = new MapSetting();

		setting.squareSize = 1;

		setting.seed = 0;
		setting.octaves = 8;
		setting.persistance = 0.5f;
		setting.lacunarity = 2.0f;
		setting.scale = Vector2.one;

		return setting;
	}

	public int squareSize = 1;

	public int seed = 0;
	public int octaves = 8;
	public float persistance = 0.5f;
	public float lacunarity = 2.0f;
	public Vector2 scale = Vector2.one;
}
