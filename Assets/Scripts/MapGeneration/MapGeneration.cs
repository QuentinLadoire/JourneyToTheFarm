using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
	public int squareSize = 1;

	public MapSetting mapSetting = null;

	MeshFilter meshFilter = null;
	MeshRenderer meshRenderer = null;

	int GetVerticesCount(int size)
	{
		return (size + 1) * (size + 1);
	}
	int GetTrianglesCount(int size)
	{
		return size * size * 2;
	}
	int GetIndicesCount(int size)
	{
		return size * size * 2 * 3;
	}

	float GetGroundHeight(float x, float y)
	{
		var groundNoiseSetting = mapSetting.groundNoiseSetting;
		return NoiseUtility.CoherentNoise2D(x, y, groundNoiseSetting) * mapSetting.groundHeightMultiplier;
	}
	float GetMoutainHeight(float x, float y)
	{
		var mountainNoiseSetting = mapSetting.mountainNoiseSetting;
		return Mathf.Clamp01(NoiseUtility.CoherentNoise2D(x, y, mountainNoiseSetting)) * mapSetting.mountainHeightMultiplier;
	}
	float GetHeight(float x, float y)
	{
		return GetGroundHeight(x, y)  + GetMoutainHeight(x, y);
	}

	Mesh GenerateMesh(int squareSize)
	{
		int size = squareSize + 1;
		Vector3[] vertices = new Vector3[GetVerticesCount(squareSize)];
		int[] indices = new int[GetIndicesCount(squareSize)];
		Vector2[] uvs = new Vector2[GetVerticesCount(squareSize)];

		int index = 0;
		for (int i = 0; i < GetVerticesCount(squareSize); i++)
		{
			var height = GetHeight(transform.position.x + i % size, transform.position.z + i / size);
			vertices[i] = new Vector3((i % size), height, (i / size));

			uvs[i] = new Vector2((float)((float)(i % size) / (float)size), (float)((float)(i / size) / (float)size));

			if ((i % size) < (size - 1))
			{
				if ((i / size) < (size - 1))
				{
					indices[index] = i;
					index++;

					indices[index] = i + squareSize + 1;
					index++;

					indices[index] = i + 1;
					index++;
				}
				if ((i / size) > 0)
				{
					indices[index] = i;
					index++;

					indices[index] = i + 1;
					index++;

					indices[index] = i - squareSize;
					index++;
				}
			}
		}

		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = indices;

		mesh.RecalculateNormals();

		return mesh;
	}

	private void Awake()
	{
		meshFilter = GetComponent<MeshFilter>();
		meshRenderer = GetComponent<MeshRenderer>();
	}
	private void Start()
	{
		meshFilter.mesh = GenerateMesh(squareSize);
	}
}
