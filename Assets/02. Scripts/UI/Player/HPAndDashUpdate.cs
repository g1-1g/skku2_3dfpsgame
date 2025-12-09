using UnityEngine;
using UnityEngine.UI;

public class HPAndDashUpdate : MonoBehaviour
{
    [SerializeField] private PlayerMove _player;

    [SerializeField] private Slider _hpSlider;
    [SerializeField] private Slider _staminaSlider;

    private void Update()
    {
        StaminaUpdate();
    }
    void StaminaUpdate()
    {
        _staminaSlider.value = Mathf.Clamp(_player.Stamina, 0f, 100f);
    }
}
