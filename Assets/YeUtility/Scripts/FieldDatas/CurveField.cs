using UnityEngine;

namespace YeUtility
{
    [CreateAssetMenu(fileName = "ProjectedOrbData", menuName = "Ye/ProjectedOrb/Create CurveField")]
    public class CurveField : FieldData
    {
        public AnimationCurve value;
        public override object GetValue()
        {
            return value;
        }

        public override object GetValueWithLv(int lv)
        {
            return value.Evaluate((float)lv);
        }
    }
}
