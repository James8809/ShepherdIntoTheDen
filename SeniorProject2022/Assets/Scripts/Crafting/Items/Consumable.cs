using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Audio;

namespace Crafting {
    [CreateAssetMenu(fileName = "Consumable", menuName = "ScriptableObjects/Items/Consumable")]
    public class Consumable : CraftableItem
    {

        public virtual void Consume(PlayerController playerController)
        {
        }
    }
}
