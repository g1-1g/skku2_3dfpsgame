using TMPro;
using UnityEngine;

public class CoinCountUpdate : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;
    void Start()
    {
        GameManager.Instance.OnCoinChanged += CountUpdate;
    }

    private void CountUpdate(int obj)
    {
        _text.text = $"{obj}";
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnCoinChanged -= CountUpdate;
        }
    }
}
