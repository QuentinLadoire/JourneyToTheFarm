using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
	public int mapSize = 256;
	public int VerticesCount => mapSize * mapSize;
	public int TrianglesCount => (mapSize - 1) * (mapSize - 1) * 2;
	public int IndicesCount => (mapSize - 1) * (mapSize - 1) * 2 * 3;

	MeshFilter meshFilter = null;
	MeshRenderer meshRenderer = null;

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
			var y = 0.0f;

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

	private void Awake()
	{
		meshFilter = GetComponent<MeshFilter>();
		meshRenderer = GetComponent<MeshRenderer>();
	}
	private void Start()
	{
		GenerateMesh();
	}
}
