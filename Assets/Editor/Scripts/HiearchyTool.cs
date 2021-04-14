using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class HiearchyTool
{
	[MenuItem("GameObject/SortSelection/Alpha A-Z", priority = 0)]
	public static void SortSelection()
	{
		var gameObjects = Selection.gameObjects;
		gameObjects = gameObjects.OrderBy(obj => obj.name).ToArray();

		for (int i = 0; i < gameObjects.Length; i++)
			gameObjects[i].transform.SetSiblingIndex(i);
	}
}
