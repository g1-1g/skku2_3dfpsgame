using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HPAndDashUpdate : MonoBehaviour
{
    [SerializeField] private Player _player;

    [SerializeField] private Image _hpBackFill;
    [SerializeField] private Image _hpFrontFill;


    [SerializeField] private Slider _staminaSlider;
    private void Start()
    {
        _player.Stats.Health.OnValueChanged += HPUpdate;
        _player.Stats.Stamina.OnValueChanged += StaminaUpdate;
    }

    private void HPUpdate(float value, float maxValue)
    {
        float finalValue = value/ maxValue;
        _hpBackFill.DOFillAmount(finalValue, 0.5f);
        _hpFrontFill.DOFillAmount(finalValue, 0.2f);
    }

    void StaminaUpdate(float value, float maxValue)
    {
        _staminaSlider.value = Mathf.Clamp(value, 0f, 100f);
    }
    private void OnDestroy()
    {
        if (_player != null)
        {
            _player.Stats.Health.OnValueChanged -= HPUpdate;
            _player.Stats.Stamina.OnValueChanged -= StaminaUpdate;
        }
    }
}
