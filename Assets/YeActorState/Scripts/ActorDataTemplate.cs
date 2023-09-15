using System;
using System.Collections.Generic;
using CommonUnit;
using Sirenix.OdinInspector;
using UnityEngine;

namespace YeActorState
{
    [Serializable]
    public class ActorDataTemplate : NamedObject
    {
        [ValueDropdown("@EditorHelper.PropertyNames", IsUniqueList = true)]
        public List<string> propertyNames;
    }
}