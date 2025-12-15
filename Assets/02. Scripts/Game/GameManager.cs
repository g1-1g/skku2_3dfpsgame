using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    private EGameState _state = EGameState.Ready;
    public EGameState State => _state;
    private Player _player;
    [SerializeField] private TextMeshProUGUI _stateText;

    [SerializeField] private float _readyTime = 2f;
    [SerializeField] private float _startRoadingTime = 0.2f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    private void Start()
    {
        _player = FindFirstObjectByType<Player>();
        _player.OnDied += GameOver;

        SetState(EGameState.Ready);
    }

    private IEnumerator StartToPlay_Coroutine()
    {
        yield return new WaitForSecondsRealtime(_readyTime);

        _stateText.text = "시작!";

        yield return new WaitForSecondsRealtime(_startRoadingTime);

        SetState(EGameState.Playing);

        _stateText.gameObject.SetActive(false);
    }

    private void SetState(EGameState newState)
    {
        _state = newState;

        switch (newState)
        {
            case EGameState.Playing:
                Time.timeScale = 1f;
                break;
            case EGameState.Ready:
                Time.timeScale = 0f;
                _stateText.text = "준비중...";
                StartCoroutine(StartToPlay_Coroutine());
                break;
            case EGameState.GameOver:
                Time.timeScale = 0f;
                break;
        }
    }

    public void GameOver()
    {
        _stateText.gameObject.SetActive(true);
        _stateText.text = "게임 오버";
        SetState(EGameState.GameOver);
    }

    private void OnDestroy()
    {
        if (_player != null) _player.OnDied -= GameOver;
    }

}
