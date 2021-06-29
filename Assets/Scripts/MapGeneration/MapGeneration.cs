using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
	[SerializeField] int squareSize = 1;

	MeshFilter meshFilter = null;

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

	Mesh GenerateMesh(int squareSize)
	{
		int size = squareSize + 1;
		Vector3[] vertices = new Vector3[GetVerticesCount(squareSize)];
		int[] indices = new int[GetIndicesCount(squareSize)];

		//Debug.Log("Square Size : " + squareSize);
		//Debug.Log("Size : " + size);
		//Debug.Log("Vertices Count : " + GetVerticesCount(squareSize));
		//Debug.Log("Triangles Count : " + GetTrianglesCount(squareSize));
		//Debug.Log("Indices Count : " + GetIndicesCount(squareSize));

		int index = 0;
		for (int i = 0; i < GetVerticesCount(squareSize); i++)
		{
			vertices[i] = new Vector3((i % size), 0.0f, (i / size));
			//Debug.Log("Vertice " + i + " : " + vertices[i]);

			if ((i % size) < (size - 1))
			{
				if ((i / size) < (size - 1))
				{
					indices[index] = i;
					//Debug.Log("Indice " + index + " : " + indices[index]);
					index++;

					indices[index] = i + squareSize + 1;
					//Debug.Log("Indice " + index + " : " + indices[index]);
					index++;

					indices[index] = i + 1;
					//Debug.Log("Indice " + index + " : " + indices[index]);
					index++;
				}
				if ((i / size) > 0)
				{
					indices[index] = i;
					//Debug.Log("Indice " + index + " : " + indices[index]);
					index++;

					indices[index] = i + 1;
					//Debug.Log("Indice " + index + " : " + indices[index]);
					index++;

					indices[index] = i - squareSize;
					//Debug.Log("Indice " + index + " : " + indices[index]);
					index++;
				}
			}
		}

		//Debug.Log("End");

		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = indices;

		return mesh;
	}

	private void Awake()
	{
		meshFilter = GetComponent<MeshFilter>();
	}
	private void Start()
	{
		meshFilter.mesh = GenerateMesh(squareSize);
	}
}
