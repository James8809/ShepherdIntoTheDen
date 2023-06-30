using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Crafting {
    [CreateAssetMenu(fileName = "Crest", menuName = "ScriptableObjects/Items/Crest")]
    public class Crest : CraftableItem
    {
        // Since there won't be a lot of abilities, kinda just hard coding it
        public enum Abilities{
            Stomp,
            Summon,
            FireTrail,
            SuperDash,
            Sword,
            Stick,
        }

        public Abilities ability;

        public Ability abilityObj;
    }
}
