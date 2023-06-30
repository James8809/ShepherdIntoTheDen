using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;
using DG.Tweening;
using Unity.Mathematics;

public class EnemyManager : DestroyableEnemy, IDamageReciever
{
    protected SkinnedMeshRenderer[] _skinnedMeshRenderers;
    protected static Color AttackWhite = new Color(1, 1, 1, .6f);
    public PlayerReferenceManager _referenceManager;
    protected BehaviorTree _behaviorTree;
    [SerializeField] private GameObject hitTextObject;

    [ColorUsageAttribute(true,true,0f,8f,0.125f,3f)]
    public Color flashColor = new Color(3, 0, 0, 1);
    
    // changing color test :)
    private MaterialPropertyBlock _propBlock;
    private Vector3 _initialPosition;


    // James fire trail status list
    public List<int> burnTickTimers = new List<int>();
    public Action<DestroyableEnemy> GetDeathEvent()
    {
        return OnDeath;
    }

    public virtual Animator GetAnimator()
    {
        return GetComponent<Animator>();
    }

    protected override void EnemyAwake()
    {
        // color changing features
        _skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        _propBlock = new MaterialPropertyBlock();
        _behaviorTree = GetComponent<BehaviorTree>();
        _initialPosition = transform.position;
    }

    public Vector3 InitialPosition => _initialPosition;

    /*
     * Default death implementation. If some other behavior is desired, update it in the derived class.
     */
    protected virtual void Die()
    {
        OnDeath?.Invoke(this);
        gameObject.SetActive(false);    // disable the enemy
    }

    public void TakeDamage(Weapon weapon)
    {
        // take damage according to weapon
        health.ModifyHealth(-weapon.weaponType.GetDamage());
        _behaviorTree.SetVariable("AttackingPlayer", (SharedBool)true);
        // spawn a text : )
        var text = Instantiate(hitTextObject, transform.position + Vector3.up * 4.0f, Quaternion.Euler(0.0f, 40f, 0.0f));
        text.GetComponent<HitText>().SetText(weapon.weaponType.GetDamage());
        if (health.GetCurrentHealth() <= 0)
        {
            Die();
        }
        else
        {
            StopCoroutine("DamageVisualFlashing");
            StartCoroutine(DamageVisualFlashing());
        }
    }
    
    // color management things:
    public Color GetCurrentAddColor()
    {
        // all colors should be uniform (hopefully) on the addColor (otherwise we need to change this)
        if (_skinnedMeshRenderers.Length > 0)
        {
            _skinnedMeshRenderers[0].GetPropertyBlock(_propBlock);
            return _propBlock.GetColor("_AddColor");
        }
        else
        {
            Debug.Log("WARNING: NO SKINNED MESH RENDERER ON ENEMY.");
            return Color.white;    // no skinnedMeshRenderer
        }
    }

    public void SetToonAddColor(Color addColor)
    {
        foreach (var renderer in _skinnedMeshRenderers)
        {
            renderer.GetPropertyBlock(_propBlock);
            _propBlock.SetColor("_AddColor", addColor);
            renderer.SetPropertyBlock(_propBlock);
        }
    }

    public Tween LerpToColor(Color addColor, float timeToTransition)
    {
        return DOTween.To(() => GetCurrentAddColor(),
            (Color color) => SetToonAddColor(color), addColor, timeToTransition).SetEase(Ease.OutQuart);
    }

    private IEnumerator DamageVisualFlashing()
    {
        SetToonAddColor(flashColor);
        for(int i = 0; i < 4; i++) {yield return null;}
        SetToonAddColor(new Color(-1, -1, -1, 1));
        for(int i = 0; i < 4; i++) {yield return null;}
        SetToonAddColor(flashColor);
        for(int i = 0; i < 4; i++) {yield return null;}
        SetToonAddColor(new Color(-1, -1, -1, 1));
        for(int i = 0; i < 4; i++) {yield return null;}
        SetToonAddColor(flashColor);
        for(int i = 0; i < 4; i++) {yield return null;}
        SetToonAddColor(Color.clear);
    }

    // add burn ticks to list 
    public void ApplyBurn(int ticks, Weapon obj)
    {
        if(burnTickTimers.Count <= 0)
        {
            burnTickTimers.Add(ticks);
            StartCoroutine(Burn(obj));
        }
        else
        {
            burnTickTimers.Add(ticks);
        }
    }

    // going through the list add do damage
    IEnumerator Burn(Weapon weapon)
    {
        while(burnTickTimers.Count >0)
        {
            for(int i = 0; i < burnTickTimers.Count; i++)
            {
                burnTickTimers[i]--;
            }
            TakeDamage(weapon);
            burnTickTimers.RemoveAll(number => number == 0);
            yield return new WaitForSeconds(0.75f);
        }
    }
}
