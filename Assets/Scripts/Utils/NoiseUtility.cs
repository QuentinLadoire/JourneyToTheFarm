using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSetting
{
	public int octaves = 4;
	[Range(0.0f, 1.0f)]
	public float persistance = 0.5f;
	public float lacunarity = 2.0f;
	public Vector2 scale = Vector2.one;
	public int seed = 0;
}

public static class NoiseUtility
{
	public static Texture2D GenerateNoiseMap(int textureResolution, TextureFormat format, int noiseSize, NoiseSetting setting)
	{
		Texture2D texture = new Texture2D(textureResolution, textureResolution, format, false);
		texture.filterMode = FilterMode.Point;

		float ratio = (noiseSize + 1) / textureResolution;
		for (int i = 0; i < textureResolution; i++)
			for (int j = 0; j < textureResolution; j++)
			{
				float noise = CoherentNoise2DNormalized((i + 1) * ratio, (j + 1) * ratio, setting);
				texture.SetPixel(i, j, Color.Lerp(Color.black, Color.white, noise));
			}

		texture.Apply();

		return texture;
	}

	public static float CoherentNoise2D(float x, float y, NoiseSetting setting)
	{
		return Noise.CoherentNoise2D(x, y, setting.octaves, setting.persistance, setting.lacunarity, setting.scale.x, setting.scale.y, setting.seed);
	}
	public static float CoherentNoise2DNormalized(float x, float y, NoiseSetting setting)
	{
		return Noise.CoherentNoise2DNormalized(x, y, setting.octaves, setting.persistance, setting.lacunarity, setting.scale.x, setting.scale.y, setting.seed);
	}
}
