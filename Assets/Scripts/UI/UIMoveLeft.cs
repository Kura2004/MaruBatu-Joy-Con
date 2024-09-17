using UnityEngine;
using DG.Tweening;

public class UIMoveLeft : MonoBehaviour
{
    [SerializeField] private RectTransform targetRectTransform; // 動かす対象のRectTransform
    [SerializeField] private float moveDistance = 100f;         // 動かす距離
    [SerializeField] private float moveDuration = 2f;           // 動かす時間
    private Tween moveTween;  // アニメーションのTweenを保存する変数

    public void StartMove()
    {
        if (moveTween != null && moveTween.IsPlaying())
        {
            moveTween.Kill();
        }

        // 現在の位置を取得
        Vector2 currentPos = targetRectTransform.anchoredPosition;

        // 左方向に移動
        moveTween = targetRectTransform.DOAnchorPosX(currentPos.x - moveDistance, moveDuration)
            .SetEase(Ease.Linear);

        Debug.Log("左に動かしました");
    }
}
