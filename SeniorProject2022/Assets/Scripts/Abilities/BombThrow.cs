using DG.Tweening;
using UnityEngine;


public class BombThrow : ConsumableState
{
    private GameObject bomb;
    private const float PlayerSpeedWithBomb = 2;
    private const float DistanceAbovePlayer = 1.3f;
    private const float BombThrowStrength = 300;

    private Rigidbody bombRigidbody;

    private int bombThrowHash, bombHoldHash, bombExplodedHash;
    
    public BombThrow(PlayerController player, GameObject prefab) : base(player)
    {
        this.prefab = prefab;
    }

    private GameObject prefab;
    public override void EnterState()
    {
        bombThrowHash = Animator.StringToHash("bombThrow");
        bombHoldHash = Animator.StringToHash("bombHold");
        bombExplodedHash = Animator.StringToHash("bombExploded");

        bomb = GameObject.Instantiate(prefab, _player.transform.position + Vector3.up * DistanceAbovePlayer, 
            Quaternion.identity);
        bomb.transform.localScale = Vector3.zero;
        bomb.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InCubic);
        bombRigidbody = bomb.GetComponent<Rigidbody>();
        bombRigidbody.useGravity = false;
        bombRigidbody.freezeRotation = true;
        _player._animator.SetTrigger(bombHoldHash);
    }

    public override void ExitState()
    {
    }

    public override void Execute()
    {
        // bomb exploded
        if (bomb == null) {
            TransitionState(_player.runningState);
            _player._animator.SetTrigger(bombExplodedHash);
            return;
        }
        Quaternion currentRotation = _player.transform.rotation;
        Vector3 worldMoveDirection = _player.currentMovement;
        if (worldMoveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(worldMoveDirection);
            _player.transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, 
                _player.rotationsPerSecond * Time.deltaTime);;
        }
        _player._characterController.Move(Time.deltaTime * PlayerSpeedWithBomb * worldMoveDirection);
        bomb.transform.position = _player.transform.position + Vector3.up * DistanceAbovePlayer;
    }
    
    public override void OnConsumableTriggered()
    {
        _player._animator.SetTrigger(bombThrowHash);
        DOVirtual.DelayedCall(0.15f, ThrowBomb);
    }

    private void ThrowBomb()
    {
        bombRigidbody.useGravity = true;
        bombRigidbody.AddForce((_player.transform.forward * 5 + Vector3.up).normalized * BombThrowStrength);
        bombRigidbody.freezeRotation = false;
        TransitionState(_player.runningState);
    }
}
