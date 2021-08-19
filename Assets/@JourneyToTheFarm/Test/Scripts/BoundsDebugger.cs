using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public class BoundsDebugger : MonoBehaviour
    {
		private void OnDrawGizmos()
		{
			var meshRenderer = GetComponent<MeshRenderer>();
			if (meshRenderer != null)
			{
				Gizmos.color = Color.cyan;
				Gizmos.DrawWireCube(meshRenderer.bounds.center, meshRenderer.bounds.size);
			}
		}
	}
}
