using UnityEngine;
using YeUtility;

namespace ActorStateTest.Data
{
    [CreateAssetMenu(fileName = "ActorDataRepo", menuName = "Tools/So/Actor Data Repo", order = 0)]
    public class ActorDataRepo : ScriptableObjectRepo<ActorDataRepo, ActorData>
    {
        
    }
}