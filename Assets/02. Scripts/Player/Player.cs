using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerStats _stats;
    public event Action<float> HealthChanged;

    private void Start()
    {
        _stats = GetComponent<PlayerStats>();
    }
    public void GetDamage(float damage)
    {
        _stats.Health.Decrease(damage);
        HealthChanged?.Invoke(_stats.Health.Value);

    }
}
