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

    [SerializeField] private TextMeshProUGUI _stateText;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }
    private void Start()
    {
        _state = EGameState.Ready;
        _stateText.text = "준비중...";
        StartCoroutine(StartToPlay_Coroutine());
    }

    private IEnumerator StartToPlay_Coroutine()
    {
        yield return new WaitForSeconds(2f);

        _stateText.text = "시작!";

        yield return new WaitForSeconds(0.2f);

        _state = EGameState.Playing;

        _stateText.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        _stateText.gameObject.SetActive(true);
        _stateText.text = "게임 오버";
        _state = EGameState.GameOver;
    }

}
