using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SimpleColorSetting
{
	public int resolution = 256;
	public float grayValue = 0.5f;
}
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
	
	public Vector2 ScaleFactor => tiling / resolution;
}

public static class NoiseUtility
{
	public static Texture2D GenerateSimpleColorTexture(SimpleColorSetting setting, int contrast = 1, TextureFormat format = TextureFormat.RGBAHalf)
	{
		Texture2D texture = new Texture2D(setting.resolution, setting.resolution, format, false);

		for (int i = 0; i < setting.resolution; i++)
			for (int j = 0; j < setting.resolution; j++)
			{
				var color = Mathf.Pow(setting.grayValue, contrast);
				texture.SetPixel(i, j, new Color(color, color, color));
			}

		texture.Apply();

		return texture;
	}
	public static Texture2D GenerateSimpleNoiseTexture(Vector2 input, SimpleNoiseSetting setting, int contrast = 1, TextureFormat format = TextureFormat.RGBAHalf)
	{
		var texture = new Texture2D(setting.resolution, setting.resolution, format, false);

		for (int i = 0; i < setting.resolution; i++)
			for (int j = 0; j < setting.resolution; j++)
			{
				var noise = Mathf.Pow(SimpleNoise2DNormalized(input.x + i, input.y + j, setting), contrast);
				texture.SetPixel(i, j, new Color(noise, noise, noise));
			}

		texture.Apply();

		return texture;
	}
	public static Texture2D GenerateFractalNoiseTexture(Vector2 input, FractalNoiseSetting setting, int contrast = 1, TextureFormat format = TextureFormat.RGBAHalf)
	{
		Texture2D texture = new Texture2D(setting.resolution, setting.resolution, format, false);

		for (int i = 0; i < setting.resolution; i++)
			for (int j = 0; j < setting.resolution; j++)
			{
				var noise = Mathf.Pow(FractalNoise2DNormalized(input.x + i, input.y + j, setting), contrast);
				texture.SetPixel(i, j, new Color(noise, noise, noise));
			}

		texture.Apply();

		return texture;
	}
	public static Texture2D GenerateFractalNoiseTexture(Vector2 input, FractalNoiseSetting setting, float value, float valueBis, int contrast = 1, TextureFormat format = TextureFormat.RGBAHalf)
	{
		Texture2D texture = new Texture2D(setting.resolution, setting.resolution, format, false);

		for (int i = 0; i < setting.resolution; i++)
			for (int j = 0; j < setting.resolution; j++)
			{
				var noise = FractalNoise2DNormalized(input.x + i, input.y + j, setting);
				if (noise < value)
					texture.SetPixel(i, j, Color.green);
				else if (noise < valueBis)
					texture.SetPixel(i, j, Color.red);
				else
				{
					noise = Mathf.Pow(noise, contrast);
					texture.SetPixel(i, j, new Color(noise, noise, noise));
				}
			}

		texture.Apply();

		return texture;
	}
	public static Texture2D GenerateTurbulenceNoiseTexture(Vector2 input, FractalNoiseSetting setting, int contrast = 1, TextureFormat format = TextureFormat.RGBAHalf)
	{
		Texture2D texture = new Texture2D(setting.resolution, setting.resolution, format, false);

		for (int i = 0; i < setting.resolution; i++)
			for (int j = 0; j < setting.resolution; j++)
			{
				var noise = Mathf.Pow(TurbulenceNoise2D(input.x + i, input.y + j, setting), contrast);
				texture.SetPixel(i, j, new Color(noise, noise, noise));
			}

		texture.Apply();

		return texture;
	}
	public static Texture2D GenerateMarbleNoiseTexture(Vector2 input, FractalNoiseSetting setting, int contrast = 1, TextureFormat format = TextureFormat.RGBAHalf)
	{
		Texture2D texture = new Texture2D(setting.resolution, setting.resolution, format, false);

		for (int i = 0; i < setting.resolution; i++)
			for (int j = 0; j < setting.resolution; j++)
			{
				var noise = Mathf.Pow(MarbleNoise2DNormalized(input.x + i, input.y + j, setting), contrast);
				texture.SetPixel(i, j, new Color(noise, noise, noise));
			}

		texture.Apply();

		return texture;
	}
	public static Texture2D GenerateWoodNoiseTexture(Vector2 input, FractalNoiseSetting setting, int contrast = 1, TextureFormat format = TextureFormat.RGBAHalf)
	{
		Texture2D texture = new Texture2D(setting.resolution, setting.resolution, format, false);

		for (int i = 0; i < setting.resolution; i++)
			for (int j = 0; j < setting.resolution; j++)
			{
				var noise = Mathf.Pow(WoodNoise2D(input.x + i, input.y + j, setting), contrast);
				texture.SetPixel(i, j, new Color(noise, noise, noise));
			}

		texture.Apply();

		return texture;
	}

	public static float SimpleNoise2D(float x, float y, SimpleNoiseSetting setting)
	{
		return Noise.SimpleNoise2D(x * setting.ScaleFactor.x, y * setting.ScaleFactor.y);
	}
	public static float SimpleNoise2DNormalized(float x, float y, SimpleNoiseSetting setting)
	{
		return Noise.SimpleNoise2DNormalized(x * setting.ScaleFactor.x, y * setting.ScaleFactor.y);
	}

	public static float FractalNoise2D(float x, float y, FractalNoiseSetting setting)
	{
		return Noise.FractalNoise2D(x * setting.ScaleFactor.x, y * setting.ScaleFactor.y, setting.layer, setting.lacunarity, setting.persistance, 0.0f, 0.0f);
	}
	public static float FractalNoise2DNormalized(float x, float y, FractalNoiseSetting setting)
	{
		return Noise.FractalNoise2DNormalized(x * setting.ScaleFactor.x, y * setting.ScaleFactor.y, setting.layer, setting.lacunarity, setting.persistance, 0.0f, 0.0f);
	}

	public static float TurbulenceNoise2D(float x, float y, FractalNoiseSetting setting)
	{
		return Noise.TurbulenceNoise2D(x * setting.ScaleFactor.x, y * setting.ScaleFactor.y, setting.layer, setting.lacunarity, setting.persistance, 0.0f, 0.0f);
	}

	public static float MarbleNoise2D(float x, float y, FractalNoiseSetting setting)
	{
		return Noise.MarbleNoise2D(x, y, setting.ScaleFactor.x, setting.ScaleFactor.y, setting.layer, setting.lacunarity, setting.persistance, 0.0f, 0.0f);
	}
	public static float MarbleNoise2DNormalized(float x, float y, FractalNoiseSetting setting)
	{
		return Noise.MarbleNoise2DNormalized(x, y, setting.ScaleFactor.x, setting.ScaleFactor.y, setting.layer, setting.lacunarity, setting.persistance, 0.0f, 0.0f);
	}

	public static float WoodNoise2D(float x, float y, FractalNoiseSetting setting)
	{
		return Noise.WoodNoise2D(x * setting.ScaleFactor.x, y * setting.ScaleFactor.y, setting.layer, setting.lacunarity, setting.persistance, 0.0f, 0.0f);
	}
}
