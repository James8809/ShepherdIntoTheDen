using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class Dialogue : MonoBehaviour
{
    // Start is called before the first frame update
    
    public TextMeshProUGUI text;
    public List<string> lines;
    public float textSpeed;
    private int index;
    public bool isFinished;
    public TextMeshProUGUI nameText;

    public void StartDialogue()
    {
        text.text = string.Empty;
        index = 0;
        isFinished = false;
        StartCoroutine(TypeLine());
    }


    public void CheckLine()
    {
        if(text.text == lines[index])
        {
            NextLine();
        }
        else
        {
            StopAllCoroutines();
            text.text = lines[index];
        }
    
    }

    public void NextLine()
    {
        if(index < lines.Count-1)
        {
            index++;
            text.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            FinishLines();
        }
    }

    public void FinishLines()
    {
        isFinished = true;
    }

    IEnumerator TypeLine()
    {
        yield return new WaitForSeconds(0.5f);
        foreach(char c in lines[index].ToCharArray())
        {
            text.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }


}
