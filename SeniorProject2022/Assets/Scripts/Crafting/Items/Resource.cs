using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Crafting {
    [CreateAssetMenu(fileName = "Resource", menuName = "ScriptableObjects/Items/Resource")]
    public class Resource : Item
    {
        [Tooltip("Prefab that gets spawned when the resources is made.")]
        public GameObject prefab;
    }
}