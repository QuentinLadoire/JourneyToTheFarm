using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.Behaviour;
using MLAPI.NetworkVariable;

namespace JTTF
{
    public class TreeState : CustomNetworkBehaviour
    {
        private NetworkVariableBool isHarvestedSync = new NetworkVariableBool(new NetworkVariableSettings
        {
            ReadPermission = NetworkVariablePermission.Everyone,
            WritePermission = NetworkVariablePermission.ServerOnly
        }, false);

        public NetworkVariableBool IsHarvestedSync => isHarvestedSync;
    }
}
