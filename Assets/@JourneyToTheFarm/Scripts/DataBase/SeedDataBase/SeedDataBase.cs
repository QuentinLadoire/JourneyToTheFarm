using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable IDE0090

namespace JTTF.DataBase
{
    [CreateAssetMenu(fileName = "NewSeedDataBase", menuName = "SeedDataBase")]
    public class SeedDataBase : ScriptableObject
    {
        public List<SeedAsset> seedAssets = new List<SeedAsset>();

        public SeedAsset GetSeedAsset(string name)
		{
            foreach (var seedAsset in seedAssets)
                if (seedAsset.name == name)
                    return seedAsset;

            return SeedAsset.None;
		}
    }
}
