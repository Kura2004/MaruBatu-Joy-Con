using UnityEngine;
using UnityEngine.Rendering.PostProcessing;  // Post-processingの名前空間
using DG.Tweening;  // DoTweenの名前空間

public class VignetteEffectController : SingletonMonoBehaviour<VignetteEffectController>
{
    [Header("Vignette Effect Settings")]
    [SerializeField] private PostProcessVolume postProcessVolume; // インスペクターで設定するPostProcessVolume
    [SerializeField] private float maxIntensity = 1f; // インスペクターで編集可能な最大Intensity
    [SerializeField] private float increaseDuration = 2f; // maxIntensityに向かうアニメーションの時間（秒）
    [SerializeField] private float decreaseDuration = 2f; // 元に戻るアニメーションの時間（秒）
    [SerializeField] private float waitDuration = 1f; // アニメーション間の待機時間（秒）
    [SerializeField] private AnimationType increaseType = AnimationType.Exponential; // 増加のアニメーションタイプ
    [SerializeField] private AnimationType decreaseType = AnimationType.Logarithmic; // 減少のアニメーションタイプ

    private Vignette vignette;
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
            // Vignetteの設定を取得
            if (!postProcessVolume.profile.TryGetSettings(out vignette))
            {
                Debug.LogError("PostProcessVolumeにVignette設定が見つかりません。正しいプロファイルが設定されているか確認してください。");
            }
        }
        else
        {
            Debug.LogError("PostProcessVolumeが設定されていません。インスペクターで設定してください。");
        }
        StopVignetteEffect();  // 初期状態でエフェクトを停止
    }

    // エフェクトを開始する
    public void StartVignetteEffect()
    {
        if (vignette != null && !isEffectActive)
        {
            isEffectActive = true;
            AnimateVignetteEffect();
        }
    }

    // エフェクトを停止する
    public void StopVignetteEffect()
    {
        if (vignette != null)
        {
            isEffectActive = false;
            if (currentTween != null)
            {
                currentTween.Kill();  // 現在のTweenを停止
                currentTween = null;
            }
            vignette.intensity.value = 0;  // Intensityをリセット
            Debug.Log("エフェクトを停止しました");
        }
    }

    // VignetteのIntensityをアニメーションで永続的に増減させる
    private void AnimateVignetteEffect()
    {
        Sequence sequence = DOTween.Sequence();

        // IntensityをmaxIntensityに向かって増加させるアニメーション
        sequence.Append(AnimateToValue(maxIntensity, increaseDuration, increaseType));

        // アニメーションの間に待機
        sequence.AppendInterval(waitDuration);

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

        return DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, targetValue, duration)
                      .SetEase(easeType);
    }
}
