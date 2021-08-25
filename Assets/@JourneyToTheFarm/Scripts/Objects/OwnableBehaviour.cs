using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class OwnableBehaviour : CustomBehaviour, IOwnable
	{
		Player ownerPlayer = null;

		public Player OwnerPlayer => ownerPlayer;

		public void SetOwner(Player owner)
		{
			ownerPlayer = owner;
		}
	}
}
