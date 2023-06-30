using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthSystem : MonoBehaviour, IDamageReciever
{
    public int baseHealth = 25;
    private int currentHealth = 25;

    public Action<int, int, int> onHealthChanged;
    public Action<int, int> onHealthInitialized;

    private PlayerStats playerStats;
    private PlayerController _playerController;
    Collider damageCollider;

    private void Awake()
    {
        currentHealth = baseHealth;
        playerStats = FindObjectOfType<PlayerStats>();
        _playerController = GetComponentInParent<PlayerController>();
        damageCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        onHealthInitialized(currentHealth, baseHealth);
    }

    private void OnTriggerEnter(Collider other)
    {
        // only weapons that can collide with this system should be registered
        var weapon = other.GetComponent<Weapon>();
        if (weapon == null)
        {
            return;
        }
        TakeDamage(weapon);
    }

    public void TakeDamage(Weapon weapon)
    {
        WeaponObject weaponObject = weapon.weaponType;
        // TODO: replace this with an event that occurs that updates the health info
        currentHealth -= Mathf.RoundToInt(weaponObject.GetDamage() * playerStats.damageTakenMultiplier);
        onHealthChanged(currentHealth, baseHealth, -weaponObject.GetDamage());
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void RestoreHealth(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, baseHealth);
        onHealthChanged(currentHealth, baseHealth, amount);
    }

    public void Die()
    {
        _playerController.Die();
        damageCollider.enabled = false;
    }

    public void ResetHealth()
    {
        currentHealth = baseHealth;
        onHealthChanged(currentHealth, baseHealth, currentHealth);
        damageCollider.enabled = true;
    }
}
