using UnityEngine;

namespace CommonUnit
{
    [CreateAssetMenu(fileName = "ProjectedOrbData", menuName = "Ye/ProjectedOrb/Create StageRewardField")]
    public class StageRewardField : FieldData
    {
        public int StoneValue;

        public override object getValue()
        {
            return StoneValue;
        }
    }
}
