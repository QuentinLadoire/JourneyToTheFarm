using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	[System.Serializable]
	public struct SeedInfo
	{
		public static SeedInfo None = new SeedInfo("NoName", 0.0f, null, null);

		public string name;
		public float growDuration;
		public GameObject seedPreviewPrefab;
		public GameObject[] seedStepPrefabs;

		public SeedInfo(string name, float growDuration, GameObject seedPreviewPrefab, GameObject[] seedStepPrefabs)
		{
			this.name = name;
			this.growDuration = growDuration;
			this.seedPreviewPrefab = seedPreviewPrefab;
			this.seedStepPrefabs = seedStepPrefabs;
		}
	}
}
