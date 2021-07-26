using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerrainSetting
{
	[Header("Ground Setting")]
	public SimpleColorSetting flatGroundSetting = new SimpleColorSetting
	{
		grayValue = 0.5f
	};

	[Header("Peaks Setting")]
	public FractalNoiseSetting peaksSetting = new FractalNoiseSetting
	{
		tiling = new Vector2(1.0f, 1.0f),
		layer = 8
	};
	[Range(0.0f, 1.0f)] public float peaksOpacity = 0.45f;

	[Header("Valleys Setting")]
	public FractalNoiseSetting valleysSetting = new FractalNoiseSetting
	{
		tiling = new Vector2(1.0f, 1.0f),
		layer = 8
	};
	[Range(0.0f, 1.0f)] public float valleysOpacity = 0.05f;

	[Space]
	public float heightMultiplier = 1000.0f;
}

[System.Serializable]
public class TreeSetting
{
	public FractalNoiseSetting noiseSetting = new FractalNoiseSetting
	{
		tiling = new Vector2(100.0f, 100.0f),
		layer = 3,
		persistance = 0.05f
	};

	[Header("Tree_01")]
	public GameObject tree_01Prefab = null;
	public float tree_01Min = 0.0f;
	public float tree_01Max = 0.2f;

	[Header("Tree_02")]
	public GameObject tree_02Prefab = null;
	public float tree_02Min = 0.2f;
	public float tree_02Max = 0.24f;
}

[System.Serializable]
public class MapGenerationSetting
{
	public TerrainSetting terrainSetting = new TerrainSetting();
	public TreeSetting treeSetting = new TreeSetting();

	public HeightmapData ComputeHeightmap(Vector2 offset)
	{
		var flatHeightmap = HeightmapUtility.GenerateHeightmapFrom(terrainSetting.flatGroundSetting);
		var peaksHeightmap = HeightmapUtility.GenerateHeightmapFrom(offset, terrainSetting.peaksSetting);
		var valleysHeightmap = HeightmapUtility.GenerateHeightmapFrom(offset, terrainSetting.valleysSetting);

		var heightmap = HeightmapUtility.LightenBlend(flatHeightmap, peaksHeightmap, terrainSetting.peaksOpacity);
		heightmap = HeightmapUtility.MultiplyBlend(heightmap, valleysHeightmap, terrainSetting.valleysOpacity);

		return heightmap;
	}
	public HeightmapData ComputeTreeHeightmap(Vector2 offset)
	{
		return HeightmapUtility.GenerateHeightmapFrom(offset, treeSetting.noiseSetting);
	}
}

public class MapGeneration : MonoBehaviour
{
	public int mapSize = 5;
	public MapGenerationSetting setting = new MapGenerationSetting();
	public GameObject chunkPrefab = null;

	List<Chunk> chunkList = new List<Chunk>();

	void CreateNewChunk(int x, int y)
	{
		if (chunkPrefab == null)
			return;

		var newChunk = Instantiate(chunkPrefab).GetComponent<Chunk>();
		newChunk.transform.parent = transform;

		//-1 because mesh vertices are between 0 to chunkSize - 1, so we need to substract 1 to chunk collide between them
		newChunk.transform.position = new Vector3(x * (Chunk.chunkSize - 1), 0.0f, y * (Chunk.chunkSize - 1));

		newChunk.setting = setting;

		chunkList.Add(newChunk);
	}

	private void Awake()
	{
		for (int i = 0; i < mapSize; i++)
			for (int j = 0; j < mapSize; j++)
			{
				CreateNewChunk(i, j);
			}
	}
}
