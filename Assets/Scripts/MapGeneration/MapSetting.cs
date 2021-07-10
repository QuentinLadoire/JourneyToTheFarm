using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMapSetting", menuName = "MapSetting")]
public class MapSetting : ScriptableObject
{
	[Header("GroundNoiseMap Setting")]
	public NoiseSetting groundNoiseSetting = new NoiseSetting();
	public float groundHeightMultiplier = 15.0f;

	[Header("MountainNoiseMap Setting")]
	public NoiseSetting mountainNoiseSetting = new NoiseSetting();
	public float mountainHeightMultiplier = 100.0f;
}
