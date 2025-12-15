using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HPAndDashUpdate : MonoBehaviour
{
    [SerializeField] private Player _player;
    private PlayerMove _move;

    [SerializeField] private Image _hpBackFill;
    [SerializeField] private Image _hpFrontFill;


    [SerializeField] private Slider _staminaSlider;
    private void Start()
    {
        _move = _player.transform.GetComponent<PlayerMove>();

        _player.HealthChanged += HPUpdate;
        _move.StaminaUpdate += StaminaUpdate;
    }

    private void HPUpdate(PlayerStats stats)
    {
        float finalValue = stats.Health.Ratio;
        _hpBackFill.DOFillAmount(finalValue, 0.5f);
        _hpFrontFill.DOFillAmount(finalValue, 0.2f);
    }

    void StaminaUpdate(float value)
    {
        _staminaSlider.value = Mathf.Clamp(value, 0f, 100f);
    }
    private void OnDestroy()
    {
        if (_player != null)
        {
            _player.HealthChanged -= HPUpdate;
            _move.StaminaUpdate += StaminaUpdate;
        }
    }
}
