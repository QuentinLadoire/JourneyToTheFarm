using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapGenerationSetting
{
	[Header("Ground Setting")]
	public SimpleColorSetting flatGroundSetting = new SimpleColorSetting
	{
		grayValue = 0.58f
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

	[Header("Tree Setting")]
	public FractalNoiseSetting treesSetting = new FractalNoiseSetting
	{
		tiling = new Vector2(100.0f, 100.0f),
		layer = 3,
		persistance = 0.05f
	};
	[Range(0.0f, 1.0f)] public float treeHeight = 0.2f;

	public HeightmapData ComputeHeightmap(Vector2 offset)
	{
		var flatHeightmap = HeightmapUtility.GenerateHeightmapFrom(flatGroundSetting);
		var peaksHeightmap = HeightmapUtility.GenerateHeightmapFrom(offset, peaksSetting);
		var valleysHeightmap = HeightmapUtility.GenerateHeightmapFrom(offset, valleysSetting);

		var heightmap = HeightmapUtility.LightenBlend(flatHeightmap, peaksHeightmap, peaksOpacity);
		heightmap = HeightmapUtility.MultiplyBlend(heightmap, valleysHeightmap, valleysOpacity);

		return heightmap;
	}
	public HeightmapData ComputeTreeHeightmap(Vector2 offset)
	{
		return HeightmapUtility.GenerateHeightmapFrom(offset, treesSetting);
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
