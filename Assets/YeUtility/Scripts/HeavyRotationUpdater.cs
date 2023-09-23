using System;
using UnityEngine;

namespace YeUtility.Scripts
{
    [RequireComponent(typeof(HeavyRotation))]
    public class HeavyRotationUpdater : MonoBehaviour
    {
        [SerializeField] private HeavyRotation heavyRotation;

        private void Update()
        {
            heavyRotation.UpdateRotation(Time.deltaTime);
        }
    }
}