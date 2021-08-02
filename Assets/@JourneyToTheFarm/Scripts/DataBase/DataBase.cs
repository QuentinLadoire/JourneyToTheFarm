using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    [CreateAssetMenu(fileName = "NewDataBase", menuName = "DataBase")]
    public class DataBase : ScriptableObject
    {
        public List<ItemAsset> toolItems = new List<ItemAsset>();
        public List<ItemAsset> seedItems = new List<ItemAsset>();
        public List<ItemAsset> ressourceItems = new List<ItemAsset>();
        public List<ItemAsset> containerItems = new List<ItemAsset>();
        public List<ItemAsset> workbenchItems = new List<ItemAsset>();

        ItemAsset GetToolItemAsset(string name)
		{
            foreach (var item in toolItems)
                if (item.name == name)
                    return item;

            return null;
		}
        ItemAsset GetSeedItemAsset(string name)
		{
            foreach (var item in seedItems)
                if (item.name == name)
                    return item;

            return null;
		}
        ItemAsset GetRessourceItemAsset(string name)
		{
            foreach (var item in ressourceItems)
                if (item.name == name)
                    return item;

            return null;
		}
        ItemAsset GetContainerItemAsset(string name)
		{
            foreach (var item in containerItems)
                if (item.name == name)
                    return item;

            return null;
		}
        ItemAsset GetWorkbenchItemAsset(string name)
		{
            foreach (var item in workbenchItems)
                if (item.name == name)
                    return item;

            return null;
		}
        public ItemAsset GetItemAsset(string name, ItemType type)
		{
            return type switch
            {
                ItemType.Tool => GetToolItemAsset(name),
                ItemType.Seed => GetSeedItemAsset(name),
                ItemType.Resource => GetRessourceItemAsset(name),
                ItemType.Container => GetContainerItemAsset(name),
                ItemType.Workbench => GetWorkbenchItemAsset(name),
                _ => null
            };
		}
    }
}
