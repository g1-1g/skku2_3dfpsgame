using UnityEngine;
using UnityEngine.UIElements.Experimental;

public enum EPlayerState
{
    Idle,
    Walk,
    Run,
    Jump,
    Shoot,
    Throw,
    Hit,
    Death,
}
public class PlayerStats : MonoBehaviour
{
    public ConsumableStat Health;
    public ConsumableStat Stamina;
    
    public ValueStat Damage;
    public ValueStat MoveSpeed;
    public ValueStat RunSpeed;
    public ValueStat JumpPower;
    public ValueStat Gravity;

    public EPlayerState State = EPlayerState.Idle;

    private void Start()
    {
      
    }
}
