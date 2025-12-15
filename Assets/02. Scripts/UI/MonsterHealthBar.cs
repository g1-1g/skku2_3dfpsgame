using System;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthBar : MonoBehaviour
{
    private Monster _monster;
    [SerializeField] private Image _gaugeImage;
    [SerializeField] private Canvas _canvas;

    private float _lastHealth = 0;

    private Camera _mainCamera;

    private void Awake()
    {
        _monster = GetComponent<Monster>();
        _monster.StatsChanged += UIUpdate;
        _mainCamera = Camera.main;
    }

    private void UIUpdate(MonsterStats stats)
    {
        if (_lastHealth != stats.Health.Value)
        {
            _lastHealth = stats.Health.Value;
            _gaugeImage.fillAmount = stats.Health.Ratio;
        }
    }


    private void LateUpdate()
    {

        //빌보드 기법
        _canvas.transform.forward = _mainCamera.transform.forward;
    }
}
