using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable IDE0079
#pragma warning disable IDE0090
#pragma warning disable CS0659
#pragma warning disable CS0661

namespace JTTF.DataBase
{
    [System.Serializable]
    public struct SeedAsset
    {
        public string name;
        public float growDuration;
        public GameObject seedPreviewPrefab;
        public GameObject[] seedStepPrefabs;

		public SeedAsset(string name, float growDuration, GameObject seedPreviewPrefab, GameObject[] seedStepPrefabs)
		{
			this.name = name;
			this.growDuration = growDuration;
			this.seedPreviewPrefab = seedPreviewPrefab;
			this.seedStepPrefabs = seedStepPrefabs;
		}

		public override bool Equals(object obj)
		{
			return obj is SeedAsset info &&
				   name == info.name &&
				   growDuration == info.growDuration;
		}

		public static SeedAsset None = new SeedAsset("NoName", 0.0f, null, null);

		public static bool operator ==(SeedAsset left, SeedAsset right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(SeedAsset left, SeedAsset right)
		{
			return !(left == right);
		}
	}
}
