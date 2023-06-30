using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManaSystem : MonoBehaviour
{
    public int baseMana = 25;
    private int currentMana;
    public int secondsPerManaRegen = 1;

    public Action<int, int, int> onManaChanged;
    public Action<int, int> onManaInitialized;

    private void Awake()
    {
        currentMana = baseMana;
        StartCoroutine(RegenerateMana());
    }

    IEnumerator RegenerateMana()
    {
        while (true)
        {
            if (currentMana < baseMana)
            {
                RestoreMana(1);
                yield return new WaitForSeconds(1);
            }
            else
                yield return null;
        }

    }

    private void Start()
    {
        onManaInitialized(currentMana, baseMana);
    }

    public bool UseMana(int amount)
    {
        if (currentMana - amount < 0)
            return false;
        currentMana -= amount;
        Debug.Log(amount);
        onManaChanged(currentMana, baseMana, -amount);
        return true;
    }

    public void RestoreMana(int amount)
    {
        currentMana = Mathf.Min(currentMana + amount, baseMana);
        onManaChanged(currentMana, baseMana, amount);
    }

    public void Die()
    {
        SceneManager.LoadScene("DeathScreen");
    }
    public void ResetMana()
    {
        currentMana = baseMana;
    }

}
