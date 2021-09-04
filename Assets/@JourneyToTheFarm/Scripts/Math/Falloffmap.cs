using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalloffmapData
{
    public int resolution = 256;
    public float[,] data = null;
}

public class FalloffmapUtility
{
    private static float Evaluate(float value)
	{
        float pow = 3;
        float b = 2.2f;
        var powValue = Mathf.Pow(value, pow);

        return powValue / (powValue + Mathf.Pow((b - b * value), pow));
	}

    public static Texture2D GenerateTextureFromFalloffMap(FalloffmapData falloffMap)
	{
        Texture2D texture = new Texture2D(falloffMap.resolution, falloffMap.resolution);

        for (int i = 0; i < falloffMap.resolution; i++)
            for (int j = 0; j < falloffMap.resolution; j++)
			{
                var value = falloffMap.data[i, j];
                texture.SetPixel(i, j, new Color(value, value, value));
			}

        texture.Apply();

        return texture;
	}

    public static FalloffmapData GenerateSquareFalloffMap(int resolution)
	{
        FalloffmapData falloffMap = new FalloffmapData
        {
            resolution = resolution,
            data = new float[resolution, resolution]
        };

        var halfSize = resolution / 2.0f;

        for (int i = 0; i < resolution; i++)
            for (int j = 0; j < resolution; j++)
			{
                var x = Mathf.Abs((i - halfSize) / halfSize);
                var y = Mathf.Abs((j - halfSize) / halfSize);

                var value = Mathf.Max(x, y);

                falloffMap.data[i, j] = Evaluate(value);
			}

        return falloffMap;
    }
    public static FalloffmapData GenerateCircleFalloffMap(int resolution)
	{
        FalloffmapData falloffMap = new FalloffmapData
        {
            resolution = resolution,
            data = new float[resolution, resolution]
        };

        var halfSize = resolution / 2.0f;

        for (int i = 0; i < resolution; i++)
            for (int j = 0; j < resolution; j++)
			{
                float x = i - halfSize;
                float y = j - halfSize;

                var sqrtLenght = x * x + y * y;
                var value = sqrtLenght / (halfSize * halfSize);

                falloffMap.data[i, j] = Evaluate(value);
            }

        return falloffMap;
	}
}

