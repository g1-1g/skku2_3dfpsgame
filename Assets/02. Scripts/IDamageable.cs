using UnityEngine;

public interface IDamageable
{
    //IDamageable 약속을 지켜야 하는 클래스는 무조건 아래 매서드를 구현해야한다.
    public bool TryTakeDamage(Damage damage);
}
