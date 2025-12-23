using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_OptionPopup : MonoBehaviour
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _gameExitButton;
    [SerializeField] private GameObject _OptionPopup;

    private EGameState _state;

    private bool _isPopUp = false;

    public void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameStateChange;
        _continueButton.onClick.AddListener(GameContinue);
        _restartButton.onClick.AddListener(GameRestart);
        _gameExitButton.onClick.AddListener(GameExit);
    }

    private void GameStateChange(EGameState state)
    {
        _state = state;
    }

    private void Update()
    {
        
        if (!(_state == EGameState.Playing || _state == EGameState.GameOver)) return;
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (!_isPopUp)
            {
                Show(); 
            }
            else
            {
                Hide();
            }
        }
    }
    private void GameExit()
    {
        GameManager.Instance.Quit();
    }

    private void GameRestart()
    {
        SceneManager.LoadScene("Roading");
    }

    private void GameContinue()
    {
        Hide();
    }

    public void Show()
    {
        _OptionPopup.SetActive(true);
        _isPopUp = true;
        GameManager.Instance.SetState(EGameState.Pause);
    }

    public void Hide()
    {
        _OptionPopup.SetActive(false);
        _isPopUp = false;
        GameManager.Instance.SetState(EGameState.Playing);
    }

    public void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged -= GameStateChange;
        }
    }
}
