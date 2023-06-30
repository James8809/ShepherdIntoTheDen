using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Playables;
using UnityEngine.UI;

public class ShrineCutscene : MonoBehaviour
{
    private Animator _animator;
    DialogueManager dialogueManager;
    public PlayableDirector director;
    public GameObject teleporter;
    public Image whitePanel;

    // Start is called before the first frame update
    void Start()
    {
        _animator = FindObjectOfType<PlayerController>().gameObject.GetComponent<Animator>();
        _animator.SetTrigger("sleeping");
        dialogueManager = FindObjectOfType<DialogueManager>();
        DOVirtual.DelayedCall(3.0f, () => _animator.SetTrigger("wakeUp"));
        dialogueManager.finishedDialogue += ReenableMovement;
        FindObjectOfType<PlayerController>().DisableInput();
    }

    public void ReenableMovement()
    {
        director.time = director.time;
        director.playableGraph.GetRootPlayable(0).SetSpeed(1);
        teleporter.SetActive(true);
        teleporter.transform.localScale = Vector3.zero;
        teleporter.transform.DOScale(new Vector3(1.0f, 0.1f, 1.0f), 1f);
    }

    public void PauseTimeline()
    {
        director.playableGraph.GetRootPlayable(0).SetSpeed(0);
        dialogueManager.EnableDialoguePress();
    }
    
}
