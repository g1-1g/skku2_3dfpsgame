using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class DamageEffect : MonoBehaviour
{
    private Image _image;
    private Player _player;
    private Color _color = new Color(1, 1, 1, 0.5f);
    private Color _originColor = Color.clear;

    private void Start()
    {
        _image = GetComponent<Image>();
         _player = FindFirstObjectByType<Player>();
        _player.Stats.Health.OnValueChanged += DamageEffectPlay;
    }

    public void DamageEffectPlay(float value, float maxValue)
    {
        _image.DOKill();
        _image.color = _originColor;

        _image.DOColor(_color, 0.3f).SetEase(Ease.OutSine).SetLoops(2, LoopType.Yoyo);
    }
}
