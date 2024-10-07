using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerImageAnimator : MonoBehaviour
{
    [SerializeField] private float moveDuration = 1f; // 補完にかかる時間
    [SerializeField] private float colorDuration = 1f; // 色の変更にかかる時間

    [Header("X軸設定")]
    [SerializeField] private float xMovementDistance = 5f; // X座標の移動距離
    [SerializeField] private Ease xEaseType = Ease.Linear; // X座標のイージングの種類

    [Header("Y軸設定")]
    [SerializeField] private float yMovementDistance = 5f; // Y座標の移動距離
    [SerializeField] private Ease yEaseType = Ease.Linear; // Y座標のイージングの種類
    [SerializeField] private int loopMax = 0; // 最大ループ数

    private int loopCounter = 0; // 現在のループカウント
    private Tween xTween; // X座標のTween
    private Tween yTween; // Y座標のTween

    [Header("スプライト設定")]
    [SerializeField] private List<Image> images; // 複数のImageを保持するリスト

    private void Start()
    {
        // 初期色を黒に設定
        foreach (var image in images)
        {
            if (image != null)
            {
                image.color = Color.black;
            }
        }
        loopCounter = 0;
    }

    // アニメーションを開始
    public void StartMovement()
    {
        xTween = MoveXPositive(); // X座標の正方向に移動し、Tweenを保存
        yTween = MoveYPositive(); // Y座標の正方向に移動し、Tweenを保存
    }

    // アニメーションを停止
    public void StopMovement()
    {
        StopXMovement(); // X軸のTweenを停止
        StopYMovement(); // Y軸のTweenを停止
    }

    // X座標のTweenを停止
    private void StopXMovement()
    {
        if (xTween != null && xTween.IsActive())
        {
            xTween.Kill(); // X軸のTweenを停止
            xTween = null; // Tweenをnullに設定
        }
    }

    // Y座標のTweenを停止
    private void StopYMovement()
    {
        if (yTween != null && yTween.IsActive())
        {
            yTween.Kill(); // Y軸のTweenを停止
            yTween = null; // Tweenをnullに設定
        }
    }

    // X座標の正方向に移動
    private Tween MoveXPositive()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        float targetX = rectTransform.localPosition.x + xMovementDistance; // 現在のX座標に移動距離を足す

        return rectTransform.DOLocalMoveX(targetX, moveDuration)
            .SetEase(xEaseType)
            .OnComplete(() =>
            {

            });
    }

    // X座標の負方向に移動
    private Tween MoveXNegative()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        float targetX = rectTransform.localPosition.x - xMovementDistance; // 現在のX座標から移動距離を引く

        return rectTransform.DOLocalMoveX(targetX, moveDuration)
            .SetEase(xEaseType)
            .OnComplete(() =>
            {

            });
    }

    // Y座標の正方向に移動
    private Tween MoveYPositive()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        float targetY = rectTransform.localPosition.y + yMovementDistance; // 現在のY座標に移動距離を足す

        return rectTransform.DOLocalMoveY(targetY, moveDuration / (2.0f * (float)loopMax))
            .SetEase(yEaseType)
            .OnComplete(() =>
            {
                if (loopCounter < loopMax) // 最大ループ数に達していない場合
                {
                    MoveYNegative(); // Y座標の負方向に移動
                }
                else
                {
                    StopYMovement(); // ループ数に達したら停止
                }
            });
    }

    // Y座標の負方向に移動
    private Tween MoveYNegative()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        float targetY = rectTransform.localPosition.y - yMovementDistance; // 現在のY座標から移動距離を引く

        return rectTransform.DOLocalMoveY(targetY, moveDuration / (2.0f * (float)loopMax))
            .SetEase(yEaseType)
            .OnComplete(() =>
            {
                loopCounter++; // ループカウントを増加
                if (loopCounter < loopMax) // 最大ループ数に達していない場合
                {
                    MoveYPositive(); // Y座標の正方向に移動
                }
                else
                {
                    StopYMovement(); // ループ数に達したら停止
                }
            });
    }

    // スプライトの色を白に変化させる
    public void ChangeSpritesColorToWhite()
    {
        foreach (var image in images)
        {
            if (image != null)
            {
                image.DOColor(Color.white, colorDuration)
                    .SetEase(Ease.InExpo); // 指定した時間で白に補完
            }
        }
    }

    // スプライトの色を引数で受け取った色に補完的に変更する
    public void ChangeSpritesColor(Color targetColor, float duration)
    {
        foreach (var image in images)
        {
            if (image != null)
            {
                image.DOColor(targetColor, duration)
                    .SetEase(Ease.InExpo); // 指定した時間で色を補完
            }
        }
    }

}
