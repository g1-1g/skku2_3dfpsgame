using System;
using System.Collections;
using Unity.Android.Gradle.Manifest;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MonsterStats))]
//[RequireComponent(typeof(CharacterController))]
public class Monster : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    private MonsterStats _stats;
    private CharacterController _characterController;
    private NavMeshAgent _agent;
    private Animator _animator;

    private bool _isPatrolling = false; //순찰 이동중 여부
    private Vector3 _PatrolPoint; //순찰 포인트
    private float _lastAttackTime = 0; //마지막 공격타임
    private Vector3 _startPosition; //시작 위치
    private float _distanceFromPlayer; //플레이와 몬스터 거리
    private float _yVelocity; // 중력 y 방향 속도

    private Vector3 _jumpStartPosition;
    private Vector3 _jumpEndPosition;

    public event Action<MonsterStats> StatsChanged;

    [Serializable]
    public struct MoveConfig
    {
        public float TraceDistance;
        public float ComebackDistance;
        public float AttackedDistance;
        public float PatrolDistance;
        public float Gravity;
    }

    [SerializeField] private MoveConfig _config;

    private void Start()
    {
        _player = FindFirstObjectByType<Player>().gameObject;
        _characterController = GetComponent<CharacterController>();
        
        _animator = GetComponent<Animator>();
        _stats = GetComponent<MonsterStats>();
        _startPosition = transform.position;
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _stats.Speed.Value;
    }

    private void Update()
    {
        if(GameManager.Instance.State != EGameState.Playing) return;

        //ApplyGravity();

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
            case EMonsterState.Jump:
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
        StatsChanged?.Invoke(_stats);

        _agent.ResetPath();

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
        return true;
    }

    private void Idle()
    {
        if (_distanceFromPlayer < _config.AttackedDistance)
        {
            _stats.State = EMonsterState.Attack;
            return;
        }else if (_distanceFromPlayer <= _config.TraceDistance)
        {
            _animator.SetBool("Walk", true);
            _stats.State = EMonsterState.Trace;
            return;
        }
        else
        {
            _animator.SetBool("Walk", true);
            _stats.State = EMonsterState.Patrol;
        }      
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
            
            float distance = Vector3.Distance(transform.position, _PatrolPoint);
            if (distance < 0.2f)
            {
                _isPatrolling = false;

                _agent.ResetPath();

            }
                
            return;
        }
        Vector2 circle = UnityEngine.Random.insideUnitCircle * _config.PatrolDistance;
        _PatrolPoint = _startPosition + new Vector3( circle.x, 0, + circle.y);
        
        _agent.SetDestination(_PatrolPoint);

        _isPatrolling = true;
    }

    private void Trace()
    {
        if (_distanceFromPlayer > _config.ComebackDistance)
        {
            _agent.ResetPath();
            _stats.State = EMonsterState.Comeback;
            return;
        }else if(_distanceFromPlayer < _config.AttackedDistance)
        {
            _agent.ResetPath();
            _stats.State = EMonsterState.Attack;
            return;
        }

        if (_agent.isOnOffMeshLink)
        {
            OffMeshLinkData linkData = _agent.currentOffMeshLinkData;
            _jumpStartPosition = linkData.startPos;
            _jumpEndPosition = linkData.endPos;

            if (_jumpEndPosition.y != _jumpStartPosition.y)
            {
                StartCoroutine(JumpRoutine());
                _stats.State = EMonsterState.Jump;
                return;
            }
        }

        _agent.SetDestination(_player.transform.position);
    }
    private IEnumerator JumpRoutine()
    {
        _agent.isStopped = true;

        _agent.ResetPath();

        Quaternion targetRotation = Quaternion.LookRotation(_jumpEndPosition - transform.position);
        transform.rotation = targetRotation;

        float duration = 0.5f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float normalized = t / duration;

            // 포물선
            float height = 2f;
            Vector3 pos = Vector3.Lerp(_jumpStartPosition, _jumpEndPosition, normalized);
            pos.y += Mathf.Sin(normalized * Mathf.PI) * height;

            transform.position = pos;
            yield return null;
        }

        transform.position = _jumpEndPosition;

        _agent.CompleteOffMeshLink();
        _agent.isStopped = false;

        _stats.State = EMonsterState.Trace;
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
            _animator.SetBool("Walk", false);
            _stats.State = EMonsterState.Idle;
            return;
        }

        _agent.SetDestination(_startPosition);
    }

    private void Attack()
    {
        if (_distanceFromPlayer > _config.AttackedDistance)
        {
            _animator.SetBool("Walk", true);
            _stats.State = EMonsterState.Trace;
            return;
        }

        if (Time.time > _lastAttackTime + _stats.AttackSpeed.Value)
        {
            _animator.SetTrigger("Attack");
            _animator.SetBool("Walk", false);

            if(_player == null) return;
            _player.GetComponent<Player>().GetDamage(_stats.Damage.Value);
            _lastAttackTime = Time.time;
        }
        transform.LookAt(new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z));
    }

    private IEnumerator Hit()
    {
       
        _animator.SetTrigger("Damage");
        yield return new WaitForSeconds(2);
        if (_stats.State == EMonsterState.Hit)
            _stats.State = EMonsterState.Idle;
    }
    private IEnumerator Death()
    {
        _animator.ResetTrigger("Damage");
        _animator.SetTrigger("Death");
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
