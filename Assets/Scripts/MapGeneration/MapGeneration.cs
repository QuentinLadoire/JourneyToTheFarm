using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapGeneration : MonoBehaviour
{
	public int mapSize = 256;

	public MapSetting mapSetting = null;

	MeshFilter meshFilter = null;
	MeshRenderer meshRenderer = null;

	int GetVerticesCount()
	{
		return mapSize * mapSize;
	}
	int GetTrianglesCount()
	{
		return (mapSize - 1) * (mapSize - 1) * 2;
	}
	int GetIndicesCount()
	{
		return (mapSize - 1) * (mapSize - 1) * 2 * 3;
	}

	float GetGroundHeight(float x, float y)
	{
		var setting = mapSetting.groundNoiseSetting;
		return NoiseUtility.FractalNoise2D(x, y, setting) * mapSetting.groundHeightMultiplier;
	}
	float GetMoutainHeight(float x, float y)
	{
		var mountainNoiseSetting = mapSetting.mountainNoiseSetting;
		return Mathf.Clamp01(NoiseUtility.FractalNoise2D(x, y, mountainNoiseSetting)) * mapSetting.mountainHeightMultiplier;
	}
	float GetHeight(float x, float y)
	{
		return GetGroundHeight(x, y);/* + GetMoutainHeight(x, y);*/
	}

	Mesh GenerateMesh()
	{
		Vector3[] vertices = new Vector3[GetVerticesCount()];
		int[] indices = new int[GetIndicesCount()];
		Vector2[] uvs = new Vector2[GetVerticesCount()];

		int index = 0;
		for (int i = 0; i < GetVerticesCount(); i++)
		{
			var x = i % mapSize;
			var z = i / mapSize;
			var y = GetHeight(transform.position.x + x, transform.position.z + z);

			vertices[i] = new Vector3(x, y, z);

			var uvX = (float)((float)x / (float)mapSize);
			var uvY = (float)((float)z / (float)mapSize);
			uvs[i] = new Vector2(uvX, uvY);
			
			if (x < (mapSize - 1))
			{
				if (z < (mapSize - 1))
				{
					indices[index] = i;
					index++;

					indices[index] = i + mapSize;
					index++;

					indices[index] = i + 1;
					index++;
				}
				if (z > 0)
				{
					indices[index] = i;
					index++;
				
					indices[index] = i + 1;
					index++;
				
					indices[index] = i + 1 - mapSize;
					index++;
				}
			}
		}

		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = indices;
		mesh.uv = uvs;

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
		meshFilter.mesh = GenerateMesh();

		var input = new Vector2(transform.position.x, transform.position.z);

		var mat = new Material(Shader.Find("Shader Graphs/Test"));
		mat.SetTexture("Texture2D_dcc01194ebed4494a1e3bbc386c54016", NoiseUtility.GenerateFractalNoiseTexture(input, mapSetting.groundNoiseSetting, 3));
		meshRenderer.material = mat;
	}
}
