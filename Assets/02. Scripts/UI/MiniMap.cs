using System;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    [SerializeField] Button _plusButton;
    [SerializeField] Button _minusButton;

    [SerializeField] MiniMapCamera _camera;

    void Start()
    {
        _plusButton.onClick.AddListener(PlusButtonClicked);
        _minusButton.onClick.AddListener(MinusButtonClicked);
    }

    private void MinusButtonClicked()
    {
        _camera.SizeDown();
    }

    private void PlusButtonClicked()
    {
        _camera.SizeUp();
    }

    private void OnDestroy()
    {
        _plusButton.onClick.RemoveAllListeners();
        _minusButton.onClick.RemoveAllListeners();
    }
}
