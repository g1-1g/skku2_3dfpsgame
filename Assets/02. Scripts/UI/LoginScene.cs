using System.Text.RegularExpressions;
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

    private string _idPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    private string _passwordPattern = 
        @"^(?=.*[a-z])" +        // 소문자 1개 이상
        @"(?=.*[A-Z])" +         // 대문자 1개 이상
        @"(?=.*[\W_])" +         // 특수문자 1개 이상
        @"[A-Za-z\d\W_]{7,20}$"; // 허용 문자 + 길이

    private void Start()
    {
        AddButtonEvents();
        Refresh();

        LastEmailSetting();
    }

    private void LastEmailSetting()
    {
        string id = PlayerPrefs.GetString("LastEmail");
        if (id != null)
        {
            _idInputField.text = id;
        }
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

        if (!PlayerPrefs.HasKey($"{id}Hash"))
        {
            _messageText.text = "아이디를 확인해주세요.";
            return;
        }

        
        
        if (!PasswordHasher.VerifyPassword(password, PlayerPrefs.GetString($"{id}Hash"),PlayerPrefs.GetString($"{id}Salt")))
        {
            _messageText.text = "비밀번호를 확인해주세요.";
            return;
        }
        PlayerPrefs.SetString("LastEmail", id);
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

        if (!Regex.IsMatch(id, _idPattern))
        {
            _messageText.text = "아이디 형식이 올바르지 않습니다.";
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

        if (!Regex.IsMatch(password, _passwordPattern))
        {
            _messageText.text = "패스워드 형식이 올바르지 않습니다.";
            return;
        }

        if (PlayerPrefs.HasKey($"{id}Hash"))
        {
            _messageText.text = "중복된 아이디입니다.";
            return;
        }

        string salt = PasswordHasher.GenerateSalt();
        PlayerPrefs.SetString($"{id}Salt", salt);
        PlayerPrefs.SetString($"{id}Hash", PasswordHasher.HashPassword(password, salt));
        _messageText.text = "";
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
