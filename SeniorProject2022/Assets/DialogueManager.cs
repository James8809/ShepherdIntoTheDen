using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class DialogueManager : MonoBehaviour
{
    [System.Serializable]
    public class Lines
    {
        public List<string> lines;
        public bool doSkip;
        public Animator animator;
        public string animName;
        public bool finishTutorial;
    }
    public List<Lines> lineLists = new List<Lines>();
    public float textSpeed;
    public PlayerController _player;
    // public Dialogue topDialogue;
    // public Dialogue bottomDialogue;
    public Dialogue dialogue;
    private int lineListIndex = 0;
    public GameObject bottomDialogueCanvas;
    public string NPCName;
    public Action finishedDialogue;
    [SerializeField] private EnemyCampTracker _enemyCampTracker;    // has to be set in inspector
    
    public Action OnFinishTutorial;

    void Awake()
    {
        _player = FindObjectOfType<PlayerController>(true);
        // subscribe to all dead event.
        if (_enemyCampTracker != null)
        {
            _enemyCampTracker.OnAllEnemiesDead += IncreaseLineIndex;
        }
    }
    void Start()
    {
        FindObjectOfType<PlayerController>().canAttack = false;
    }

    void CheckLine(InputAction.CallbackContext context)
    {
        dialogue.CheckLine();
        if(dialogue.isFinished)
        {
            DisableDialoguePress();
            finishedDialogue?.Invoke();
        }
    }
    
    public void EnableDialoguePress()
    {
        _player.DisableInput();
        _player._playerInput.DialogueControls.Enable();
        _player._playerInput.DialogueControls.Click.started+=CheckLine;
        bottomDialogueCanvas.SetActive(true);
        dialogue = bottomDialogueCanvas.GetComponentInChildren<Dialogue>();
        dialogue.textSpeed = textSpeed;
        dialogue.nameText.text = NPCName;
        LoadNextLines();
        dialogue.StartDialogue();
    }

    public void DisableDialoguePress()
    {
        if(lineLists[lineListIndex].animator != null)
        {
            if(lineLists[lineListIndex].animName != null){
                setDialogueAnimTrigger(lineLists[lineListIndex].animName);
            }
        }
        if(lineLists[lineListIndex].finishTutorial != false)
        {
            OnFinishTutorial();
        }
        if(lineLists[lineListIndex].doSkip)
        {
            IncreaseLineIndex();
        }
        _player.EnableInput();
        _player._playerInput.DialogueControls.Click.started-=CheckLine;
        _player._playerInput.DialogueControls.Disable();
        bottomDialogueCanvas.SetActive(false);
    }

    public void setDialogueAnimTrigger(string animName){
        lineLists[lineListIndex].animator.SetTrigger(animName);
    }
    public void LoadNextLines()
    {
        if (lineListIndex >= lineLists.Count)
        {
            return;
        }
        dialogue.lines = lineLists[lineListIndex].lines;
    }
    public void IncreaseLineIndex()
    {
        lineListIndex ++;
    }

}

