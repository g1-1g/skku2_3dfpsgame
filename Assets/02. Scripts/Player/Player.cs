using System;
using UnityEngine;
using static PlayerGunFire;

public class Player : MonoBehaviour
{
    private PlayerStats _stats;
    public event Action<PlayerStats> HealthChanged;
    public event Action OnDied;
    public PlayerGunFire _playerGunFire;

    public GameObject _gun;
    private bool _isGunInLeftHand = false;
    public Transform RightHandSocket;
    public Transform LeftHandSocket;

    private Animator _animator;
    private void Start()
    {
        _stats = GetComponent<PlayerStats>();
        _animator = GetComponent<Animator>();
        _playerGunFire = GetComponent<PlayerGunFire>();

        _playerGunFire.OnShoot += Shoot;
        _playerGunFire.OnGunReload += Reload;

    }

    

    public void GetDamage(float damage)
    {
        _stats.Health.Decrease(damage);
        HealthChanged?.Invoke(_stats);
        if (_stats.Health.Value <= 0)
        {
            OnDied?.Invoke();
        }
    }

    private void Update()
    {

        switch (_stats.State)
        {
            case EPlayerState.Idle:
                Idle();
                break;
            case EPlayerState.Walk:
                //Walk();
                break;
            case EPlayerState.Run:
                //Run();
                break;
            case EPlayerState.Shoot:
                break;
            case EPlayerState.Throw:
                Throw();
                break;
            case EPlayerState.Hit:
                break;
            case EPlayerState.Death:
                break;
            case EPlayerState.Jump:
                break;
        }
    }

    private void Throw()
    {
 
    }

    private void Shoot(PlayerGunFire.Gun gun)
    {
        _stats.State = EPlayerState.Shoot;
        AttachGunToHand();
        _animator.SetTrigger("Shoot");
        _stats.State = EPlayerState.Idle;
    }

    private void Reload(PlayerGunFire.Gun gun)
    {
        _stats.State = EPlayerState.Reload;
        AttachGunToHand();
        _animator.SetTrigger("Reload");
        _stats.State = EPlayerState.Idle;
    }
    private void Idle()
    {
        
    }

    
    public void ThrowAnimationEvent()
    {
        _stats.State = EPlayerState.Throw;
        AttachGunToHand();
    }
    public void AttachGunToHand()
    {
        if (_isGunInLeftHand)
        {
            _gun.transform.SetParent(RightHandSocket);
            _isGunInLeftHand = false;
        }
        else
        {
            if (_stats.State != EPlayerState.Throw) return;
            _gun.transform.SetParent(LeftHandSocket);
            _isGunInLeftHand = true;
        }
        _gun.transform.localPosition = Vector3.zero;
        _gun.transform.localRotation = Quaternion.identity;
        
    }

    private void OnDestroy()
    {
        if (_playerGunFire != null) return;
        _playerGunFire.OnShoot += Shoot;
        _playerGunFire.OnGunReload += Reload;
    }
}
