using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager;
    // check player tag 
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" & dialogueManager!= null)
        {
            dialogueManager.EnableDialoguePress();
        }
    }
}
