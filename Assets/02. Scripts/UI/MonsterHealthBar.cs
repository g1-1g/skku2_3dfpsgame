using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthBar : MonoBehaviour
{
    private MonsterStats _monster;
    [SerializeField] private Image _gaugeImage;
    [SerializeField] private Canvas _canvas;

    private float _lastHealth = 0;

    private void Awake()
    {
        _monster = GetComponent<MonsterStats>();
    }

    private void LateUpdate()
    {
        if(_lastHealth != _monster.Health.Value)
        {
            _lastHealth = _monster.Health.Value;
            _gaugeImage.fillAmount = _monster.Health.Value / _monster.Health.MaxValue;
        }

        //빌보드 기법
        _canvas.transform.forward = Camera.main.transform.forward;


    }
}
