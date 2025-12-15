using System;
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
    
    [SerializeField] private float _readyTime = 2f;

    public event Action<EGameState> OnGameStateChanged;

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

        SetState(EGameState.Playing);
    }

    private void SetState(EGameState newState)
    {
        _state = newState;

        switch (newState)
        {
            case EGameState.Playing:
                OnGameStateChanged?.Invoke(_state);
                Time.timeScale = 1f;
                break;
            case EGameState.Ready:
                OnGameStateChanged?.Invoke(_state);
                Time.timeScale = 0f;
                StartCoroutine(StartToPlay_Coroutine());
                break;
            case EGameState.GameOver:
                OnGameStateChanged?.Invoke(_state);
                Time.timeScale = 0f;
                break;
        }
    }

    public void GameOver()
    {
        SetState(EGameState.GameOver);
    }

    private void OnDestroy()
    {
        if (_player != null) _player.OnDied -= GameOver;
    }

}
