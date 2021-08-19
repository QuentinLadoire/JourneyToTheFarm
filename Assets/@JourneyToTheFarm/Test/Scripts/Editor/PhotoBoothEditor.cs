using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Test
{
    [CustomEditor(typeof(PhotoBooth))]
    public class PhotoBoothEditor : Editor
    {
		PhotoBooth photoBooth = null;

		private void OnEnable()
		{
			photoBooth = target as PhotoBooth;
		}
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (GUILayout.Button("Generate Icons"))
			{
				photoBooth.GenerateIcons();
				AssetDatabase.Refresh();
			}
		}
	}
}
