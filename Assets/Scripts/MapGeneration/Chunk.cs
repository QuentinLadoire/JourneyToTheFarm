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
	MeshCollider meshCollider = null;

	HeightmapData heightmap = null;
	HeightmapData treeHeightmap = null;
	HeightmapData rockHeightmap = null;
	HeightmapData grassHeightmap = null;

	float GetHeight(int x, int y)
	{
		return heightmap.data[x, y] * setting.terrainSetting.heightMultiplier - setting.terrainSetting.heightMultiplier * 0.5f;
	}

	void CreateNewTree_01(int x, int y)
	{
		if (setting.treeSetting.tree_01Prefab == null)
			return;

		var newTreePrefab = Instantiate(setting.treeSetting.tree_01Prefab);
		newTreePrefab.transform.parent = transform;
		newTreePrefab.transform.localPosition = new Vector3(x, GetHeight(x, y), y);

		var randomAngle = Random.Range(0.0f, 360.0f);
		newTreePrefab.transform.eulerAngles = new Vector3(0.0f, randomAngle, 0.0f);

		var randomScale = Random.Range(2.0f, 4.0f);
		newTreePrefab.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
	}
	void CreateNewTree_02(int x, int y)
	{
		if (setting.treeSetting.tree_02Prefab == null)
			return;

		var newTreePrefab = Instantiate(setting.treeSetting.tree_02Prefab);
		newTreePrefab.transform.parent = transform;
		newTreePrefab.transform.localPosition = new Vector3(x, GetHeight(x, y), y);

		var randomAngle = Random.Range(0.0f, 360.0f);
		newTreePrefab.transform.eulerAngles = new Vector3(0.0f, randomAngle, 0.0f);

		var randomScale = Random.Range(2.0f, 4.0f);
		newTreePrefab.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
	}

	void CreateNewRock_05(int x, int y)
	{
		if (setting.rockSetting.rock_05Prefab == null)
			return;

		var newRockPrefab = Instantiate(setting.rockSetting.rock_05Prefab);
		newRockPrefab.transform.parent = transform;
		newRockPrefab.transform.localPosition = new Vector3(x, GetHeight(x, y), y);

		var randomAngle = Random.Range(0.0f, 360.0f);
		newRockPrefab.transform.eulerAngles = new Vector3(0.0f, randomAngle, 0.0f);

		var randomScale = Random.Range(5.0f, 15.0f);
		newRockPrefab.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
	}

	void CreateNewGrass_Patch_05(int x, int y)
	{
		if (setting.grassSetting.grass_Patch_02Prefab == null)
			return;

		var newRockPrefab = Instantiate(setting.grassSetting.grass_Patch_02Prefab);
		newRockPrefab.transform.parent = transform;
		newRockPrefab.transform.localPosition = new Vector3(x, GetHeight(x, y), y);

		var randomAngle = Random.Range(0.0f, 360.0f);
		newRockPrefab.transform.eulerAngles = new Vector3(0.0f, randomAngle, 0.0f);

		var randomScale = Random.Range(0.2f, 0.8f);
		newRockPrefab.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
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
		meshCollider.sharedMesh = mesh;
	}
	void GenerateTexture()
	{
		if (heightmap == null)
			return;

		var mat = new Material(meshRenderer.material);
		mat.SetTexture("Texture2D_dcc01194ebed4494a1e3bbc386c54016", HeightmapUtility.GenerateTextureFromHeightmap(heightmap));
		meshRenderer.material = mat;
	}
	void ComputeTreeHeightmap()
	{
		if (setting == null)
			return;

		var offset = new Vector2(transform.position.x, transform.position.z);
		treeHeightmap = setting.ComputeTreeHeightmap(offset);
	}
	void GenerateTree()
	{
		if (heightmap == null || treeHeightmap == null)
			return;

		for (int i = 0; i < treeHeightmap.resolution; i++)
			for (int j = 0; j < treeHeightmap.resolution; j++)
			{
				var height = treeHeightmap.data[i, j];
				if (heightmap.data[i, j] < 0.4875)//Grass Height
				{
					if (setting.treeSetting.tree_02Min < height && height < setting.treeSetting.tree_02Max)
						CreateNewTree_02(i, j);
				}
			}
	}
	void ComputeRockHeightmap()
	{
		if (setting == null)
			return;

		var offset = new Vector2(transform.position.x, transform.position.z);
		rockHeightmap = setting.ComputeRockHeightmap(offset);
	}
	void GenerateRock()
	{
		if (heightmap == null || rockHeightmap == null)
			return;

		for (int i = 0; i < rockHeightmap.resolution; i++)
			for (int j = 0; j < rockHeightmap.resolution; j++)
			{
				var height = rockHeightmap.data[i, j];
				if (heightmap.data[i, j] < 0.4875)//Grass Height
				{
					if (setting.rockSetting.rock_05Min < height && height < setting.rockSetting.rock_05Max)
						CreateNewRock_05(i, j);
				}
			}
	}
	void ComputeGrassHeightmap()
	{
		if (setting == null)
			return;

		var offset = new Vector2(transform.position.x, transform.position.z);
		grassHeightmap = setting.ComputeGrassHeightmap(offset);
	}
	void GenerateGrass()
	{
		if (heightmap == null || grassHeightmap == null)
			return;

		for (int i = 0; i < grassHeightmap.resolution; i++)
			for (int j = 0; j < grassHeightmap.resolution; j++)
			{
				var height = grassHeightmap.data[i, j];
				if (heightmap.data[i, j] < 0.4875)//Grass Height
				{
					if (setting.grassSetting.grass_Patch_02Min < height && height < setting.grassSetting.grass_Patch_02Max)
						CreateNewGrass_Patch_05(i, j);
				}
			}
	}

	private void Awake()
	{
		meshFilter = GetComponent<MeshFilter>();
		meshRenderer = GetComponent<MeshRenderer>();
		meshCollider = GetComponent<MeshCollider>();
	}
	private void Start()
	{
		Random.InitState((int)(transform.position.x + transform.position.y));

		ComputeHeightmap();
		GenerateMesh();

		GenerateTexture();

		ComputeTreeHeightmap();
		GenerateTree();

		ComputeRockHeightmap();
		GenerateRock();

		ComputeGrassHeightmap();
		GenerateGrass();
	}
}
