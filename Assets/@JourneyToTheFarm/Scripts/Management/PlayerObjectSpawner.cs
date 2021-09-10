using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

namespace JTTF
{
    public class PlayerObjectSpawner : CustomBehaviour
    {
		protected override void Start()
		{
			base.Start();

			World.SpawnPlayerObject(transform.position, transform.rotation);

			Destroy();
		}
	}
}
