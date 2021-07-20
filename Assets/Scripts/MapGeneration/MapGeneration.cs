using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
	public int mapSize = 256;
	public int VerticesCount => mapSize * mapSize;
	public int TrianglesCount => (mapSize - 1) * (mapSize - 1) * 2;
	public int IndicesCount => (mapSize - 1) * (mapSize - 1) * 2 * 3;

	public SimpleColorSetting flatGroundSetting = new SimpleColorSetting
	{
		grayValue = 0.58f
	};
	public FractalNoiseSetting peaksSetting = new FractalNoiseSetting
	{
		tiling = new Vector2(10.0f, 10.0f)
	};
	[Range(0.0f, 1.0f)] public float peaksOpacity = 0.2f;
	public FractalNoiseSetting valleysSetting = new FractalNoiseSetting
	{
		tiling = new Vector2(10.0f, 10.0f)
	};
	[Range(0.0f, 1.0f)] public float valleysOpacity = 0.1f;

	public float heightMultiplier = 100.0f;

	public int textureContrast = 1;

	HeightmapData heightmap = null;

	MeshFilter meshFilter = null;
	MeshRenderer meshRenderer = null;

	float GetHeight(float x, float y)
	{
		return heightmap.data[(int)x, (int)y] * heightMultiplier - heightMultiplier * 0.75f;
	}

	void ComputeHeightmap()
	{
		var flatHeightmap = HeightmapUtility.GenerateHeightmapFrom(flatGroundSetting);
		var peaksHeightmap = HeightmapUtility.GenerateHeightmapFrom(peaksSetting);
		var valleysHeightmap = HeightmapUtility.GenerateHeightmapFrom(valleysSetting);

		heightmap = HeightmapUtility.LightenBlend(flatHeightmap, peaksHeightmap, peaksOpacity);
		heightmap = HeightmapUtility.MultiplyBlend(heightmap, valleysHeightmap, valleysOpacity);
	}
	void GenerateMesh()
	{
		Vector3[] vertices = new Vector3[VerticesCount];
		int[] indices = new int[IndicesCount];
		Vector2[] uvs = new Vector2[VerticesCount];

		int index = 0;
		for (int i = 0; i < VerticesCount; i++)
		{
			var x = i % mapSize;
			var z = i / mapSize;
			var y = GetHeight(x, z);

			#region SetPosition
			vertices[i] = new Vector3(x, y, z);
			#endregion

			#region SetUVs
			var uvX = (float)((float)x / (float)mapSize);
			var uvY = (float)((float)z / (float)mapSize);
			uvs[i] = new Vector2(uvX, uvY);
			#endregion

			#region Setindices
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
			#endregion
		}

		Mesh mesh = new Mesh()
		{
			vertices = vertices,
			triangles = indices,
			uv = uvs,
		};

		mesh.RecalculateNormals();

		meshFilter.mesh = mesh;
	}
	void GenerateTexture()
	{
		meshRenderer.sharedMaterial.SetTexture("Texture2D_dcc01194ebed4494a1e3bbc386c54016", HeightmapUtility.GenerateTextureFromHeightmap(heightmap, textureContrast));
	}

	private void OnValidate()
	{
		if (UnityEditor.EditorApplication.isPlaying)
		{
			meshFilter = GetComponent<MeshFilter>();
			meshRenderer = GetComponent<MeshRenderer>();

			ComputeHeightmap();
			GenerateMesh();
			GenerateTexture();
		}
	}

	private void Awake()
	{
		meshFilter = GetComponent<MeshFilter>();
		meshRenderer = GetComponent<MeshRenderer>();
	}
	private void Start()
	{
		ComputeHeightmap();
		GenerateMesh();
		GenerateTexture();
	}
}
