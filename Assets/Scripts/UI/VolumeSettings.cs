using UnityEngine;
using UnityEngine.UI;
using Saito.SoundManager;

public class VolumeSettings : MonoBehaviour
{
    [Header("音量調整スライダー")]
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider seVolumeSlider;

    private void Start()
    {
        // SoundManagerから音量の初期値を取得し、スライダーに設定
        bgmVolumeSlider.value = SoundManager.Instance.bgmMasterVolume;
        seVolumeSlider.value = SoundManager.Instance.seMasterVolume;

        // スライダーの値変更イベントにリスナーを追加
        bgmVolumeSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
        seVolumeSlider.onValueChanged.AddListener(OnSeVolumeChanged);
    }

    /// <summary>
    /// BGM音量スライダーの値が変更された時の処理
    /// </summary>
    /// <param name="value">新しい音量値</param>
    private void OnBgmVolumeChanged(float value)
    {
        SoundManager.Instance.SetBgmMasterVolume(value);
    }

    /// <summary>
    /// SE音量スライダーの値が変更された時の処理
    /// </summary>
    /// <param name="value">新しい音量値</param>
    private void OnSeVolumeChanged(float value)
    {
        SoundManager.Instance.SetSeMasterVolume(value);
    }
}

