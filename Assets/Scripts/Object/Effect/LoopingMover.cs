using UnityEngine;
using DG.Tweening;

public class LoopingMover : MonoBehaviour
{
    [SerializeField] private Vector3 movementDistance = new Vector3(5f, 0f, 0f); // 移動距離
    [SerializeField] private float moveDuration = 1f; // 移動にかかる時間
    [SerializeField] private Ease easeType = Ease.Linear; // イージングの種類
    private Vector3 originalPosition; // 元の位置

    private void Start()
    {
        originalPosition = transform.localPosition; // 元の位置を記録
        StartLoopingMovement(); // ループする移動を開始
    }

    // ループする移動を開始
    private void StartLoopingMovement()
    {
        MoveInPositiveDirection();
    }

    // 正の方向へ移動
    private void MoveInPositiveDirection()
    {
        Vector3 targetPosition = originalPosition + movementDistance; // 移動先の座標

        transform.DOLocalMove(targetPosition, moveDuration)
            .SetEase(easeType)
            .OnComplete(() => MoveInNegativeDirection()); // 移動完了後、負の方向へ移動
    }

    // 負の方向へ移動
    private void MoveInNegativeDirection()
    {
        Vector3 targetPosition = originalPosition - movementDistance; // 負の移動先の座標

        transform.DOLocalMove(targetPosition, moveDuration)
            .SetEase(easeType)
            .OnComplete(() => MoveInPositiveDirection()); // 移動完了後、正の方向へ移動
    }
}
