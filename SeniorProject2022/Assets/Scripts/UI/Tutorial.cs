using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tutorial : MonoBehaviour
{
    public string moveText = "Use [W], [A], [S], [D] to move";
    public string evadeText = "Press [spacebar] to evade";
    public string attackText = "Use left mouse click to attack.";
    public string herdText = "Press [Q] to make your sheep herd charge forward.";
    public float fadeDuration = 2.0f;
    bool fadingIn;
    bool fadingOut;
    public TextMeshProUGUI instructionText;
    public CanvasGroup instructionGroup;
    private PlayerInput _playerInput;
    bool readyForAttack = false;
    public enum TutorialState
    {
        Init,
        Movement,
        Evading,
        Attacking,
        Herding,
        Finished
    }

    public TutorialState state;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.Enable();
        _playerInput.CharacterControls.Move.performed += EndMoveTutorial;
    }

    // Start is called before the first frame update
    void Start()
    {
        state = TutorialState.Init;
        if (instructionText == null)
        {
            instructionGroup = GameObject.Find("Instructions").GetComponent<CanvasGroup>();
            if (instructionGroup == null)
            {
                Debug.LogError("Can't find \"Instructions\" game object on player ui prefab. Please assign manually or fix the prefab.");
                return;
            }
            instructionText = instructionGroup.GetComponentInChildren<TextMeshProUGUI>();
        }
        instructionGroup.alpha = 0;
        StartCoroutine(MoveTutorial());
    }

    public IEnumerator MoveTutorial()
    {
        state = TutorialState.Movement;
        yield return StartCoroutine(ShowText(moveText));
        yield return new WaitUntil(() => state != TutorialState.Movement);
        yield return StartCoroutine(HideText());
        yield return StartCoroutine(EvadeTutorial());
    }

    public IEnumerator EvadeTutorial()
    {
        yield return new WaitUntil(() => state == TutorialState.Evading);
        yield return StartCoroutine(ShowText(evadeText));
        yield return new WaitUntil(() => state != TutorialState.Evading);
        yield return StartCoroutine(HideText());
        readyForAttack = true;
        StartCoroutine(AttackTutorial());
    }

    public IEnumerator AttackTutorial()
    {
        yield return new WaitUntil(() => readyForAttack);
        state = TutorialState.Attacking;
        yield return StartCoroutine(ShowText(attackText));
        _playerInput.CharacterControls.ClickMelee.started += EndAttackTutorial;
        _playerInput.CharacterControls.Melee.started += EndAttackTutorial;
        yield return new WaitUntil(() => state > TutorialState.Attacking);
        _playerInput.CharacterControls.ClickMelee.started -= EndAttackTutorial;
        _playerInput.CharacterControls.Melee.started -= EndAttackTutorial;
        yield return StartCoroutine(HideText());
    }

    public IEnumerator HerdingTutorial()
    {
        yield return new WaitUntil(() => state == TutorialState.Herding);
        _playerInput.CharacterControls.CommandHerd.started += EndHerdingTutorial;
        yield return StartCoroutine(ShowText(herdText));
        yield return new WaitUntil(() => state != TutorialState.Herding);
        _playerInput.CharacterControls.CommandHerd.started -= EndHerdingTutorial;
        yield return StartCoroutine(HideText());
        _playerInput.Disable();
    }

    void EndMoveTutorial(InputAction.CallbackContext context)
    {
        state = TutorialState.Evading;
        _playerInput.CharacterControls.Move.performed -= EndMoveTutorial;
        _playerInput.CharacterControls.Dash.started += EndEvadeTutorial;
    }

    void EndEvadeTutorial(InputAction.CallbackContext context)
    {
        state = TutorialState.Attacking;
        _playerInput.CharacterControls.Dash.started -= EndEvadeTutorial;
    }

    public void StartAttackTutorial()
    {
        StartCoroutine(AttackTutorial());
    }

    void EndAttackTutorial(InputAction.CallbackContext context)
    {
        state = TutorialState.Herding;
    }

    void EndHerdingTutorial(InputAction.CallbackContext context)
    {
        state = TutorialState.Finished;
        _playerInput.CharacterControls.Melee.started -= EndHerdingTutorial;
    }

    IEnumerator ShowText(string text = "")
    {
        instructionText.text = text;
        while (instructionGroup.alpha < 1.0f)
        {
            instructionGroup.alpha += Time.deltaTime / fadeDuration;
            yield return null;
        }
    }

    IEnumerator HideText()
    {
        while (instructionGroup.alpha > 0.0f)
        {
            instructionGroup.alpha -= Time.deltaTime / fadeDuration;
            yield return null;
        }
        yield return null;
    }

}
