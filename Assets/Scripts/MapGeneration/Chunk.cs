using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
	public const int chunkSize = 256;
	public int VerticesCount => chunkSize * chunkSize;
	public int TrianglesCount => (chunkSize - 1) * (chunkSize - 1) * 2;
	public int IndicesCount => (chunkSize - 1) * (chunkSize - 1) * 2 * 3;

	[HideInInspector]
	public MapGenerationSetting setting = null;

	MeshFilter meshFilter = null;
	MeshRenderer meshRenderer = null;

	HeightmapData heightmap = null;

	float GetHeight(int x, int y)
	{
		return heightmap.data[x, y] * setting.heightMultiplier - setting.heightMultiplier * 0.5f;
	}

    void ComputeHeightmap()
	{
		if (setting == null)
			return;

		var offset = new Vector2(transform.position.x, transform.position.z);
		heightmap = setting.ComputeHeightmap(offset);
	}
	void GenerateMesh()
	{
		if (heightmap == null)
			return;

		Vector3[] vertices = new Vector3[VerticesCount];
		int[] indices = new int[IndicesCount];
		Vector2[] uvs = new Vector2[VerticesCount];

		int index = 0;
		for (int i = 0; i < VerticesCount; i++)
		{
			var x = i % chunkSize;
			var z = i / chunkSize;
			var y = GetHeight(x, z);

			#region SetPosition
			vertices[i] = new Vector3(x, y, z);
			#endregion

			#region SetUVs
			var uvX = (float)((float)x / (float)chunkSize);
			var uvY = (float)((float)z / (float)chunkSize);
			uvs[i] = new Vector2(uvX, uvY);
			#endregion

			#region Setindices
			if (x < (chunkSize - 1))
			{
				if (z < (chunkSize - 1))
				{
					indices[index] = i;
					index++;

					indices[index] = i + chunkSize;
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

					indices[index] = i + 1 - chunkSize;
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
		if (heightmap == null)
			return;

		var mat = new Material(meshRenderer.material);
		mat.SetTexture("Texture2D_dcc01194ebed4494a1e3bbc386c54016", HeightmapUtility.GenerateTextureFromHeightmap(heightmap));
		meshRenderer.material = mat;
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
