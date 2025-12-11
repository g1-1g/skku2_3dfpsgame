using System;
using UnityEngine;
using UnityEngine.UI;

public class HPAndDashUpdate : MonoBehaviour
{
    [SerializeField] private Player _player;
    private PlayerMove _move;

    [SerializeField] private Slider _hpSlider;
    [SerializeField] private Slider _staminaSlider;
    private void Start()
    {
        _move = _player.transform.GetComponent<PlayerMove>();

        _player.HealthChanged += HPUpdate;
        _move.StaminaUpdate += StaminaUpdate;
    }

    private void HPUpdate(float value)
    {
        _hpSlider.value = Mathf.Clamp(value, 0f, 100f);
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
