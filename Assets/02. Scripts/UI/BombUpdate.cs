using System;
using UnityEngine;
using UnityEngine.UI;

public class BombUpdate : MonoBehaviour
{
    [SerializeField] private Animator[] images;
    void Start()
    {
        BombFactory.Instance.OnBombCreated += remainBombUpdate;
    
    }

    private void remainBombUpdate(int remainBombCount)
    {
        images[remainBombCount].SetBool("Bomb", true);
    }
}
