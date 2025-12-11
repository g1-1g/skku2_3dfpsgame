using System;
using UnityEngine;
using UnityEngine.UI;

public class BombUpdate : MonoBehaviour
{
    [SerializeField] private Animator[] _images;
    [SerializeField] private PlayerBombFire _playerBombFire;
    void Start()
    {
        _playerBombFire.OnBombCreated += remainBombUpdate;
    
    }

    private void remainBombUpdate(int remainBombCount)
    {
        if (remainBombCount >= 0 && remainBombCount < _images.Length) _images[remainBombCount].SetBool("Bomb", true);
    }

    private void OnDestroy()
    {
        if (_playerBombFire != null)
        {
            _playerBombFire.OnBombCreated -= remainBombUpdate;
        }
    }
}
