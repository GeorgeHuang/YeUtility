using UnityEngine;
using UnityEngine.Serialization;

namespace YeUtility
{
    [RequireComponent(typeof(YeHeavyRotation))]
    public class YeHeavyRotationUpdater : MonoBehaviour
    {
        [FormerlySerializedAs("heavyRotation")] [SerializeField]
        private YeHeavyRotation yeHeavyRotation;

        private void Update()
        {
            yeHeavyRotation.UpdateRotation(Time.deltaTime);
        }
    }
}