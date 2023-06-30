using UnityEngine;
using DG.Tweening;


public class DaggerThrow : ConsumableState
{
    private GameObject dagger;
    private const float PlayerSpeedWithBomb = 2;
    private const float BombThrowStrength = 300;
    
    private float _moveFactor = 0.0f;
    private float _rotationFactor = 0.0f;
    private Quaternion initialRotation = Quaternion.identity;

    private Rigidbody bombRigidbody;
    public int daggerThrowHash;
    
    
    public DaggerThrow(PlayerController player, GameObject prefab) : base(player)
    {
        this.prefab = prefab;
    }

    private GameObject prefab;
    public override void EnterState()
    {
        daggerThrowHash = Animator.StringToHash("throwDagger");
        _player._animator.SetTrigger(daggerThrowHash);
        InstantiateDagger();
        DOVirtual.DelayedCall(0.15f, () => TransitionState(_player.runningState));
    }

    public override void ExitState()
    {
    }

    private void InstantiateDagger()
    {
        var playerTransform = _player.transform;
        dagger = GameObject.Instantiate(prefab, playerTransform.position + playerTransform.forward * 0.5f +
                                                Vector3.up * 0.5f, initialRotation);
    }

    public override void Execute()
    {
        Quaternion currentRotation = _player.transform.rotation;
        _player.transform.rotation = Quaternion.Slerp(currentRotation, initialRotation, 
            40 * Time.deltaTime * _rotationFactor);
        Debug.Log(_player.rotationsPerSecond + ", " + _rotationFactor);
    }

    public void SetDaggerThrowState(float moveFactor, float rotationFactor, Quaternion rotation)
    {
        Debug.Log("Setting dager throw");
        _moveFactor = moveFactor;
        _rotationFactor = rotationFactor;
        _rotationFactor = 1f;
        initialRotation = rotation;
    }
    
    public override void OnConsumableTriggered()
    {
    }
}
