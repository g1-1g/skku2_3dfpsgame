using System;
using TMPro;
using UnityEngine;

public class GunUI : MonoBehaviour
{
    [SerializeField] private PlayerGunFire _gunFire;
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
    void Start()
    {
        _gunFire.GunUpdate += GunUIUpdate;
    }

    private void GunUIUpdate(PlayerGunFire.Gun gun)
    {
        _textMeshProUGUI.text = $"{gun._currentBullet} / {gun._reserveBullet}";
    }
}
