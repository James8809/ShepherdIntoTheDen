using System.Collections;
using System.Collections.Generic;
using Crafting;
using UnityEngine;

namespace Crafting {
    [CreateAssetMenu(fileName = "StatBooster", menuName = "ScriptableObjects/Items/Consumables/StatBooster")]
    public class StatBooster : Consumable
    {
        public StatType statType;
        [Tooltip("Integer values for health (ex. 5, 10), decimal values for Attack and Speed buffs (1.5 => 1.5x attack)")]
        public float value;
        [Tooltip("How long the stat buff will be available for IN SECONDS. -1 if doesn't go away.")]
        public float duration;

        public Sprite statIcon;
        public Color circleIconColor;

        public enum StatType
        {
            Health,
            Attack,
            Speed,
            Defense,
            Mana
        };

        public override void Consume(PlayerController playerController)
        {
            switch (statType)
            {
                case StatType.Health:
                    playerController.playerHealthSystem.RestoreHealth((int)value);
                    break;
                case StatType.Attack:
                    playerController.playerStats.SetAttackMultiplier(this);
                    break;
                case StatType.Defense:
                    playerController.playerStats.SetDamageTakenMultiplier(this);
                    break;
                case StatType.Speed:
                    playerController.playerStats.SetSpeedMultiplier(this);
                    break;
                case StatType.Mana:
                    playerController.playerManaSystem.RestoreMana((int)value);
                    break;
            }
        }
    }
}
