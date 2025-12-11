using System.Collections;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    //몬스터 AI
    // 규칙 기반 인공지능 : 정해진 규칙에 따라 조건문/반복문 등을 이용해서 코딩하는 것
    //                      -> ex) FMS(유한 상태머신), BT(행동 트리)
    // 학습 기반 인공지능 : 머신러닝(딥러닝, 강화학습 ...)
    //플레이어가 일정 거리내로 돌아오면 trace
    //일정 거리로 멀어지면 return start pos
    [SerializeField] private GameObject _player;
    private CharacterController _characterController;

    public EMonsterState state = EMonsterState.Idle;
    [SerializeField] private float _health = 100;
    private float _moveSpeed = 3;
    private float _attackSpeed = 3;
    private float _attackDamage = 10;
    

    [SerializeField] private float _traceDistance = 3;
    [SerializeField] private float _comebackDistance = 5;
    [SerializeField] private float _attackedDistance = 1.5f;
    [SerializeField] private float _patrolDistance = 10f;

    private bool _isPatrolling = false;
    private Vector3 _PatrolPoint;
    private float _lastAttackTime = 0;
    private Vector3 _startPosition;
    private float _distanceFromPlayer;




    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _startPosition = transform.position;
    }

    private void Update()
    {

        _distanceFromPlayer = Vector3.Distance(transform.position, _player.transform.position);


        switch (state)
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
        if (state == EMonsterState.Death)
        {
            return false;
        }
        _health -= damage;

        if (_health > 0)
        {
            state = EMonsterState.Hit;
            StartCoroutine(Hit());
        }
        else
        {
            state = EMonsterState.Death;
            StartCoroutine(Death());
        }
        _characterController.Move(knockBack);
        return true;
    }

    private void Idle()
    {
        if (_distanceFromPlayer <= _traceDistance)
        {
            state = EMonsterState.Trace;
        }
        state = EMonsterState.Patrol;
    }
    
    private void Patrol()
    {
        if (_distanceFromPlayer <= _traceDistance)
        {
            state = EMonsterState.Trace;
            return;
        }
        if (_isPatrolling)
        {
            Vector3 direction = (_PatrolPoint - transform.position).normalized;
            _characterController.Move(direction * _moveSpeed * Time.deltaTime);

            float distance = Vector3.Distance(transform.position, _PatrolPoint);
            if (distance < 0.1f) _isPatrolling = false;
            return;
        }
        Vector2 circle = Random.insideUnitCircle * _patrolDistance;
        _PatrolPoint = _startPosition + new Vector3( circle.x, 0, + circle.y);

        Debug.Log($"Stat Patrolling to {_PatrolPoint}");
        _isPatrolling = true;
    }

    private void Trace()
    {
        if (_distanceFromPlayer > _comebackDistance)
        {
            state = EMonsterState.Comeback;
            return;
        }else if(_distanceFromPlayer < _attackedDistance)
        {
            state = EMonsterState.Attack;
            return;
        }

        Vector3 direction = (_player.transform.position - transform.position).normalized;
        _characterController.Move(direction * Time.deltaTime * _moveSpeed);
    }

    private void ComeBack()
    {
        if (_distanceFromPlayer < _traceDistance)
        {
            state = EMonsterState.Trace; 
            return;
        }
        if (Vector3.Distance(_startPosition, transform.position) < 0.5f)
        {
            state = EMonsterState.Idle;
            return;
        }
        Vector3 direction = (_startPosition - transform.position).normalized;
        _characterController.Move(direction * _moveSpeed * Time.deltaTime);
    }

    private void Attack()
    {
        if (_distanceFromPlayer > _attackedDistance)
        {
            state = EMonsterState.Trace;
            return;
        }

        if (Time.time > _lastAttackTime + _attackSpeed)
        {
            Debug.Log("attack");
            _player.GetComponent<Player>().GetDamage(_attackDamage);
            _lastAttackTime = Time.time;
        }
    }

    private IEnumerator Hit()
    {
        yield return new WaitForSeconds(2);
        state = EMonsterState.Idle;
    }
    private IEnumerator Death()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
