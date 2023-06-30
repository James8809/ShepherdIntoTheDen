using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Bomb : MonoBehaviour
{
    public GameObject hitbox;
    public float timeUntilExplode = 5;
    public GameObject bombExplosion;
    public GameObject bombModel;
    
    private CombatEffectManager combatEffectManager;
    private MeshRenderer bombMaterialModel;
    private float time;
    
    private void Awake()
    {
        combatEffectManager = FindObjectOfType<CombatEffectManager>();
        time = timeUntilExplode;
        bombMaterialModel = bombModel.GetComponentInChildren<MeshRenderer>();
        StartCoroutine(BombFlash(0.5f));
        ExplosionSequence();
    }

    private void Update()
    {
        time -= Time.deltaTime;
    }

    IEnumerator BombFlash(float t)
    {
        if (time < 0.2f) {
            bombMaterialModel.materials[0].EnableKeyword("_EMISSION");
            yield return null;
        }
        yield return new WaitForSeconds(t);
        bombMaterialModel.materials[0].EnableKeyword("_EMISSION");
        DOVirtual.DelayedCall(0.1f, () => bombMaterialModel.materials[0].DisableKeyword("_EMISSION"));

        if (time < timeUntilExplode / 2)
        {
            StartCoroutine(BombFlash(0.2f));
        }
        else if (time < timeUntilExplode)
        {
            StartCoroutine(BombFlash(0.7f));
        }
    }

    private void ExplosionSequence()
    {
        DOVirtual.DelayedCall(timeUntilExplode, ExplodeBomb);
    }

    private void ExplodeBomb()
    {
        // hit-box
        hitbox.SetActive(true);
        DOVirtual.DelayedCall(0.2f, () => hitbox.SetActive(false));
        bombExplosion.SetActive(true);
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;

        // bomb model scaling
        Sequence scaleSequence = DOTween.Sequence();
        scaleSequence.Append(bombModel.transform.DOScale(new Vector3(2, 2, 2), 0.2f));
        scaleSequence.Append(bombModel.transform.DOScale(Vector3.zero, 0.1f));
        scaleSequence.Play();
        
        // camera shake
        if (combatEffectManager)
            combatEffectManager.ShakeCamera(10, 1.5f);
        DOVirtual.DelayedCall(1f, () => Destroy(gameObject));
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("PlayerWeapon"))
            return;
        
        // Player hit knockback
        var weapon = other.gameObject.GetComponent<Weapon>();
        if (weapon == null)
        {
            return;
        }
        // weapon on player
        Vector3 knockBackdir;
        if (other.gameObject.transform.parent.GetComponent<PlayerController>())
            knockBackdir = other.gameObject.transform.parent.forward;
        else // weapon from somewhere else
            knockBackdir = (transform.position - weapon.transform.position);
        knockBackdir.y = 0;
        knockBackdir = knockBackdir.normalized;
        gameObject.GetComponent<Rigidbody>().AddForce(knockBackdir * 250);
    }
}
