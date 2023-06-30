using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HurtUIBehavior : MonoBehaviour
{
    private PlayerHealthSystem _healthSystem;
    private Image hurtImage;
    private Coroutine hurtCoroutine = null;

    public int framesToShow = 8;

    private void Awake()
    {
        hurtImage = GetComponent<Image>();
        _healthSystem = FindObjectOfType<PlayerHealthSystem>();
        hurtImage.color = new Color(1, 1, 1, 0);
    }

    private void OnEnable()
    {
        _healthSystem.onHealthChanged += ShowEffect;
    }
    
    private void OnDisable()
    {
        _healthSystem.onHealthChanged -= ShowEffect;
    }

    public void ShowEffect(int currentHealth, int maxHealth, int healthChange)
    {
        if (healthChange < 0)
        {
            hurtImage.color = Color.white;
            hurtImage.DOColor(new Color(1, 1, 1, 0), framesToShow / 60.0f).SetEase(Ease.OutQuad);
        }
    }
}
