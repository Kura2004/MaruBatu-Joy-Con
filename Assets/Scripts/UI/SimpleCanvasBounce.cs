using UnityEngine;
using DG.Tweening;

public class SimpleCanvasBounce : CanvasBounce
{

    protected override void Update()
    {
        // キャンバスが落下する条件
        if (ShouldDropCanvas())
        {
            DropCanvas();
            Debug.Log("キャンバスが落下します");
        }

        // バウンドが完了している場合、かつ Q キーが押されたときにキャンバスを上昇させる
        if (Input.GetKeyDown(KeyCode.Q) && !isFalling && isBouncingComplete)
        {
            RiseCanvas();

            if (dropOnStart)
            {
                // 必要な処理があればここに追加できます
                dropOnStart = false;
            }

            Debug.Log("キャンバスが上昇します");
        }
    }

    protected override void DropCanvas()
    {
        InitializeDrop();
        // キャンバスオブジェクトをアクティブにする
        canvasObject.SetActive(true);

        // キャンバスを初期の高さに設定
        canvasRectTransform.anchoredPosition = new Vector2(canvasRectTransform.anchoredPosition.x, initialDropHeight);
        // 落下アニメーション
        canvasRectTransform.DOAnchorPosY(groundY, initialBounceDuration).SetEase(Ease.InQuad).OnComplete(Bounce);
    }

    protected override void Bounce()
    {
        float currentBounceHeight = bounceHeight;
        float currentBounceDuration = initialBounceDuration;
        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < bounceCount; i++)
        {
            // バウンドアニメーションが終わった後に特定のメソッドを呼ぶ
            sequence.AppendCallback(() => ScenesAudio.FallSe());

            sequence.Append(canvasRectTransform.DOAnchorPosY(groundY + currentBounceHeight, currentBounceDuration).SetEase(Ease.OutQuad));
            sequence.Append(canvasRectTransform.DOAnchorPosY(groundY, currentBounceDuration).SetEase(Ease.InQuad));

            // 弾む高さと時間を減衰させる
            currentBounceHeight *= heightDampingFactor;
            currentBounceDuration *= durationDampingFactor;
        }

        sequence.OnComplete(() =>
        {
            isFalling = false; // 落下フラグをリセット
            isBouncingComplete = true; // バウンドアニメーションが完了したフラグを設定
            ScenesAudio.FallSe();
        });
        sequence.Play();
    }

    protected override void RiseCanvas()
    {
        if (!isFalling)
        {
            // キャンバスを地面の位置に設定
            canvasRectTransform.anchoredPosition = new Vector2(canvasRectTransform.anchoredPosition.x, groundY);

            // 上昇アニメーション
            canvasRectTransform.DOAnchorPosY(initialDropHeight, riseDuration).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                // アニメーション完了後、キャンバスを非アクティブに設定
                canvasObject.SetActive(false);
            });
        }
        isBlocked = false;
    }
}



