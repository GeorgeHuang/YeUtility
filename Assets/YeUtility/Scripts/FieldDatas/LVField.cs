using System.Collections.Generic;
using UnityEngine;

namespace CommonUnit
{
    [CreateAssetMenu(fileName = "ProjectedOrbData", menuName = "Ye/ProjectedOrb/Create LVField")]
    public class LVField : FieldData
    {
        public List<float> listValue;
        public CurveField curveFiled;

        public override object getValueWithLv(int lv)
        {
            if (curveFiled != null) return curveFiled.getValueWithLv(lv);
            if (lv > 0 && listValue.Count > lv - 1)
            {
                return listValue[lv - 1];
            }
            return 0;
        }
    }
}
