using UnityEngine;

public enum BlendMode
{
	Normal,
	Multiply,
	Darken,
	Lighten
}

public class HeightmapData
{
	public int resolution = 256;
	public float[,] data = null;
}

public static class HeightmapUtility
{
	public static HeightmapData GenerateHeightmapFrom(SimpleColorSetting setting)
	{
		HeightmapData heightmap = new HeightmapData()
		{
			resolution = setting.resolution,
			data = new float[setting.resolution, setting.resolution]
		};

		for (int i = 0; i < setting.resolution; i++)
			for (int j = 0; j < setting.resolution; j++)
			{
				heightmap.data[i, j] = setting.grayValue;
			}

		return heightmap;
	}
	public static HeightmapData GenerateHeightmapFrom(Vector2 offset, SimpleNoiseSetting setting)
	{
		HeightmapData heightmap = new HeightmapData()
		{
			resolution = setting.resolution,
			data = new float[setting.resolution, setting.resolution]
		};

		for (int i = 0; i < setting.resolution; i++)
			for (int j = 0; j < setting.resolution; j++)
			{
				heightmap.data[i, j] = NoiseUtility.SimpleNoise2DNormalized(i + offset.x, j + offset.y, setting);
			}

		return heightmap;
	}
	public static HeightmapData GenerateHeightmapFrom(Vector2 offset, FractalNoiseSetting setting)
	{
		HeightmapData heightmap = new HeightmapData()
		{
			resolution = setting.resolution,
			data = new float[setting.resolution, setting.resolution]
		};

		for (int i = 0; i < setting.resolution; i++)
			for (int j = 0; j < setting.resolution; j++)
			{
				heightmap.data[i, j] = NoiseUtility.FractalNoise2DNormalized(i + offset.x, j + offset.y, setting);
			}

		return heightmap;
	}

	public static HeightmapData NormalBlend(HeightmapData layer1, HeightmapData layer2)
	{
		HeightmapData heightmap = new HeightmapData()
		{
			resolution = layer1.resolution,
			data = new float[layer1.resolution, layer1.resolution]
		};

		for (int i = 0; i < layer1.resolution; i++)
			for (int j = 0; j < layer1.resolution; j++)
			{
				heightmap.data[i, j] = layer2.data[i, j];
			}

		return heightmap;
	}
	public static HeightmapData MultiplyBlend(HeightmapData layer1, HeightmapData layer2, float opacity = 1.0f)
	{
		HeightmapData heightmap = new HeightmapData()
		{
			resolution = layer1.resolution,
			data = new float[layer1.resolution, layer1.resolution]
		};

		for (int i = 0; i < layer1.resolution; i++)
			for (int j = 0; j < layer1.resolution; j++)
			{
				var result = layer1.data[i, j] * layer2.data[i, j];
				heightmap.data[i, j] = Mathf.Lerp(layer1.data[i, j], result, opacity);
			}

		return heightmap;
	}
	public static HeightmapData DarkenBlend(HeightmapData layer1, HeightmapData layer2, float opacity = 1.0f)
	{
		HeightmapData heightmap = new HeightmapData()
		{
			resolution = layer1.resolution,
			data = new float[layer1.resolution, layer1.resolution]
		};

		for (int i = 0; i < layer1.resolution; i++)
			for (int j = 0; j < layer1.resolution; j++)
			{
				var result = Mathf.Min(layer1.data[i, j], layer2.data[i, j]);
				heightmap.data[i, j] = Mathf.Lerp(layer1.data[i, j], result, opacity);
			}

		return heightmap;
	}
	public static HeightmapData LightenBlend(HeightmapData layer1, HeightmapData layer2, float opacity = 1.0f)
	{
		HeightmapData heightmap = new HeightmapData()
		{
			resolution = layer1.resolution,
			data = new float[layer1.resolution, layer1.resolution]
		};

		for (int i = 0; i < layer1.resolution; i++)
			for (int j = 0; j < layer1.resolution; j++)
			{
				var result = Mathf.Max(layer1.data[i, j], layer2.data[i, j]);
				heightmap.data[i, j] = Mathf.Lerp(layer1.data[i, j], result, opacity);
			}

		return heightmap;
	}
	public static HeightmapData Blend(HeightmapData layer1, HeightmapData layer2, BlendMode blendMode, float opacity = 1.0f)
	{
		return blendMode switch
		{
			BlendMode.Normal => NormalBlend(layer1, layer2),
			BlendMode.Multiply => MultiplyBlend(layer1, layer2, opacity),
			BlendMode.Darken => DarkenBlend(layer1, layer2, opacity),
			BlendMode.Lighten => LightenBlend(layer1, layer2, opacity),
			_ => null
		};
	}

	public static Texture2D GenerateTextureFromHeightmap(HeightmapData heightmap, int contrast = 1, TextureFormat format = TextureFormat.RGBAHalf)
	{
		Texture2D texture = new Texture2D(heightmap.resolution, heightmap.resolution, format, false);
		for (int i = 0; i < heightmap.resolution; i++)
			for (int j = 0; j < heightmap.resolution; j++)
			{
				var value = Mathf.Pow(heightmap.data[i, j], contrast);
				texture.SetPixel(i, j, new Color(value, value, value));
			}

		texture.Apply();

		return texture;
	}
}
