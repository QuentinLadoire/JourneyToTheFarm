using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SimpleNoiseSetting
{
	public int resolution = 256;
	public Vector2 tiling = Vector2.one;

	public Vector2 ScaleFactor => tiling / resolution;
}

[System.Serializable]
public class FractalNoiseSetting
{
	public int resolution = 256;
	public Vector2 tiling = Vector2.one;

	public int layer = 5;
	public float lacunarity = 2.0f;
	[Range(0.0f, 1.0f)]
	public float persistance = 0.5f;
	public int seed = 0;

	public Vector2 ScaleFactor => tiling / resolution;
}

public static class NoiseUtility
{
	public static Texture2D GenerateSimpleNoiseTexture(Vector2 input, SimpleNoiseSetting setting, int contrast = 1, int resolution = 256, TextureFormat format = TextureFormat.RGBAHalf)
	{
		var texture = new Texture2D(resolution, resolution, format, false);

		for (int i = 0; i < resolution; i++)
			for (int j = 0; j < resolution; j++)
			{
				var noise = Mathf.Pow(SimpleNoiseNormalized(input.x + i, input.y + j, setting), contrast);
				texture.SetPixel(i, j, new Color(noise, noise, noise));
			}

		texture.Apply();

		return texture;
	}
	public static Texture2D GenerateFractalNoiseTexture(Vector2 input, FractalNoiseSetting setting, int contrast = 1, int resolution = 256, TextureFormat format = TextureFormat.RGBAHalf)
	{
		Texture2D texture = new Texture2D(resolution, resolution, format, false);

		for (int i = 0; i < resolution; i++)
			for (int j = 0; j < resolution; j++)
			{
				var noise = Mathf.Pow(FractalNoise2DNormalized(input.x + i, input.y + j, setting), contrast);
				texture.SetPixel(i, j, new Color(noise, noise, noise));
			}

		texture.Apply();

		return texture;
	}

	public static float SimpleNoise(float x, float y, SimpleNoiseSetting setting)
	{
		return Noise.SimpleNoise2D(x * setting.ScaleFactor.x, y * setting.ScaleFactor.y);
	}
	public static float SimpleNoiseNormalized(float x, float y, SimpleNoiseSetting setting)
	{
		return Noise.SimpleNoise2DNormalized(x * setting.ScaleFactor.x, y * setting.ScaleFactor.y);
	}

	public static float FractalNoise2D(float x, float y, FractalNoiseSetting setting)
	{
		return Noise.FractalNoise2D(x * setting.ScaleFactor.x, y * setting.ScaleFactor.y, setting.layer, setting.lacunarity, setting.persistance, setting.seed);
	}
	public static float FractalNoise2DNormalized(float x, float y, FractalNoiseSetting setting)
	{
		return Noise.FractalNoise2DNormalized(x * setting.ScaleFactor.x, y * setting.ScaleFactor.y, setting.layer, setting.lacunarity, setting.persistance, setting.seed);
	}
}
