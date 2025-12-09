using System;
using UnityEngine;
using UnityEngine.UI;

public class BombUpdate : MonoBehaviour
{
    [SerializeField] private Animator[] _images;
    void Start()
    {
        BombFactory.Instance.OnBombCreated += remainBombUpdate;
    
    }

    private void remainBombUpdate(int remainBombCount)
    {
        if (remainBombCount >= 0 && remainBombCount < _images.Length) _images[remainBombCount].SetBool("Bomb", true);
    }

    private void OnDestroy()
    {
        if (BombFactory.Instance != null)
        {
            BombFactory.Instance.OnBombCreated -= remainBombUpdate;
        }
    }
}
