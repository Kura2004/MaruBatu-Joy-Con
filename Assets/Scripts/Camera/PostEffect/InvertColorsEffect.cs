using UnityEngine;
using DG.Tweening; // DOTween 名前空間を追加

public class InvertColorsEffect : CameraPostProcessingEffect
{
    private float interval = 1f; // エフェクト切り替え間隔
    private float duration = 5f; // アニメーションの総再生時間

    private Sequence sequence;

    // シングルトンのインスタンス
    public static InvertColorsEffect Instance => SingletonMonoBehaviour<InvertColorsEffect>.Instance;

    public void StartAnimation(float newInterval, float newDuration)
    {
        interval = newInterval;
        duration = newDuration;

        // DOTweenでアニメーションを設定
        sequence = DOTween.Sequence();

        // エフェクトの切り替えを行うタイミングでアニメーションを作成
        sequence
            .AppendCallback(() => SetEffect(EffectType.InvertColors))
            .AppendInterval(interval)
            .AppendCallback(() => SetEffect(EffectType.None))
            .AppendInterval(interval)
            .SetLoops((int)(duration / (2 * interval)), LoopType.Yoyo) // エフェクトを交互に切り替え
            .OnKill(() => SetEffect(EffectType.None)); // アニメーションが終了したらエフェクトを無効にする
    }

    public void StopAnimation()
    {
        if (sequence != null)
        {
            sequence.Kill(); // DOTweenで作成したアニメーションを停止
        }
        SetEffect(EffectType.None);
    }
}

