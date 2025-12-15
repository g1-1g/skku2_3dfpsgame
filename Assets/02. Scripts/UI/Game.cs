using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    private TextMeshProUGUI _stateText;

    void Start()
    {
        GameManager.Instance.OnGameStateChanged += UIUpdate;
        _stateText = GetComponent<TextMeshProUGUI>();
    }

    private void UIUpdate(EGameState state)
    {
        switch (state)
        {
            case EGameState.Playing:
                StartCoroutine(Play());
                break;
            case EGameState.Ready:
                Ready();
                break;
            case EGameState.GameOver:
                GameOver();
                break;

        }
    }

    private IEnumerator Play()
    {
        _stateText.text = "게임 시작";
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }

    private void Ready()
    {
        _stateText.text = "준비중 ...";
    }

    private void GameOver()
    {
        gameObject.SetActive(true);
        _stateText.text = "게임 오버";
    }
}
