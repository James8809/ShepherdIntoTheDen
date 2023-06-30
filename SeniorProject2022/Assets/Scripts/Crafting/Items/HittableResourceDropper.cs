using System.Collections;
using System.Collections.Generic;
using Crafting;
using UnityEngine;
using Enemy;
using DG.Tweening;

public class HittableResourceDropper : ResourceDropper
{
    public AnimationClip hitAnimation;
    public int hitsUntilDestroyed = 6;

    private Animation anim;
    private int numHits = 0;
    private Tween doTween;
    private Tween doTween2;
    private Tween doTween3;

    
    private void Start()
    {
        base.Start();
        if (hitAnimation)
            anim = GetComponent<Animation>();
    }
    
    // On hit from weapon, drop + spawn resource
    private void OnTriggerEnter(Collider other)
    {
        // only weapons that can collide with this system should be registered
        var weapon = other.GetComponent<Weapon>();
        if (weapon == null)
            return;
        if (numHits <= hitsUntilDestroyed)
            TriggerEnterHitObject();
    }
    
    private void TriggerEnterHitObject()
    {
        numHits++;
        if (numHits == hitsUntilDestroyed)
            TransitionDestroyGameObject();
        SpawnResources();
        if (hitAnimation != null)
        {
            anim.clip = hitAnimation;
            anim.Play();
        }
    }
    
    // Shrinks game object and destroys it
    private void TransitionDestroyGameObject()
    {
        float timeToDestroy = 1.5f;
        doTween = transform.DOMoveY(transform.position.y - 2, timeToDestroy, false).SetEase(Ease.InOutCubic);
        doTween2 = transform.DOScale(0, timeToDestroy * 0.7f).SetEase(Ease.InOutCubic);
        doTween3 = transform.DOShakeRotation(timeToDestroy, Vector3.up * 10f).OnComplete(()=> {
            doTween.Kill();
            doTween2.Kill();
            Destroy(this.gameObject);
            });
    }
}
