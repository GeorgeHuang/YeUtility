using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using YeUtility;

namespace YeActorState
{
    [Serializable]
    public class ActorDataTemplate : NamedObject
    {
        [ValueDropdown("@YeActorStateEditorHelper.PropertyNames", IsUniqueList = true)]
        public List<string> propertyNames;
    }
}