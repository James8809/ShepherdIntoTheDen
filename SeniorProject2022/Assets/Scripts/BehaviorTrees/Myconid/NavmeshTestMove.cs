using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class NavmeshTestMove : MonoBehaviour
{
    private Camera _mainCam;
    private PlayerInput _playerInput;
    private NavMeshAgent _navmeshAgent;
    private Animator _anim;

    private void Update()
    {
        _anim.SetFloat("speed", _navmeshAgent.velocity.magnitude);
    }

    private void Awake()
    {
        _mainCam = Camera.main;
        _navmeshAgent = GetComponent<NavMeshAgent>();
        _playerInput = new PlayerInput();
        _anim = GetComponent<Animator>();
    }
    
    void OnEnable()
    {
        _playerInput.CharacterControls.Enable();
        _playerInput.CharacterControls.ClickMelee.started += GoToPointClicked;
    }

    void OnDisable()
    {
        _playerInput.CharacterControls.ClickMelee.started -= GoToPointClicked;
        _playerInput.CharacterControls.Disable();
    }

    private void GoToPointClicked(InputAction.CallbackContext context)
    {
        Vector3 mousePosition = _playerInput.CharacterControls.MousePosition.ReadValue<Vector2>();
        Ray ray = _mainCam.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            _navmeshAgent.SetDestination(hit.point);
        }
    }
}
