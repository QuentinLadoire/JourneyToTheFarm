using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public class PhotoBooth : MonoBehaviour
    {
		public float dephtPosition = -10;
		public MeshRenderer quadMeshRenderer = null;

		Organizer organizer = null;
		RenderTexture renderTexture = null;

		void PlaceCamera()
		{
			var center = organizer.GetCenterPosition();
			Camera.main.transform.position = new Vector3(center.x, center.y, dephtPosition);
			Camera.main.orthographicSize = organizer.GetSize().x * 0.5f;
		}

		private void OnValidate()
		{
			organizer = GetComponent<Organizer>();
			renderTexture = new RenderTexture(256 * organizer.columnCount, 256 * organizer.columnCount, 24);
			quadMeshRenderer.sharedMaterial.mainTexture = renderTexture;
			Camera.main.targetTexture = renderTexture;

			PlaceCamera();
		}

		public void GenerateIcons()
		{
			RenderTexture.active = renderTexture;
			Camera.main.Render();
			Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height);
			texture.ReadPixels(new Rect(0.0f, 0.0f, texture.width, texture.height), 0, 0);

			for (int i = 0; i < texture.width; i++)
				for (int j = 0; j < texture.height; j++)
				{
					var color = texture.GetPixel(i, j);
					if (color == Color.green)
					{
						texture.SetPixel(i, j, Color.clear);
					}
				}

			texture.Apply();

			var bytes = texture.EncodeToPNG();
			var path = Application.dataPath + "/@JourneyToTheFarm/GameAssets/Textures/Icons/Icons.png";
			System.IO.File.WriteAllBytes(path, bytes);
		}
	}
}
