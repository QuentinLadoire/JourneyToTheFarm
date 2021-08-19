using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
	[ExecuteInEditMode]
	public class Organizer : MonoBehaviour
    {
		public Vector2 cellSize = Vector2.one;
		public Vector2 spacing = Vector2.zero;
		public int columnCount = 6;

		Vector2 GetOrganizedPosition(int index)
		{
			var x = cellSize.x * 0.5f + (index % columnCount) * (cellSize.x + spacing.x);
			var y = cellSize.y * 0.5f + (index / columnCount) * (cellSize.y + spacing.y);

			return new Vector2(x, y);
		}

		void Organize()
		{
			if (columnCount == 0) return;
			if (transform.childCount == 0) return;

			for (int i = 0; i < transform.childCount; i++)
			{
				var child = transform.GetChild(i);
				child.position = GetOrganizedPosition(i);

				var meshRenderer = child.GetComponent<MeshRenderer>();
				if (meshRenderer != null)
				{
					child.localScale = Vector3.one;

					var maxSize = meshRenderer.bounds.size.x > meshRenderer.bounds.size.y ? meshRenderer.bounds.size.x : meshRenderer.bounds.size.y;
					var scale = cellSize.y / maxSize;
					child.localScale = new Vector3(scale, scale, scale);

					child.position += child.position - meshRenderer.bounds.center;
					var position = child.position;
					position.z = 0.0f;
					child.position = position;
				}
			}
		}

		private void Update()
		{
			Organize();
		}
		private void OnDrawGizmos()
		{
			var rect = new Rect();
			rect.min = Vector2.zero;
			rect.max = (GetOrganizedPosition(columnCount * columnCount - 1) + cellSize * 0.5f);

			Gizmos.DrawLine(new Vector2(rect.xMin, rect.yMin), new Vector2(rect.xMax, rect.yMin));
			Gizmos.DrawLine(new Vector2(rect.xMax, rect.yMin), new Vector2(rect.xMax, rect.yMax));
			Gizmos.DrawLine(new Vector2(rect.xMax, rect.yMax), new Vector2(rect.xMin, rect.yMax));
			Gizmos.DrawLine(new Vector2(rect.xMin, rect.yMax), new Vector2(rect.xMin, rect.yMin));

			for (int i = 1; i < columnCount; i++)
			{
				var from = new Vector2(i * (cellSize.x + spacing.x) - (0.5f * spacing.x), 0.0f);
				var to = new Vector2(i * (cellSize.x + spacing.x) - (0.5f * spacing.x), rect.yMax);
				Gizmos.DrawLine(from, to);

				from = new Vector2(0.0f, i * (cellSize.y + spacing.y) - (0.5f * spacing.y));
				to = new Vector2(rect.xMax, i * (cellSize.y + spacing.y) - (0.5f * spacing.y));
				Gizmos.DrawLine(from, to);
			}

			Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
			for (int i = 0; i < columnCount * columnCount; i++)
			{
				var center = GetOrganizedPosition(i);
				Gizmos.DrawCube(center, cellSize);
			}
		}

		public Vector2 GetCenterPosition()
		{
			return (GetOrganizedPosition(columnCount * columnCount - 1) + cellSize * 0.5f) * 0.5f;
		}
		public Vector2 GetSize()
		{
			return GetOrganizedPosition(columnCount * columnCount - 1) + cellSize * 0.5f;
		}
	}
}
