using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;

#pragma warning disable IDE0044
#pragma warning disable IDE0090

namespace JTTF
{
    public class FarmPlotState : CustomNetworkBehaviour
    {
        private NetworkVariableString seedNameSync = new NetworkVariableString(new NetworkVariableSettings
        {
            ReadPermission = NetworkVariablePermission.Everyone,
            WritePermission = NetworkVariablePermission.ServerOnly
        });
        private NetworkVariableFloat currentGrowDurationSync = new NetworkVariableFloat(new NetworkVariableSettings
        {
            ReadPermission = NetworkVariablePermission.Everyone,
            WritePermission = NetworkVariablePermission.ServerOnly
        });

        public NetworkVariableString SeedNameSync => seedNameSync;
        public NetworkVariableFloat CurrentGrowDurationSync => currentGrowDurationSync;
    }
}
