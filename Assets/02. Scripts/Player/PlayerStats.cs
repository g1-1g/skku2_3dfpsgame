using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PlayerStats : MonoBehaviour
{
    public ConsumableStat Health;
    public ConsumableStat Stamina;
    
    public ValueStat Damage;
    public ValueStat MoveSpeed;
    public ValueStat RunSpeed;
    public ValueStat JumpPower;
    public ValueStat Gravity;

    private void Start()
    {
      
    }
}
