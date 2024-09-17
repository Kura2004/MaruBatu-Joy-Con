using UnityEngine;
using TMPro; // TextMeshProを使用するための名前空間
using DG.Tweening; // DOTweenを使用するための名前空間

public class TimeLimitController : SingletonMonoBehaviour<TimeLimitController>
{
    public TMP_Text timerDisplay; // 制限時間を表示するTextMeshPro
    public float currentTime;    // 現在の制限時間
    public bool isTimerRunning;  // タイマーが動いているかどうかを管理

    [Header("Effect Settings")]
    [SerializeField] private float effectTriggerTime = 5f; // エフェクトを再生する時間（秒）
    [SerializeField] private float effectDelay = 0.1f; // エフェクトの再生を遅らせる時間（秒）

    public bool isEffectTriggered = false; // エフェクトが既にトリガーされたかどうか

    private void Start()
    {
        ResetTimer(); // スタート時に制限時間をリセット
        isTimerRunning = false;
    }

    private void Update()
    {
        if (!GameStateManager.Instance.IsBoardSetupComplete) { return; }

        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime; // 毎フレーム時間を減少させる
            if (currentTime <= 0f)
            {
                currentTime = 0f;
                ResetEffect();
                TimeUp(); // 時間が切れた時の処理を呼び出し
                StopTimer(); // タイマーを停止
                return;
            }
            else if (currentTime <= effectTriggerTime && !isEffectTriggered) 
            {
                TriggerEffects(); // エフェクトを再生
            }

            UpdateTimerDisplay(); // 表示を更新
        }
    }

    // 制限時間をリセットするメソッド
    public void ResetTimer()
    {
        currentTime = TimeLimitAdjuster.timeLimit; // TimeLimitAdjusterのtimeLimitを使用してリセット
        isTimerRunning = true; // タイマーを再開
        isEffectTriggered = false; // エフェクトのトリガー状態をリセット
        ResetEffect();
        Debug.Log("時間をリセットします");
    }

    public void ResetEffect()
    {
        var chromatic = ChromaticAberrationEffectController.Instance;
        var vignette = VignetteEffectController.Instance;
        if (chromatic != null && vignette != null)
        {
            ScenesAudio.UnsetSeLoop(Saito.SoundManager.SoundManager.SeSoundData.SE.Heart);
            chromatic.StopChromaticAberrationEffect();
            vignette.StopVignetteEffect();
        }
    }

    // エフェクトを再生するメソッド
    private void TriggerEffects()
    {
        var chromatic = ChromaticAberrationEffectController.Instance;
        var vignette = VignetteEffectController.Instance;
        if (chromatic != null && vignette != null)
        {
            chromatic.StartChromaticAberrationEffect();
            vignette.StartVignetteEffect();
            // DoTweenを使って遅延処理を実装
            DOVirtual.DelayedCall(effectDelay, () =>
            {
                ScenesAudio.SetSeLoop(Saito.SoundManager.SoundManager.SeSoundData.SE.Heart);
            });

            isEffectTriggered = true; // エフェクトを一度だけ再生するためのフラグを設定
        }
    }

    // 制限時間を表示するメソッド
    private void UpdateTimerDisplay()
    {
        timerDisplay.text = currentTime.ToString("F1") + "s"; // 小数点以下1桁で表示
    }

    // 時間切れ時に呼び出されるメソッド
    private void TimeUp()
    {
        Debug.Log("Time is up!");
        ScenesAudio.WinSe();
        FadeManager.Instance.LoadScene("GameOver", 1.0f);
    }

    // タイマーの減少を停止するメソッド
    public void StopTimer()
    {
        isTimerRunning = false;
    }

    // タイマーを再開するメソッド
    public void StartTimer()
    {
        isTimerRunning = true;
    }
}
