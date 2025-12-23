using System;
using UnityEngine;

[Serializable]
public class ConsumableStat
{
    [SerializeField] private float _maxValue;
    [SerializeField] private float _value;
    [SerializeField] private float _regenValue;

    public float Value => _value;
    public float Ratio => _maxValue > 0 ? _value / _maxValue : 0f;

    public event Action<float, float> OnValueChanged;

    public void Initialize()
    {
        SetValue(_maxValue);
    }
    public void Regenerate(float time)
    {
        _value += _regenValue * time;
        if (_value > _maxValue)
        {
            _value = _maxValue;
        }
        OnValueChanged?.Invoke(_value, _maxValue);
    }

    public bool TryConsume(float amount)
    {
        if (_value < amount) return false;

        Consume(amount);
        return true;
    }

    public void Consume(float amount)
    {
        _value -= amount;
        OnValueChanged?.Invoke(_value, _maxValue);
    }

    public void IncreaseMax(float amount)
    {
        _maxValue += amount;
        OnValueChanged?.Invoke(_value, _maxValue);
    }

    public void Increase(float amount)
    {
        SetValue(_value + amount);
    }

    public void DecreaseMax(float amount)
    {
        _maxValue -= amount;
        OnValueChanged?.Invoke(_value, _maxValue);
    }

    public void Decrease(float amount)
    {
        SetValue(_value - amount);
    }

    public void SetValue(float amount)
    {
        _value = amount;
        if (_value > _maxValue)
        {
            _value = _maxValue;
        }
        else if (_value < 0)
        {
            _value = 0;
        }
        OnValueChanged?.Invoke(_value, _maxValue);
    }
}
