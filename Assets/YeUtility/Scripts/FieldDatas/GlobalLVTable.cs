using System.Collections.Generic;
using CommonUnit;
using UnityEngine;

namespace YeUtility
{
    [CreateAssetMenu(fileName = "ProjectedOrbData", menuName = "Ye/ProjectedOrb/Create GlobalLVTable")]
    public class GlobalLVTable : LVField
    {
        public int MaxLV = 0;
        public int initValue = 1;
        public List<int> CostTable;
        public override System.Object GetCostWithLv(int lv)
        {
            return CostTable[lv - 1];
        }

        public object GetMaxLV()
        {
            return MaxLV;
        }

        public override object InitValue()
        {
            return initValue;
        }
    }
}
