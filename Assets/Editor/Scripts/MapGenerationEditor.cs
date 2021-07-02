using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGeneration))]
public class MapGenerationEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Settings"))
		{
			if ((target as MapGeneration).mapSetting != null)
				MapGenerationWindow.OpenWindow(AssetDatabase.GetAssetPath((target as MapGeneration).mapSetting.GetInstanceID()));
			else
				MapGenerationWindow.OpenWindow();
		}
	}
}
