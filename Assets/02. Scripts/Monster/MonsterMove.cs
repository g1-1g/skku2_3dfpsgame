using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Switch;

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
    

    [SerializeField] private float _traceDistance = 3;
    [SerializeField] private float _comebackDistance = 5;
    [SerializeField] private float _attackedDistance = 1;

    private float _attackTimer = 0;
    private Vector3 _startPosition;
    private float distance;



    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _startPosition = transform.position;
    }

    private void Update()
    {

        distance = Vector3.Distance(transform.position, _player.transform.position);


        switch (state)
        {
            case EMonsterState.Idle:
                Idle();
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

    public bool TryTakeDamage(float damage)
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
        return true;
    }

    private void Idle()
    {
        if (distance <= _traceDistance)
        {
            state = EMonsterState.Trace;
        }
    }

    private void Trace()
    {
        if (distance > _comebackDistance)
        {
            state = EMonsterState.Comeback;
            return;
        }else if(distance < _attackedDistance)
        {
            state = EMonsterState.Attack;
        }

        Vector3 direction = (_player.transform.position - transform.position).normalized;
        transform.Translate(direction * Time.deltaTime * _moveSpeed);
    }

    private void ComeBack()
    {
        if (distance < _traceDistance)
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
        if (distance > _attackedDistance)
        {
            state = EMonsterState.Trace;
            return;
        }

        _attackTimer += Time.deltaTime;
        if (_attackTimer > _attackSpeed)
        {
            Debug.Log("attack");
            _attackTimer = 0;
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
