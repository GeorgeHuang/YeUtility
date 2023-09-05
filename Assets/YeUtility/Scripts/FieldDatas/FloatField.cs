using UnityEngine;

namespace YeUtility
{
    [CreateAssetMenu(fileName = "ProjectedOrbData", menuName = "Ye/ProjectedOrb/Create FloadField")]
    public class FloatField : FieldData
    {
        public float value;
        public override object GetValue()
        {
            return value;
        }
    }
}
