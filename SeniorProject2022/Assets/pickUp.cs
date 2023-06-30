using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class pickUp : MonoBehaviour
{
    public GameObject stick;
    private DialogueManager dialogueManager;
    private CrestManager crestManager;
    private Tween tween;
    public WeaponObject weapon;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Detecting player on stick");
            crestManager.EnableStick(true);
            dialogueManager.IncreaseLineIndex();
            var playerController = FindObjectOfType<PlayerController>();
            playerController.AddAttackFirstTime();
            playerController.SetPlayerWeapon(weapon);
            tween.Kill();
            Destroy(this.gameObject);

        }
    }

    void Awake()
    {
        dialogueManager = FindObjectOfType<DialogueManager>(true);
        crestManager = FindObjectOfType<CrestManager>(true);
    }
    void Start()
    {
        crestManager = FindObjectOfType<CrestManager>(true);
        Debug.Log(crestManager);
        tween = transform.DOLocalMoveY(transform.position.y + 0.03f, 1, false)
            .SetLoops(-1, LoopType.Yoyo)
            .SetDelay(UnityEngine.Random.Range(0.2f, 0.5f))
            .SetEase(Ease.InOutSine);
    }
}
