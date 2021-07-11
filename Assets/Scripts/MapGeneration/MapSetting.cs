using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMapSetting", menuName = "MapSetting")]
public class MapSetting : ScriptableObject
{
	[Header("GroundNoiseMap Setting")]
	public FractalNoiseSetting groundNoiseSetting = new FractalNoiseSetting();
	public float groundHeightMultiplier = 15.0f;

	[Header("MountainNoiseMap Setting")]
	public FractalNoiseSetting mountainNoiseSetting = new FractalNoiseSetting();
	public float mountainHeightMultiplier = 100.0f;
}
