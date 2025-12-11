using UnityEngine;

public class MonsterStats : MonoBehaviour
{
    public ConsumableStat Health;

    public ValueStat Damage;
    public ValueStat Speed;
    public ValueStat AttackSpeed;


    public EMonsterState State = EMonsterState.Idle;
}
