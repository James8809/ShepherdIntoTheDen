using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using Crafting;

public class RecipeCamp : MonoBehaviour
{
    // NOTE: THIS MUST BE A CHILD OF AN ENEMY CAMP
    private Animator _anim;
    public FMODUnity.EventReference rewardSound;
    public GameObject recipeObject, openBoxParticles;
    public Transform animationStart, animationEnd, particleSpawnPoint;
    
    public CraftableItem item;
    
    [SerializeField] private EnemyCampTracker _enemyCampTracker;    // has to be set in inspector
    private void Awake()
    {
        // subscribe to all dead event.
        if (_enemyCampTracker != null)
        {
            _enemyCampTracker.OnAllEnemiesDead += OnCampDeafeated;
        }

        // yet another singleton thing :/
        _anim = GetComponent<Animator>();
    }

    public void OnCampDeafeated()
    {
        _anim.SetTrigger("openChest");
        PlaySound();
    }

    public void SpawnRecipe()
    {
        float duration = 2.0f;
        var recipe = Instantiate(recipeObject, animationStart.position, transform.rotation);
        recipe.transform.localScale = new Vector3(.1f, .1f, .1f);
        recipe.transform.DOScale(new Vector3(58f, 58f, 6f), duration);
        recipe.transform.DOJump(animationEnd.position, 2.0f, 1, duration);
        recipe.GetComponent<Scroll>().item = this.item;
    }

    public void SpawnParticles()
    {
        var particles = Instantiate(openBoxParticles, particleSpawnPoint.position, transform.rotation * Quaternion.Euler(0, 90, 0));
        Destroy(particles, 12.0f);
    }

    public void PlayerToItemCam()
    {
        ItemCamera _itemCamera = FindObjectOfType<ItemCamera>();
        _itemCamera.Vcam.m_Lens.FieldOfView = 16;
        _itemCamera.PointCamAtItem(transform);
    }

    public void ItemToPlayerCam()
    {
        FindObjectOfType<ItemCamera>().PointCamBackAtPlayer();
    }

    public void PlaySound(){
        FMODUnity.RuntimeManager.PlayOneShot(rewardSound);
    }
}