using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefabReplacer : EditorWindow
{
	static PrefabReplacer m_window = null;
	
	[MenuItem("Tools/PrefabReplacer")]
	public static void CreateWindow()
	{
		m_window = GetWindow<PrefabReplacer>();
		m_window.Show();
	}

	GameObject m_prefab = null;
	[SerializeField] List<GameObject> m_objectsToReplace = new List<GameObject>();

	bool m_folderHeaderOpen = false;
	bool m_useObjectScale = false;
	bool m_keepName = false;

	private void ReplaceObjects()
	{
		foreach (var obj in m_objectsToReplace)
		{
			GameObject newPrefab = null;
			if (PrefabUtility.GetPrefabAssetType(m_prefab) == PrefabAssetType.NotAPrefab)
			{
				newPrefab = Instantiate(m_prefab);
			}
			else
			{
				newPrefab = PrefabUtility.InstantiatePrefab(m_prefab) as GameObject;
			}

			if (newPrefab != null)
			{
				Undo.RegisterCreatedObjectUndo(newPrefab, "Created go");
				newPrefab.transform.position = obj.transform.position;
				newPrefab.transform.rotation = obj.transform.rotation;
				if (m_useObjectScale)
				{
					newPrefab.transform.localScale = obj.transform.localScale;
				}
				if (obj.transform.parent != null)
				{
					newPrefab.transform.parent = obj.transform.parent;
				}
				if (m_keepName)
				{
					newPrefab.gameObject.name = obj.gameObject.name;
				}
			}
		}
	}
	private void RemoveObjects()
	{
		for (int i = 0; i < m_objectsToReplace.Count; i++)
		{
			Undo.DestroyObjectImmediate(m_objectsToReplace[i]);
		}
		m_objectsToReplace.Clear();
	}


	private void Update()
	{
		m_objectsToReplace.RemoveAll(item => item == null);
	}
	private void OnGUI()
	{
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Prefabs");
		m_prefab = EditorGUILayout.ObjectField(m_prefab, typeof(GameObject), true) as GameObject;
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Separator();

		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("AddSelection"))
		{
			for (int i = 0; i < Selection.gameObjects.Length; i++)
			{
				if (!m_objectsToReplace.Contains(Selection.gameObjects[i]))
				{
					m_objectsToReplace.Add(Selection.gameObjects[i]);
				}
			}

			m_objectsToReplace.Sort((GameObject obj1, GameObject obj2) =>
			{
				return (obj1.name.CompareTo(obj2.name));
			});
		}
		if (GUILayout.Button("ClearSelection"))
		{
			m_objectsToReplace.Clear();
		}
		EditorGUILayout.EndHorizontal();

		m_folderHeaderOpen = EditorGUILayout.BeginFoldoutHeaderGroup(m_folderHeaderOpen, "ObjectsToReplace");
		if (m_folderHeaderOpen)
		{
			foreach (var obj in m_objectsToReplace)
			{
				EditorGUILayout.ObjectField(obj, typeof(GameObject), false);
			}
		}
		EditorGUILayout.EndFoldoutHeaderGroup();

		EditorGUILayout.Separator();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Use object Scale");
		m_useObjectScale = EditorGUILayout.Toggle(m_useObjectScale);

		GUILayout.Label("Keep same Name");
		m_keepName = EditorGUILayout.Toggle(m_keepName);
		EditorGUILayout.EndHorizontal();

		if (GUILayout.Button("Replace"))
		{
			ReplaceObjects();
			RemoveObjects();
		}
	}
}
