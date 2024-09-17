using UnityEngine;
using DG.Tweening;

public class ContinuousScaling : MonoBehaviour
{
    [SerializeField] private float scalingRate = 1.1f;
    [SerializeField] private float scalingDuration = 1.0f; // スケールの増加にかかる時間
    [SerializeField] private bool loopScaling = true; // スケールをループさせるかどうか

    private void Start()
    {
        StartScaling();
    }

    private void StartScaling()
    {
        // オブジェクトのスケールを増加させる
        transform.DOScale(transform.localScale * scalingRate, scalingDuration)
            .SetEase(Ease.Linear)
            .SetLoops(loopScaling ? -1 : 0, LoopType.Incremental); // ループさせてスケールを増加し続ける
    }
}

