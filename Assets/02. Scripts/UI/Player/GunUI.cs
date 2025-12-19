using System;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GunUI : MonoBehaviour
{
    [SerializeField] private PlayerGunFire _gunFire;
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
    [SerializeField] private Slider _slider;
    void Start()
    {
        _gunFire.OnGunUpdate += GunUIUpdate;
        _gunFire.OnGunReload += GunReloading;
    }

    private void GunReloading(PlayerGunFire.Gun gun)
    {
        _slider.DOValue(Mathf.Clamp(gun.ReloadInterval, 0, 1f), gun.ReloadInterval).OnComplete(() =>
        {
            _slider.value = 0;
        });
    }

    private void GunUIUpdate(PlayerGunFire.Gun gun)
    {
        _textMeshProUGUI.text = $"{gun.CurrentBullet} / {gun.ReserveBullet}";
    }

    private void OnDestroy()
    {
        if (_gunFire == null) return;
        _gunFire.OnGunUpdate -= GunUIUpdate;
        _gunFire.OnGunReload -= GunReloading;
    }


}
