using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class FarmPlot : MonoBehaviour
    {
        public bool HasSeed { get; private set; } = false;

        string seedName = "NoSeed";
        float growingDurationMax = 10.0f;

        public void SetSeed(string name, float growingDuration)
		{
            seedName = name;
            growingDurationMax = growingDuration;
            HasSeed = true;
		}
    }
}
