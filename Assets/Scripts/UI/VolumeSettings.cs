using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshProの名前空間
using Saito.SoundManager;
using DG.Tweening; // DOTweenの名前空間

public class VolumeSettings : MonoBehaviour
{
    [Header("音量調整スライダー")]
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider seVolumeSlider;

    [Header("音量テキスト")]
    [SerializeField] private TextMeshProUGUI bgmVolumeText; // BGM音量テキスト
    [SerializeField] private TextMeshProUGUI seVolumeText; // SE音量テキスト

    [Header("テキスト拡大設定")]
    [SerializeField] private float scaleFactor = 1.2f; // 拡大倍率
    [SerializeField] private float scaleDuration = 0.3f; // 拡大にかかる時間

    private Vector3 initialBgmVolumeTextScale; // BGMテキストの初期スケール
    private Vector3 initialSeVolumeTextScale; // SEテキストの初期スケール

    public bool onSeVolume = false;

    private void Start()
    {
        // SoundManagerから音量の初期値を取得し、スライダーに設定
        bgmVolumeSlider.value = SoundManager.Instance.bgmMasterVolume;
        seVolumeSlider.value = SoundManager.Instance.seMasterVolume;

        // スライダーの値変更イベントにリスナーを追加
        bgmVolumeSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
        seVolumeSlider.onValueChanged.AddListener(OnSeVolumeChanged);

        // テキストの初期スケールを保存
        initialBgmVolumeTextScale = bgmVolumeText.transform.localScale;
        initialSeVolumeTextScale = seVolumeText.transform.localScale;

        EnableBgmVolumeControl();
    }

    /// <summary>
    /// BGM音量スライダーの値が変更された時の処理
    /// </summary>
    /// <param name="value">新しい音量値</param>
    public void OnBgmVolumeChanged(float value)
    {
        SoundManager.Instance.SetBgmMasterVolume(value);
    }

    /// <summary>
    /// SE音量スライダーの値が変更された時の処理
    /// </summary>
    /// <param name="value">新しい音量値</param>
    public void OnSeVolumeChanged(float value)
    {
        SoundManager.Instance.SetSeMasterVolume(value);
    }

    /// <summary>
    /// BGM音量に指定した値を足すメソッド
    /// </summary>
    /// <param name="amount">足す音量値</param>
    public void AddBgmVolume(float amount)
    {
        float newVolume = SoundManager.Instance.bgmMasterVolume + amount;
        // 音量の範囲を制限する（例: 0から1の間）
        newVolume = Mathf.Clamp(newVolume, 0f, 1f);
        SoundManager.Instance.SetBgmMasterVolume(newVolume);
        bgmVolumeSlider.value = newVolume; // スライダーも更新
    }

    /// <summary>
    /// SE音量に指定した値を足すメソッド
    /// </summary>
    /// <param name="amount">足す音量値</param>
    public void AddSeVolume(float amount)
    {
        float newVolume = SoundManager.Instance.seMasterVolume + amount;
        // 音量の範囲を制限する（例: 0から1の間）
        newVolume = Mathf.Clamp(newVolume, 0f, 1f);
        SoundManager.Instance.SetSeMasterVolume(newVolume);
        seVolumeSlider.value = newVolume; // スライダーも更新
    }

    /// <summary>
    /// SEボリューム調整を有効にするメソッド
    /// </summary>
    public void EnableSeVolumeControl()
    {
        onSeVolume = true;
        ScaleText(seVolumeText, scaleFactor, scaleDuration);
        ResetTextScale(bgmVolumeText, scaleDuration);
    }

    /// <summary>
    /// BGMボリューム調整を有効にするメソッド
    /// </summary>
    public void EnableBgmVolumeControl()
    {
        onSeVolume = false;
        ScaleText(bgmVolumeText, scaleFactor, scaleDuration);
        ResetTextScale(seVolumeText, scaleDuration);
    }

    /// <summary>
    /// テキストを拡大するメソッド
    /// </summary>
    /// <param name="text">拡大するテキスト</param>
    /// <param name="scaleFactor">拡大倍率</param>
    /// <param name="duration">拡大にかかる時間</param>
    public void ScaleText(TextMeshProUGUI text, float scaleFactor, float duration)
    {
        text.transform.DOScale(scaleFactor, duration).OnComplete(() =>
        {
            // 完了時の処理（必要に応じて追加）
        });
    }

    /// <summary>
    /// テキストを元の大きさに戻すメソッド
    /// </summary>
    /// <param name="text">戻すテキスト</param>
    /// <param name="duration">戻すのにかかる時間</param>
    public void ResetTextScale(TextMeshProUGUI text, float duration)
    {
        // 初期スケールを使用して戻す
        if (text == bgmVolumeText)
        {
            text.transform.DOScale(initialBgmVolumeTextScale, duration);
        }
        else if (text == seVolumeText)
        {
            text.transform.DOScale(initialSeVolumeTextScale, duration);
        }
    }
}
