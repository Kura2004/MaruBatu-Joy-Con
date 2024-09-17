using UnityEngine;
using UnityEngine.Rendering.PostProcessing;  // Post-processingの名前空間
using DG.Tweening;  // DoTweenの名前空間

public class ChromaticAberrationEffectController : SingletonMonoBehaviour<ChromaticAberrationEffectController>
{
    [Header("Chromatic Aberration Effect Settings")]
    [SerializeField] private PostProcessVolume postProcessVolume; // インスペクターで設定するPostProcessVolume
    [SerializeField] private float maxIntensity = 1f; // インスペクターで編集可能な最大Intensity
    [SerializeField] private float increaseDuration = 2f; // maxIntensityに向かうアニメーションの時間（秒）
    [SerializeField] private float decreaseDuration = 2f; // 元に戻るアニメーションの時間（秒）
    [SerializeField] private float waitBetweenAnimations = 1f; // アニメーションの間の待機時間（秒）
    [SerializeField] private AnimationType increaseType = AnimationType.Exponential; // 増加のアニメーションタイプ
    [SerializeField] private AnimationType decreaseType = AnimationType.Logarithmic; // 減少のアニメーションタイプ

    private ChromaticAberration chromaticAberration;
    private bool isEffectActive = false;
    private Tween currentTween;

    public enum AnimationType
    {
        Linear,
        Exponential,
        Logarithmic
    }

    // 初期化
    private void Start()
    {
        if (postProcessVolume != null)
        {
            // ChromaticAberrationの設定を取得
            if (!postProcessVolume.profile.TryGetSettings(out chromaticAberration))
            {
                Debug.LogError("PostProcessVolumeにChromatic Aberration設定が見つかりません。正しいプロファイルが設定されているか確認してください。");
            }
        }
        else
        {
            Debug.LogError("PostProcessVolumeが設定されていません。インスペクターで設定してください。");
        }
        StopChromaticAberrationEffect();  // 初期状態でエフェクトを停止
    }

    // エフェクトのOn/Offを切り替えるメソッド
    public void ToggleChromaticAberrationEffect()
    {
        if (isEffectActive)
        {
            StopChromaticAberrationEffect();
        }
        else
        {
            StartChromaticAberrationEffect();
        }
    }

    // エフェクトを開始する
    public void StartChromaticAberrationEffect()
    {
        if (chromaticAberration != null && !isEffectActive)
        {
            isEffectActive = true;
            AnimateChromaticAberrationEffect();
        }
    }

    // エフェクトを停止する
    public void StopChromaticAberrationEffect()
    {
        if (chromaticAberration != null)
        {
            isEffectActive = false;
            if (currentTween != null)
            {
                currentTween.Kill();  // 現在のTweenを停止
                currentTween = null;
            }
            chromaticAberration.intensity.value = 0;  // Intensityをリセット
            Debug.Log("エフェクトを停止しました");
        }
    }

    // Chromatic AberrationのIntensityをアニメーションで永続的に増減させる
    private void AnimateChromaticAberrationEffect()
    {
        Sequence sequence = DOTween.Sequence();

        // IntensityをmaxIntensityに向かって増加させるアニメーション
        sequence.Append(AnimateToValue(maxIntensity, increaseDuration, increaseType));

        // アニメーションの間の待機時間
        sequence.AppendInterval(waitBetweenAnimations);

        // Intensityを0に向かって減少させるアニメーション
        sequence.Append(AnimateToValue(0, decreaseDuration, decreaseType));

        // ループさせる
        sequence.SetLoops(-1);

        // 現在のTweenを保持しておく
        currentTween = sequence;
    }

    // Intensityを目標値までアニメーションさせる
    private Tween AnimateToValue(float targetValue, float duration, AnimationType animationType)
    {
        Ease easeType = Ease.Linear;
        switch (animationType)
        {
            case AnimationType.Exponential:
                easeType = Ease.InQuad;  // 加速アニメーション
                break;
            case AnimationType.Logarithmic:
                easeType = Ease.OutQuad;  // 減速アニメーション
                break;
            case AnimationType.Linear:
            default:
                easeType = Ease.Linear;
                break;
        }

        return DOTween.To(() => chromaticAberration.intensity.value, x => chromaticAberration.intensity.value = x, targetValue, duration)
                      .SetEase(easeType);
    }
}
