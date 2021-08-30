using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    [CreateAssetMenu(fileName = "NewPrefabDataBase", menuName = "PrefabDataBase")]
    public class PrefabDataBase : ScriptableObject
    {
        [SerializeField] private GameObject globalCanvasPrefab = null;
        [SerializeField] private GameObject mapGenerationPrefab = null;
        [SerializeField] private GameObject farmerControllerPrefab = null;
        [SerializeField] private GameObject cameraControllerPrefab = null;

        public GameObject GlobalCanvasPrefab => globalCanvasPrefab;
        public GameObject MapGenerationPrefab => mapGenerationPrefab;
        public GameObject FarmerControllerPrefab => farmerControllerPrefab;
        public GameObject CameraControllerPrefab => cameraControllerPrefab;
    }
}
