using UnityEngine;

namespace Crafting
{
    [CreateAssetMenu(fileName = "Throwable", menuName = "ScriptableObjects/Items/Consumables/Throwable")]
    public class Throwable : Consumable
    {
        public enum ThrowableType
        {
            Dagger,
            Bomb
        };

        public ThrowableType type;
        public GameObject throwablePrefab;
        public override void Consume(PlayerController playerController)
        {
            playerController.TriggerConsumable(type, throwablePrefab);
        }
    }
}