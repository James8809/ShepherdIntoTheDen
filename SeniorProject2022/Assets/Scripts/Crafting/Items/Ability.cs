using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crafting {
    [CreateAssetMenu(fileName = "Ability", menuName = "ScriptableObjects/Items/Ability")]
    public class Ability : Item
    {
        public bool canKeyMap;
        public string abilityName;
    }
}
