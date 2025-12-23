using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    private EGameState _state = EGameState.Ready;
    public EGameState State => _state;
    private Player _player;

    [SerializeField] private float _readyTime = 2f;
    [SerializeField] private float _overTime = 1f;
    
    public event Action<EGameState> OnGameStateChanged;
    public event Action<int> OnCoinChanged;

    private int _coin;

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

    private void Update()
    {
        
    }
    private IEnumerator StartToPlay_Coroutine()
    {
        yield return new WaitForSecondsRealtime(_readyTime);

        SetState(EGameState.Playing);
    }
    private IEnumerator GameOverDelay_Coroutine()
    {
        yield return new WaitForSeconds(_overTime);
        SetState(EGameState.GameOver);
    }

    public void SetState(EGameState newState)
    {
        _state = newState;

        switch (newState)
        {
            case EGameState.Playing:
                OnGameStateChanged?.Invoke(_state);
                Time.timeScale = 1f;
                break;
            case EGameState.Ready:
                ResetCoin();
                OnGameStateChanged?.Invoke(_state);
                Time.timeScale = 0f;
                StartCoroutine(StartToPlay_Coroutine());
                break;
            case EGameState.GameOver:
                OnGameStateChanged?.Invoke(_state);
                Time.timeScale = 0f;
                break;
            case EGameState.Pause:
                OnGameStateChanged?.Invoke(_state);
                Time.timeScale = 0;
                break;
        }
    }

    public void GetCoin(int count)
    {
        _coin += count;
        OnCoinChanged?.Invoke(_coin);
    }

    public void ResetCoin()
    {
        _coin = 0;
        OnCoinChanged?.Invoke(_coin);
    }
    public void GameOver()
    {
        StartCoroutine(GameOverDelay_Coroutine());
    }

    private void OnDestroy()
    {
        if (_player != null) _player.OnDied -= GameOver;
    }

    public void Quit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit(); // 어플리케이션 종료
        #endif
    }

}
