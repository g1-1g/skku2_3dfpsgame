using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginScene : MonoBehaviour
{
    // 로그인씬 (로그인/회원가입) -> 게임씬

    private enum ESceneMode
    {
        Login,
        Register
    }

    private ESceneMode _mode = ESceneMode.Login;

    // 비밀번호 확인 오브젝트
    [SerializeField] private GameObject _passwordConfirmObject;
    [SerializeField] private Button _gotoRegisterButton;
    [SerializeField] private Button _loginButton;
    [SerializeField] private Button _gotoLoginButton;
    [SerializeField] private Button _registerButton;
    [SerializeField] private TMP_InputField _idInputField;
    [SerializeField] private TMP_InputField _passwordInputField;
    [SerializeField] private TMP_InputField _passwordConfirmInputField;
    [SerializeField] private TextMeshProUGUI _messageText;

    private void Start()
    {
        AddButtonEvents();
        Refresh();
    }

    private void AddButtonEvents()
    {
        _gotoRegisterButton.onClick.AddListener(GotoRegister);
        _loginButton.onClick.AddListener(Login);
        _gotoLoginButton.onClick.AddListener(GotoLogin);
        _registerButton.onClick.AddListener(Register);
    }

    private void Refresh()
    {
        // 2차 비밀번호 오브젝트는 회원가입 모드일때만 노출
        _passwordConfirmObject.SetActive(_mode == ESceneMode.Register);
        _gotoRegisterButton.gameObject.SetActive(_mode == ESceneMode.Login);
        _loginButton.gameObject.SetActive(_mode == ESceneMode.Login);
        _gotoLoginButton.gameObject.SetActive(_mode == ESceneMode.Register);
        _registerButton.gameObject.SetActive(_mode == ESceneMode.Register);
    }

    private void Login()
    {
        string id = _idInputField.text;
        if (string.IsNullOrEmpty(id))
        {
            _messageText.text = "아이디를 입력해주세요.";
            return;
        }

        string password = _passwordInputField.text;
        if (string.IsNullOrEmpty(password))
        {
            _messageText.text = "패스워드를 입력해주세요.";
            return;
        }

        if (!PlayerPrefs.HasKey(id))
        {
            _messageText.text = "아이디를 확인해주세요.";
            return;
        }

        string myPassword = PlayerPrefs.GetString(id);
        if (myPassword != password)
        {
            _messageText.text = "비밀번호를 확인해주세요.";
            return;
        }

        SceneManager.LoadScene("Roading");
    }

    private void Register()
    {
        string id = _idInputField.text;
        if (string.IsNullOrEmpty(id))
        {
            _messageText.text = "아이디를 입력해주세요.";
            return;
        }

        string password = _passwordInputField.text;
        if (string.IsNullOrEmpty(password))
        {
            _messageText.text = "패스워드를 입력해주세요.";
            return;
        }

        string password2 = _passwordConfirmInputField.text;
        if (string.IsNullOrEmpty(password2) || password != password2)
        {
            _messageText.text = "패스워드가 일치하지 않습니다..";
            return;
        }

        if (PlayerPrefs.HasKey(id))
        {
            _messageText.text = "중복된 아이디입니다.";
            return;
        }

        PlayerPrefs.SetString(id, password);
        GotoLogin();
    }

    private void GotoLogin()
    {
        _mode = ESceneMode.Login;
        Refresh();
    }

    private void GotoRegister()
    {
        _mode = ESceneMode.Register;
        Refresh();
    }

}
