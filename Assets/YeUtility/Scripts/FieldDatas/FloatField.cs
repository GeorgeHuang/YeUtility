using UnityEngine;

namespace CommonUnit
{
    [CreateAssetMenu(fileName = "ProjectedOrbData", menuName = "Ye/ProjectedOrb/Create FloadField")]
    public class FloatField : FieldData
    {
        public float value;
        public override object getValue()
        {
            return value;
        }
    }
}
