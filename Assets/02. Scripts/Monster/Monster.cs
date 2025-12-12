using System;
using System.Collections;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

[RequireComponent(typeof(MonsterStats))]
[RequireComponent(typeof(CharacterController))]
public class Monster : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    private MonsterStats _stats;
    private CharacterController _characterController;

    private bool _isPatrolling = false; //순찰 이동중 여부
    private Vector3 _PatrolPoint; //순찰 포인트
    private float _lastAttackTime = 0; //마지막 공격타임
    private Vector3 _startPosition; //시작 위치
    private float _distanceFromPlayer; //플레이와 몬스터 거리
    private float _yVelocity; // 중력 y 방향 속도

    [Serializable]
    public struct MoveConfig
    {
        public float TraceDistance;
        public float ComebackDistance;
        public float AttackedDistance;
        public float PatrolDistance;
    }

    [SerializeField] private MoveConfig _config;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _stats = GetComponent<MonsterStats>();
        _startPosition = transform.position;
    }

    private void Update()
    {

        _distanceFromPlayer = Vector3.Distance(transform.position, _player.transform.position);


        switch (_stats.State)
        {
            case EMonsterState.Idle:
                Idle();
                break;
            case EMonsterState.Patrol:
                Patrol();
                break;
            case EMonsterState.Trace:
                Trace();
                break;
            case EMonsterState.Comeback:
                ComeBack();
                break;
            case EMonsterState.Attack:
                Attack();
                break;
            case EMonsterState.Hit:
                break;
            case EMonsterState.Death: 
                break;
        }
    }

    public bool TryTakeDamage(float damage, Vector3 knockBack)
    {
        if (_stats.State == EMonsterState.Death)
        {
            return false;
        }
        _stats.Health.Decrease(damage);

        if (_stats.Health.Value > 0)
        {
            _stats.State = EMonsterState.Hit;
            StartCoroutine(Hit());
        }
        else
        {
            _stats.State = EMonsterState.Death;
            StartCoroutine(Death());
        }
        _characterController.Move(knockBack);
        return true;
    }

    private void Idle()
    {
        if (_distanceFromPlayer <= _config.TraceDistance)
        {
            _stats.State = EMonsterState.Trace;
            return;
        }
        _stats.State = EMonsterState.Patrol;
    }
    
    private void Patrol()
    {
        if (_distanceFromPlayer <= _config.TraceDistance)
        {
            _stats.State = EMonsterState.Trace;
            return;
        }
        if (_isPatrolling)
        {
            Vector3 direction = (_PatrolPoint - transform.position).normalized;
            _characterController.Move(direction * _stats.Speed.Value * Time.deltaTime);
            transform.LookAt(_PatrolPoint);

            float distance = Vector3.Distance(transform.position, _PatrolPoint);
            if (distance < 0.1f) _isPatrolling = false;
            return;
        }
        Vector2 circle = UnityEngine.Random.insideUnitCircle * _config.PatrolDistance;
        _PatrolPoint = _startPosition + new Vector3( circle.x, 0, + circle.y);

        Debug.Log($"Stat Patrolling to {_PatrolPoint}");
        _isPatrolling = true;
    }

    private void Trace()
    {
        if (_distanceFromPlayer > _config.ComebackDistance)
        {
            _stats.State = EMonsterState.Comeback;
            return;
        }else if(_distanceFromPlayer < _config.AttackedDistance)
        {
            _stats.State = EMonsterState.Attack;
            return;
        }

        Vector3 direction = (_player.transform.position - transform.position).normalized;
        _characterController.Move(direction * Time.deltaTime * _stats.Speed.Value);
        transform.LookAt(_player.transform.position);
    }

    private void ComeBack()
    {
        if (_distanceFromPlayer < _config.TraceDistance)
        {
            _stats.State = EMonsterState.Trace; 
            return;
        }
        if (Vector3.Distance(_startPosition, transform.position) < 0.5f)
        {
            _stats.State = EMonsterState.Idle;
            return;
        }
        Vector3 direction = (_startPosition - transform.position).normalized;
        _characterController.Move(direction * _stats.Speed.Value * Time.deltaTime);
        transform.LookAt(_startPosition);
    }

    private void Attack()
    {
        if (_distanceFromPlayer > _config.AttackedDistance)
        {
            _stats.State = EMonsterState.Trace;
            return;
        }

        if (Time.time > _lastAttackTime + _stats.AttackSpeed.Value)
        {
            Debug.Log("attack");
            if(_player == null) return;
            _player.GetComponent<Player>().GetDamage(_stats.Damage.Value);
            _lastAttackTime = Time.time;
        }
    }

    private IEnumerator Hit()
    {
        yield return new WaitForSeconds(2);
        if (_stats.State == EMonsterState.Hit)
            _stats.State = EMonsterState.Idle;
    }
    private IEnumerator Death()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
