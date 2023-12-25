using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace YeUtility
{
    [RequireComponent(typeof(YeHeavyRotation))]
    public class YeHeavyRotationUpdater : MonoBehaviour
    {
        [SerializeField] private bool autoGet = true;
        [SerializeField] private YeHeavyRotation yeHeavyRotation;
        private void Awake()
        {
            if (yeHeavyRotation == null || autoGet)
            {
                yeHeavyRotation = GetComponent<YeHeavyRotation>();
            }
            
        }

        private void Update()
        {
            yeHeavyRotation?.UpdateRotation(Time.deltaTime);
        }
    }
}