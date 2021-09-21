using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.Behaviour;

namespace JTTF.Management
{
    public class PlayerObjectSpawner : CustomBehaviour
    {
		protected override void Start()
		{
			base.Start();

			World.SpawnPlayerObject(transform.position, transform.rotation);
			GameManager.DeactiveCursor();

			Destroy();
		}
	}
}
