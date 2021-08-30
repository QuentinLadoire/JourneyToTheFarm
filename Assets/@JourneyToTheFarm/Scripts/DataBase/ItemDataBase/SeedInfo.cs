using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0659
#pragma warning disable 0661

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

		public override bool Equals(object obj)
		{
			return obj is SeedInfo info &&
				   name == info.name &&
				   growDuration == info.growDuration;
		}

		public static bool operator ==(SeedInfo left, SeedInfo right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(SeedInfo left, SeedInfo right)
		{
			return !(left == right);
		}
	}
}
