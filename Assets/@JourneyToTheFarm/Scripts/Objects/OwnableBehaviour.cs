using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.Character;
using JTTF.Interface;

namespace JTTF.Behaviour
{
	public class OwnableBehaviour : CustomBehaviour, IOwnable
	{
		private Player ownerPlayer = null;

		public Player OwnerPlayer => ownerPlayer;

		public void SetOwner(Player owner)
		{
			ownerPlayer = owner;
		}
	}
}
