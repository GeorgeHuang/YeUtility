using UnityEngine;

namespace CommonUnit
{
    [CreateAssetMenu(fileName = "ProjectedOrbData", menuName = "Ye/ProjectedOrb/Create CurveField")]
    public class CurveField : FieldData
    {
        public AnimationCurve value;
        public override object getValue()
        {
            return value;
        }

        public override object getValueWithLv(int lv)
        {
            return value.Evaluate((float)lv);
        }
    }
}
