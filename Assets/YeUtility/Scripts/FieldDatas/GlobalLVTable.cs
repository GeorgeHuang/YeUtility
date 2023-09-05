using System.Collections.Generic;
using UnityEngine;

namespace CommonUnit
{
    [CreateAssetMenu(fileName = "ProjectedOrbData", menuName = "Ye/ProjectedOrb/Create GlobalLVTable")]
    public class GlobalLVTable : LVField
    {
        public int MaxLV = 0;
        public int initValue = 1;
        public List<int> CostTable;
        public override System.Object getCostWithLv(int lv)
        {
            return CostTable[lv - 1];
        }

        public override object getMaxLV()
        {
            return MaxLV;
        }

        public override object InitValue()
        {
            return initValue;
        }
    }
}
