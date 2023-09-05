using System.Collections.Generic;
using UnityEngine;

namespace YeUtility
{
    [CreateAssetMenu(fileName = "ProjectedOrbData", menuName = "Ye/ProjectedOrb/Create LVField")]
    public class LVField : FieldData
    {
        public List<float> listValue;
        public CurveField curveFiled;

        public override object GetValueWithLv(int lv)
        {
            if (curveFiled != null) return curveFiled.GetValueWithLv(lv);
            if (lv > 0 && listValue.Count > lv - 1)
            {
                return listValue[lv - 1];
            }
            return 0;
        }
    }
}
