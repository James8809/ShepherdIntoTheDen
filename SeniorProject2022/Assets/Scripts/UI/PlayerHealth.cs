using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{

    public int maxHealth;
    public int currentHealth;

    public HealthBarUI healthBar;

    public GameObject screenUI;
    public GameObject deathScreen;
    public GameObject winScreen;
    private Rigidbody playerRgbd;

    public int playerInvincibilityTimeSeconds = 2; //iframes after hit
    private bool isPlayerInvincible = false;

    // Start is called before the first frame update
    void Start()
    {
        playerRgbd = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        healthBar = FindObjectOfType<HealthBarUI>();
        healthBar.SetMaxHealth(maxHealth);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "DNR")
        {
            Debug.Log("Win");
            Time.timeScale = 0f;
            screenUI.SetActive(false);
            winScreen.SetActive(true);
        }
    }

    public void TakeDamage(Vector3 knockbackForce, int damage)
    {
        if (isPlayerInvincible)
            return;

        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        //playerRgbd.AddForce(knockbackForce, ForceMode.Impulse);

        if(currentHealth <= 0)
        {
            Debug.Log("DEATH");
            // Despawns persistent herd
            PlayerPrefs.SetInt("herdSize", 0);
            //Time.timeScale = 0f;
            if (screenUI)
                screenUI.SetActive(false);
            //deathScreen.SetActive(true);
            SceneManager.LoadScene(3);
        }
        else
        {
            StartCoroutine(HitCooldown());
        }
    }

    public void RestoreHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        healthBar.SetHealth(currentHealth);
    }

    IEnumerator HitCooldown()
    {
        isPlayerInvincible = true;
        yield return new WaitForSeconds(playerInvincibilityTimeSeconds);
        isPlayerInvincible = false;
    }
}
